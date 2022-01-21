using System;

namespace Kean.Domain
{
    /// <summary>
    /// 指示事件处理程序的顺序
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class EventHandlerIndexAttribute : Attribute
    {
        /// <summary>
        /// 初始化 Kean.Domain.EventHandlerOrderAttribute 类的新实例
        /// </summary>
        /// <param name="index">序号</param>
        public EventHandlerIndexAttribute(uint index) =>
            Index = index;

        /// <summary>
        /// 获取序号
        /// </summary>
        public uint Index { get; }
    }
}
