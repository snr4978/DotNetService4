using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Kean.Infrastructure.Network
{
    /// <summary>
    /// UDP 端点
    /// </summary>
    public sealed class UdpPeer : IDisposable
    {
        private readonly int _port; // 监听端口
        private readonly Socket _socket; // 套接字
        private readonly byte[] _buffer = new byte[1024 * 256]; // 缓冲区

        /// <summary>
        /// 初始化 Kean.Infrastructure.Network.UdpPeer 类的新实例
        /// </summary>
        /// <param name="port">端口</param>
        public UdpPeer(int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            _socket.Bind(new IPEndPoint(IPAddress.Any, _port = port));
        }

        /// <summary>
        /// 套接字发送数据成功时发生
        /// </summary>
        public event EventHandler<UdpEventArgs> SendSuccess;

        /// <summary>
        /// 套接字发送数据失败时发生
        /// </summary>
        public event EventHandler<UdpEventArgs> SendFail;

        /// <summary>
        /// 套接字接收数据成功时发生
        /// </summary>
        public event EventHandler<UdpEventArgs> ReceiveSuccess;

        /// <summary>
        /// 套接字接收数据失败时发生
        /// </summary>
        public event EventHandler<UdpEventArgs> ReceiveFail;

        /// <summary>
        /// 广播数据
        /// </summary>
        /// <param name="data">广播的数据</param>
        public Task Broadcast(params byte[] data)
        {
            EndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, _port);
            try
            {
                return Task.Factory.FromAsync(_socket.BeginSendTo(data, 0, data.Length, SocketFlags.None, endPoint, null, endPoint), r =>
                {
                    var endPoint = r.AsyncState as EndPoint;
                    try
                    {
                        _socket.EndSendTo(r);
                        SendSuccess?.Invoke(this, new()
                        {
                            EndPoint = endPoint,
                            Data = data
                        });
                    }
                    catch (Exception ex)
                    {
                        SendFail?.Invoke(this, new()
                        {
                            EndPoint = endPoint,
                            Data = data,
                            Exception = ex
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                SendFail?.Invoke(this, new()
                {
                    EndPoint = endPoint,
                    Data = data,
                    Exception = ex
                });
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="endPoint">目标终结点</param>
        /// <param name="data">发送的数据</param>
        public Task Send(EndPoint endPoint, params byte[] data)
        {
            try
            {
                return Task.Factory.FromAsync(_socket.BeginSendTo(data, 0, data.Length, SocketFlags.None, endPoint, null, endPoint), r =>
                {
                    var endPoint = r.AsyncState as EndPoint;
                    try
                    {
                        _socket.EndSendTo(r);
                        SendSuccess?.Invoke(this, new()
                        {
                            EndPoint = endPoint,
                            Data = data
                        });
                    }
                    catch (Exception ex)
                    {
                        SendFail?.Invoke(this, new()
                        {
                            EndPoint = endPoint,
                            Data = data,
                            Exception = ex
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                SendFail?.Invoke(this, new()
                {
                    EndPoint = endPoint,
                    Data = data,
                    Exception = ex
                });
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        public Task Receive()
        {
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, _port);
            void callback(IAsyncResult r)
            {
                var endPoint = r.AsyncState as EndPoint;
                try
                {
                    int length = _socket.EndReceiveFrom(r, ref endPoint);
                    if (length > 0)
                    {
                        byte[] data = new byte[length];
                        Array.Copy(_buffer, data, length);
                        ReceiveSuccess?.Invoke(this, new()
                        {
                            EndPoint = endPoint,
                            Data = data
                        });
                        _socket.BeginReceiveFrom(_buffer, 0, _buffer.Length, SocketFlags.None, ref endPoint, callback, endPoint);
                    }
                }
                catch (Exception ex)
                {
                    ReceiveFail?.Invoke(this, new() { EndPoint = endPoint, Exception = ex });
                }
            }
            try
            {
                return Task.Factory.FromAsync(_socket.BeginReceiveFrom(_buffer, 0, _buffer.Length, SocketFlags.None, ref endPoint, null, endPoint), callback);
            }
            catch (Exception ex)
            {
                ReceiveFail?.Invoke(this, new() { EndPoint = endPoint, Exception = ex });
                return Task.CompletedTask;
            }
        }

        void IDisposable.Dispose()
        {
            _socket?.Dispose();
        }
    }
}
