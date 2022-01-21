using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Infrastructure.Network
{
    /// <summary>
    /// 心跳
    /// </summary>
    public sealed class Heartbeat : IDisposable
    {
        private readonly Task _task; // 任务
        private readonly CancellationTokenSource _cancellation = new(); // 取消信号
        private readonly ManualResetEvent _reset = new(true); // 恢复信号

        /// <summary>
        /// 初始化 Kean.Infrastructure.Network.Heartbeat 类的新实例
        /// </summary>
        /// <param name="ip">目标 IP</param>
        /// <param name="interval">心跳间隔（毫秒）</param>
        public Heartbeat(string ip, int interval = 10000)
        {
            _task = new Task(async () =>
            {
                var ping = new Ping();
                while (!_cancellation.Token.IsCancellationRequested)
                {
                    _reset.WaitOne();
                    if (ping.Send(ip).Status == IPStatus.Success)
                    {
                        if (Status != true)
                        {
                            Status = true;
                            Success?.Invoke(this, new());
                        }
                    }
                    else
                    {
                        if (Status != false)
                        {
                            Status = false;
                            Fail?.Invoke(this, new());
                        }
                    }
                    await Task.Delay(interval);
                }
            }, _cancellation.Token);
        }

        /// <summary>
        /// 获取最近一次心跳的状态结果
        /// </summary>
        public bool? Status { get; private set; }

        /// <summary>
        /// 心跳成功时发生
        /// </summary>
        public event EventHandler Success;

        /// <summary>
        /// 心跳失败时发生
        /// </summary>
        public event EventHandler Fail;

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            _task.Start();
            _task.ContinueWith(t => t.Dispose());
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            _reset.Reset();
        }

        /// <summary>
        /// 恢复
        /// </summary>
        public void Continue()
        {
            _reset.Set();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _cancellation.Cancel();
        }

        void IDisposable.Dispose()
        {
            Stop();
        }
    }
}
