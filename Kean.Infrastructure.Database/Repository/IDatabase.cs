namespace Kean.Infrastructure.Database.Repository
{
    /// <summary>
    /// 表示数据库连接
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// 获取数据库连接上下文
        /// </summary>
        IDbContext Context { get; }

        /// <summary>
        /// 保存数据
        /// </summary>
        void Save();

        /// <summary>
        /// 刷新数据
        /// </summary>
        void Flush();

        /// <summary>
        /// 指定对象目标
        /// </summary>
        /// <typeparam name="T">数据库对象映射的实体类型</typeparam>
        ISchema<T> From<T>()
             where T : IEntity;

        /// <summary>
        /// 指定对象目标
        /// </summary>
        /// <typeparam name="T">数据库对象映射的实体类型</typeparam>
        /// <param name="name">对象名</param>
        ISchema<T> From<T>(string name = null)
             where T : IEntity;

        /// <summary>
        /// 指定对象目标
        /// </summary>
        /// <typeparam name="T1">数据库对象映射的实体类型1</typeparam>
        /// <typeparam name="T2">数据库对象映射的实体类型2</typeparam>
        ISchema<T1, T2> From<T1, T2>()
             where T1 : IEntity
             where T2 : IEntity;

        /// <summary>
        /// 指定对象目标
        /// </summary>
        /// <typeparam name="T1">数据库对象映射的实体类型1</typeparam>
        /// <typeparam name="T2">数据库对象映射的实体类型2</typeparam>
        /// <param name="name1">对象名1</param>
        /// <param name="name2">对象名2</param>
        ISchema<T1, T2> From<T1, T2>(string name1 = null, string name2 = null)
             where T1 : IEntity
             where T2 : IEntity;

        /// <summary>
        /// 指定对象目标
        /// </summary>
        /// <typeparam name="T1">数据库对象映射的实体类型1</typeparam>
        /// <typeparam name="T2">数据库对象映射的实体类型2</typeparam>
        /// <typeparam name="T3">数据库对象映射的实体类型3</typeparam>
        ISchema<T1, T2, T3> From<T1, T2, T3>()
             where T1 : IEntity
             where T2 : IEntity
             where T3 : IEntity;

        /// <summary>
        /// 指定对象目标
        /// </summary>
        /// <typeparam name="T1">数据库对象映射的实体类型1</typeparam>
        /// <typeparam name="T2">数据库对象映射的实体类型2</typeparam>
        /// <typeparam name="T3">数据库对象映射的实体类型3</typeparam>
        /// <param name="name1">对象名1</param>
        /// <param name="name2">对象名2</param>
        /// <param name="name3">对象名3</param>
        ISchema<T1, T2, T3> From<T1, T2, T3>(string name1 = null, string name2 = null, string name3 = null)
             where T1 : IEntity
             where T2 : IEntity
             where T3 : IEntity;
    }
}
