using Kean.Domain;
using Kean.Infrastructure.Database.Repository;

namespace Kean.Infrastructure.Repository
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseCollection _databaseCollection; // 数据库连接集合

        /*
         * 构造函数
         * 工作单元表示一个操作事务，这里主要考虑一个数据库事务
         */
        public UnitOfWork(IDatabaseCollection databaseCollection)
        {
            _databaseCollection = databaseCollection;
        }

        /*
         * 实现接口 Kean.Domain.IUnitOfWork.IsEntered ，表示是否已打开仓库
         */
        public bool IsEntered { get; private set; }

        /*
         * 实现接口 Kean.Domain.IUnitOfWork.Enter ，表示工作单元打开仓库连接
         */
        public IUnitOfWork Enter()
        {
            IsEntered = true;
            return this;
        }

        /*
         * 实现接口 Kean.Domain.IUnitOfWork.Exit ，表示工作单元提交更改并退出
         * 一般应该表示工作单元的结束
         */
        public IUnitOfWork Exit()
        {
            foreach (var item in _databaseCollection)
            {
                item.Save();
            }
            IsEntered = false;
            return this;
        }

        /*
         * 实现 System.IDisposable.Dispose
         * 释放事务，关闭连接
         */
        public void Dispose()
        {
            foreach (var item in _databaseCollection)
            {
                item.Flush();
            }
            IsEntered = false;
        }
    }
}

