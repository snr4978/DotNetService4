using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Kean.Infrastructure.Network
{
    /// <summary>
    /// TCP 客户端
    /// </summary>
    public sealed class TcpClient : IDisposable
    {
        private readonly IPEndPoint _endPoint; // 目标网络终结点
        private Socket _socket; // 套接字
        private bool _alive; // 活动标记
        private readonly byte[] _buffer = new byte[1024 * 256]; // 缓冲区

        /// <summary>
        /// 初始化 Kean.Infrastructure.Network.TcpClient 类的新实例
        /// </summary>
        /// <param name="ip">目标 IP</param>
        /// <param name="port">目标端口</param>
        public TcpClient(string ip, int port)
        {
            _endPoint = new(Dns.GetHostAddresses(ip)[0], port);
        }

        /// <summary>
        /// 获取或设置本地端口
        /// </summary>
        public int? LocalPort { get; set; }

        /// <summary>
        /// 获取是否与目标成功建立连接
        /// </summary>
        public bool Connected { get => _socket != null && _socket.Connected; }

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
        /// 打开套接字
        /// </summary>
        public Task Open()
        {
            _socket?.Dispose();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                if (LocalPort.HasValue)
                {
                    _socket.Bind(new IPEndPoint(IPAddress.Any, LocalPort.Value));
                }
                return Task.Factory.FromAsync(_socket.BeginConnect(_endPoint, null, _socket), r =>
                {
                    var socket = r.AsyncState as Socket;
                    try
                    {
                        socket.EndConnect(r);
                        _alive = true;
                        OpenSuccess?.Invoke(this, new() { Socket = socket });
                    }
                    catch (Exception ex)
                    {
                        OpenFail?.Invoke(this, new() { Socket = socket, Exception = ex });
                    }
                });
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
                return Task.Factory.FromAsync(_socket.BeginDisconnect(false, null, _socket), r =>
                {
                    var socket = r.AsyncState as Socket;
                    try
                    {
                        socket.EndDisconnect(r);
                        CloseSuccess?.Invoke(this, new() { Socket = socket });
                    }
                    catch (Exception ex)
                    {
                        CloseFail?.Invoke(this, new() { Socket = socket, Exception = ex });
                    }
                });
            }
            catch (Exception ex)
            {
                CloseFail?.Invoke(this, new() { Socket = _socket, Exception = ex });
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">发送的数据</param>
        public Task Send(params byte[] data)
        {
            if (_alive)
            {
                try
                {
                    return Task.Factory.FromAsync(_socket.BeginSend(data, 0, data.Length, 0, null, _socket), r =>
                    {
                        var socket = r.AsyncState as Socket;
                        try
                        {
                            socket.EndSend(r);
                            SendSuccess?.Invoke(this, new()
                            {
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
                            if (_alive)
                            {
                                _alive = false;
                                Disconnect?.Invoke(this, new() { Socket = socket, Exception = ex });
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    SendFail?.Invoke(this, new()
                    {
                        Socket = _socket,
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
                    Socket = _socket,
                    Data = data,
                    Exception = new SocketException()
                });
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        public Task Receive()
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
                            if (_alive)
                            {
                                _alive = false;
                                Disconnect?.Invoke(this, new() { Socket = socket, Exception = new ArgumentOutOfRangeException() });
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        ReceiveFail?.Invoke(this, new() { Socket = socket, Exception = ex });
                        if (_alive)
                        {
                            _alive = false;
                            Disconnect?.Invoke(this, new() { Socket = socket, Exception = ex });
                        }
                    }
                }
                try
                {
                    return Task.Factory.FromAsync(_socket.BeginReceive(_buffer, 0, _buffer.Length, 0, null, _socket), callback);
                }
                catch (Exception ex)
                {
                    ReceiveFail?.Invoke(this, new() { Socket = _socket, Exception = ex });
                    return Task.CompletedTask;
                }
            }
            else
            {
                ReceiveFail?.Invoke(this, new() { Socket = _socket, Exception = new SocketException() });
                return Task.CompletedTask;
            }
        }

        void IDisposable.Dispose()
        {
            _socket?.Dispose();
        }
    }
}
