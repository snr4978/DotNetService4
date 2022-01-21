using System;

namespace Kean.Domain
{
    /// <summary>
    /// 表示工作单元
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 获取是否进入工作单元
        /// </summary>
        bool IsEntered { get; }

        /// <summary>
        /// 进入工作单元
        /// </summary>
        IUnitOfWork Enter();

        /// <summary>
        /// 退出工作单元。如果开启了仓库事务，则提交
        /// </summary>
        IUnitOfWork Exit();
    }
}
