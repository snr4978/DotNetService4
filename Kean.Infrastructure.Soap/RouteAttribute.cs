using System;

namespace Kean.Infrastructure.Soap
{
    /// <summary>
    /// 指示 Soap 的路由路径
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RouteAttribute : Attribute
    {
        /// <summary>
        /// 初始化 Kean.Infrastructure.Soap.RouteAttribute 类的新实例
        /// </summary>
        /// <param name="path">路由路径</param>
        public RouteAttribute(string path) =>
            Path = path;

        /// <summary>
        /// 路由路径
        /// </summary>
        public string Path { get; set; }
    }
}
