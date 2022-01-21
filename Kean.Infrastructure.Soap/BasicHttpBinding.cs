using System;
using System.Text;

namespace Kean.Infrastructure.Soap
{
    /// <summary>
    /// 基本 HTTP 绑定
    /// </summary>
    public class BasicHttpBinding : System.ServiceModel.BasicHttpBinding
    {
        /// <summary>
        /// 初始化 Kean.Infrastructure.Soap.BasicHttpBinding 类的新实例
        /// </summary>
        public BasicHttpBinding() : base()
        {
            CloseTimeout = new TimeSpan(0, 5, 0);
            OpenTimeout = new TimeSpan(0, 5, 0);
            ReceiveTimeout = new TimeSpan(0, 5, 0);
            SendTimeout = new TimeSpan(0, 5, 0);
            MaxBufferSize = int.MaxValue;
            MaxBufferPoolSize = int.MaxValue;
            MaxReceivedMessageSize = int.MaxValue;
            TextEncoding = Encoding.UTF8;
            ReaderQuotas.MaxArrayLength = int.MaxValue;
            ReaderQuotas.MaxStringContentLength = int.MaxValue;
            ReaderQuotas.MaxBytesPerRead = int.MaxValue;
        }
    }
}
