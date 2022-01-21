using System;
using System.Data;

namespace Kean.Infrastructure.Database.Repository
{
    /// <summary>
    /// 抽象数据库连接
    /// </summary>
    public abstract class AbstractDatabase : IDatabase, IDisposable
    {
        protected readonly IDatabaseCollection _databaseCollection; // 集合
        protected readonly IDriver _driver; // 驱动
        protected IDbContext _context; // 连接上下文
        private bool _transaction; // 是否开启事务

        /// <summary>
        /// 构造函数
        /// </summary>
        public AbstractDatabase(IDatabaseCollection databaseCollection, string config)
        {
            _databaseCollection = databaseCollection;
            _driver = Configuration.Configure(config);
        }

        /*
         * 实现 Kean.Infrastructure.Database.Repository.IDatabase.Context
         */
        public IDbContext Context
        {
            get
            {
                if (_context == null || _context.State == ConnectionState.Closed)
                {
                    _context = _driver.CreateContext();
                    if (!_databaseCollection.Contains(this))
                    {
                        _databaseCollection.Add(this);
                    }
                }
                if (!_transaction)
                {
                    _context.BeginTransaction();
                    _transaction = true;
                }
                return _context;
            }
        }

        /*
         * 实现 System.IDisposable.Dispose()
         */
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Transaction?.Dispose();
                _context.Dispose();
            }
            if (_databaseCollection.Contains(this))
            {
                _databaseCollection.Remove(this);
            }
        }

        /*
         * 实现 Kean.Infrastructure.Databases.Repository.IDatabase.Save()
         */
        public void Save()
        {
            if (_context != null && _transaction)
            {
                _context.Transaction.Commit();
                _transaction = false;
            }
        }

        /*
         * 实现 Kean.Infrastructure.Databases.Repository.IDatabase.Flush()
         */
        public void Flush()
        {
            if (_context != null && _transaction)
            {
                _context.Transaction.Dispose();
                _transaction = false;
            }
        }

        /*
         * 实现 Kean.Infrastructure.Databases.Repository.IDatabase.From<T>()
         */
        public ISchema<T> From<T>()
             where T : IEntity
            => Context.From<T>();

        /*
         * 实现 Kean.Infrastructure.Databases.Repository.IDatabase.From<T>(string name = null)
         */
        public ISchema<T> From<T>(string name = null)
             where T : IEntity 
            => Context.From<T>(name);

        /*
         * 实现 Kean.Infrastructure.Databases.Repository.IDatabase.From<T1, T2>()
         */
        public ISchema<T1, T2> From<T1, T2>()
             where T1 : IEntity
             where T2 : IEntity 
            => Context.From<T1, T2>();

        /*
         * 实现 Kean.Infrastructure.Databases.Repository.IDatabase.From<T1, T2>(string name1 = null, string name2 = null)
         */
        public ISchema<T1, T2> From<T1, T2>(string name1 = null, string name2 = null)
             where T1 : IEntity
             where T2 : IEntity
            => Context.From<T1, T2>(name1, name2);

        /*
         * 实现 Kean.Infrastructure.Databases.Repository.IDatabase.From<T1, T2, T3>()
         */
        public ISchema<T1, T2, T3> From<T1, T2, T3>()
             where T1 : IEntity
             where T2 : IEntity
             where T3 : IEntity
            => Context.From<T1, T2, T3>();

        /*
         * 实现 Kean.Infrastructure.Databases.Repository.IDatabase.From<T1, T2, T3>(string name1 = null, string name2 = null, string name3 = null)
         */
        public ISchema<T1, T2, T3> From<T1, T2, T3>(string name1 = null, string name2 = null, string name3 = null)
             where T1 : IEntity
             where T2 : IEntity
             where T3 : IEntity
            => Context.From<T1, T2, T3>(name1, name2, name3);
    }
}

