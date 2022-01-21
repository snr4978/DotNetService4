using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Kean.Infrastructure.Network
{
    /// <summary>
    /// TCP 服务端
    /// </summary>
    public sealed class TcpServer : IDisposable
    {
        private readonly int _port; // 监听端口
        private readonly int _backlog; // 挂起连接队列的最大长度
        private Socket _socket; // 套接字
        private bool _alive; // 活动标记
        private readonly byte[] _buffer = new byte[1024 * 256]; // 缓冲区

        /// <summary>
        /// 初始化 Kean.Infrastructure.Network.TcpServer 类的新实例
        /// </summary>
        /// <param name="port">目标端口</param>
        /// <param name="backlog">挂起连接队列的最大长度</param>
        public TcpServer(int port, int backlog = 10)
        {
            _port = port;
            _backlog = backlog;
        }

        /// <summary>
        /// 套接字打开成功时发生
        /// </summary>
        public event EventHandler<TcpEventArgs> OpenSuccess;

        /// <summary>
        /// 套接字打开失败时发生
        /// </summary>
        public event EventHandler<TcpEventArgs> OpenFail;

        /// <summary>
        /// 套接字关闭成功时发生
        /// </summary>
        public event EventHandler<TcpEventArgs> CloseSuccess;

        /// <summary>
        /// 套接字关闭失败时发生
        /// </summary>
        public event EventHandler<TcpEventArgs> CloseFail;

        /// <summary>
        /// 套接字发送数据成功时发生
        /// </summary>
        public event EventHandler<TcpEventArgs> SendSuccess;

        /// <summary>
        /// 套接字发送数据失败时发生
        /// </summary>
        public event EventHandler<TcpEventArgs> SendFail;

        /// <summary>
        /// 套接字接收数据成功时发生
        /// </summary>
        public event EventHandler<TcpEventArgs> ReceiveSuccess;

        /// <summary>
        /// 套接字接收数据失败时发生
        /// </summary>
        public event EventHandler<TcpEventArgs> ReceiveFail;

        /// <summary>
        /// 套接字意外断开时发生
        /// </summary>
        public event EventHandler<TcpEventArgs> Disconnect;

        /// <summary>
        /// 客户端接入成功时发生
        /// </summary>
        public event EventHandler<TcpEventArgs> AcceptSuccess;

        /// <summary>
        /// 客户端接入失败时发生
        /// </summary>
        public event EventHandler<TcpEventArgs> AcceptFail;

        /// <summary>
        /// 打开套接字
        /// </summary>
        public Task Open()
        {
            _socket?.Dispose();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                _socket.Bind(new IPEndPoint(IPAddress.Any, _port));
                _socket.Listen(_backlog);
                void callback(IAsyncResult r)
                {
                    var socket = r.AsyncState as Socket;
                    try
                    {
                        var remote = socket.EndAccept(r);
                        AcceptSuccess?.Invoke(this, new() { Socket = remote });
                        socket.BeginAccept(callback, socket);
                    }
                    catch (Exception ex)
                    {
                        AcceptFail?.Invoke(this, new() { Socket = socket, Exception = ex });
                    }
                }
                var task = Task.Factory.FromAsync(_socket.BeginAccept(null, _socket), callback);
                _alive = true;
                OpenSuccess?.Invoke(this, new() { Socket = _socket });
                return task;
            }
            catch (Exception ex)
            {
                OpenFail?.Invoke(this, new() { Socket = _socket, Exception = ex });
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 关闭套接字
        /// </summary>
        public Task Close()
        {
            try
            {
                _alive = false;
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Dispose();
                CloseSuccess?.Invoke(this, new() { Socket = _socket });
            }
            catch (Exception ex)
            {
                CloseFail?.Invoke(this, new() { Socket = _socket, Exception = ex });
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="data">发送的数据</param>
        public Task Send(Socket socket, params byte[] data)
        {
            if (_alive)
            {
                try
                {
                    return Task.Factory.FromAsync(socket.BeginSend(data, 0, data.Length, 0, null, socket), r =>
                    {
                        var socket = r.AsyncState as Socket;
                        try
                        {
                            socket.EndSend(r);
                            SendSuccess?.Invoke(this, new() { 
                                Socket = socket, 
                                Data = data
                            });
                        }
                        catch (Exception ex)
                        {
                            SendFail?.Invoke(this, new()
                            {
                                Socket = socket,
                                Data = data,
                                Exception = ex
                            });
                            Disconnect?.Invoke(this, new() { Socket = socket, Exception = ex });
                        }
                    });
                }
                catch (Exception ex)
                {
                    SendFail?.Invoke(this, new()
                    {
                        Socket = socket,
                        Data = data,
                        Exception = ex
                    });
                    return Task.CompletedTask;
                }
            }
            else
            {
                SendFail?.Invoke(this, new()
                {
                    Socket = socket,
                    Data = data,
                    Exception = new SocketException()
                });
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="socket">套接字</param>
        public Task Receive(Socket socket)
        {
            if (_alive)
            {
                void callback(IAsyncResult r)
                {
                    var socket = r.AsyncState as Socket;
                    try
                    {
                        int length = socket.EndReceive(r);
                        if (length > 0)
                        {
                            byte[] data = new byte[length];
                            Array.Copy(_buffer, data, length);
                            ReceiveSuccess?.Invoke(this, new()
                            {
                                Socket = socket,
                                Data = data
                            });
                            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, callback, socket);
                        }
                        else
                        {
                            Disconnect?.Invoke(this, new() { Socket = socket, Exception = new ArgumentOutOfRangeException() });
                        }

                    }
                    catch (Exception ex)
                    {
                        ReceiveFail?.Invoke(this, new() { Socket = socket, Exception = ex });
                        Disconnect?.Invoke(this, new() { Socket = socket, Exception = ex });
                    }
                }
                try
                {
                    return Task.Factory.FromAsync(socket.BeginReceive(_buffer, 0, _buffer.Length, 0, null, socket), callback);
                }
                catch (Exception ex)
                {
                    ReceiveFail?.Invoke(this, new() { Socket = socket, Exception = ex });
                    return Task.CompletedTask;
                }
            }
            else
            {
                ReceiveFail?.Invoke(this, new() { Socket = socket, Exception = new SocketException() });
                return Task.CompletedTask;
            }
        }

        void IDisposable.Dispose()
        {
            _socket?.Dispose();
        }
    }
}
