using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Data;

namespace Kean.Infrastructure.Database
{
    /// <summary>
    /// 基于 Dapper 的 Oracle 连接上下文
    /// </summary>
    internal sealed class OracleDapperContext : IDbContext
    {
        private readonly Hashtable _cache = new();
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        internal OracleDapperContext(string connectionString) => Connection = new OracleConnection(connectionString);
        
        public IDbConnection Connection { get; }

        public IDbTransaction Transaction { get; private set; }

        public ISchema<T> From<T>()
             where T : IEntity 
            => From<T>(null);

        public ISchema<T> From<T>(string name = null)
             where T : IEntity
        {
            var key = name ?? typeof(T).Name;
            if (_cache.ContainsKey(key))
            {
                return _cache[key] as ISchema<T>;
            }
            else
            {
                var schema = new OracleDapperSchema<T>(this, name);
                _cache.Add(key, schema);
                return schema;
            }
        }

        public ISchema<T1, T2> From<T1, T2>()
             where T1 : IEntity
             where T2 : IEntity 
            => From<T1, T2>(null, null);

        public ISchema<T1, T2> From<T1, T2>(string name1 = null, string name2 = null)
             where T1 : IEntity
             where T2 : IEntity
        {
            var key = $"{name1 ?? typeof(T1).Name}&{name2 ?? typeof(T2).Name}";
            if (_cache.ContainsKey(key))
            {
                return _cache[key] as ISchema<T1, T2>;
            }
            else
            {
                var schema = new OracleDapperSchema<T1, T2>(this, name1, name2);
                _cache.Add(key, schema);
                return schema;
            }
        }

        public ISchema<T1, T2, T3> From<T1, T2, T3>()
             where T1 : IEntity
             where T2 : IEntity
             where T3 : IEntity
            => From<T1, T2, T3>(null, null, null);

        public ISchema<T1, T2, T3> From<T1, T2, T3>(string name1 = null, string name2 = null, string name3 = null)
             where T1 : IEntity
             where T2 : IEntity
             where T3 : IEntity
        {
            var key = $"{name1 ?? typeof(T1).Name}&{name2 ?? typeof(T2).Name}&{name3 ?? typeof(T3).Name}";
            if (_cache.ContainsKey(key))
            {
                return _cache[key] as ISchema<T1, T2, T3>;
            }
            else
            {
                var schema = new OracleDapperSchema<T1, T2, T3>(this, name1, name2, name3);
                _cache.Add(key, schema);
                return schema;
            }
        }

        string IDbConnection.ConnectionString
        {
            get => Connection.ConnectionString;
            set => Connection.ConnectionString = value;
        }

        int IDbConnection.ConnectionTimeout => Connection.ConnectionTimeout;

        string IDbConnection.Database => Connection.Database;

        ConnectionState IDbConnection.State => Connection.State;

        IDbTransaction IDbConnection.BeginTransaction() => Transaction = Connection.BeginTransaction();

        IDbTransaction IDbConnection.BeginTransaction(IsolationLevel il) => Transaction = Connection.BeginTransaction(il);

        void IDbConnection.ChangeDatabase(string databaseName) => Connection.ChangeDatabase(databaseName);

        void IDbConnection.Close() => Connection.Close();

        IDbCommand IDbConnection.CreateCommand() => Connection.CreateCommand();

        void IDbConnection.Open() => Connection.Open();

        void IDisposable.Dispose() => Connection.Dispose();
    }
}
