using System;

namespace Kean.Infrastructure.SignalR
{
    /// <summary>
    /// 指示 Hub 的路由模板
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RouteAttribute : Attribute
    {
        /// <summary>
        /// 初始化 Kean.Infrastructure.Signalr.RouteAttribute 类的新实例
        /// </summary>
        /// <param name="pattern">路由模板</param>
        public RouteAttribute(string pattern) =>
            Pattern = pattern;

        /// <summary>
        /// 路由模板
        /// </summary>
        public string Pattern { get; set; }
    }
}
