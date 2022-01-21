using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kean.Infrastructure.Database
{
    /// <summary>
    /// 表示对数据库对象的操作
    /// </summary>
    /// <typeparam name="T">数据库对象映射的实体类型</typeparam>
    public interface ISchema<T> 
        where T : IEntity
    {
        /// <summary>
        /// 生成 Sql 查询
        /// </summary>
        /// <param name="expression">投影表达式</param>
        /// <returns>Sql 查询</returns>
        Query Query(Expression<Func<T, object>> expression);

        /// <summary>
        /// 插入新记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        Task<object> Add(T entity);

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        Task<int> Update(T entity);

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="parameters">值</param>
        Task<int> Update(Parameters parameters);

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="parameters">值</param>
        Task<int> Update(dynamic parameters);

        /// <summary>
        /// 删除记录
        /// </summary>
        Task<int> Delete();

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        Task<int> Delete(T entity);

        /// <summary>
        /// 查询单一结果
        /// </summary>
        Task<T> Single();

        /// <summary>
        /// 查询单一结果
        /// </summary>
        /// <param name="expression">投影表达式</param>
        Task<dynamic> Single(Expression<Func<T, dynamic>> expression);

        /// <summary>
        /// 查询结果集合
        /// </summary>
        Task<IEnumerable<T>> Select();

        /// <summary>
        /// 查询结果集合
        /// </summary>
        /// <param name="expression">投影表达式</param>
        Task<IEnumerable<dynamic>> Select(Expression<Func<T, dynamic>> expression);

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="expression">查询表达式</param>
        ISchema<T> Where(Expression<Func<T, bool?>> expression);

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        ISchema<T> Where(string expression);

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        /// <param name="parameters">参数</param>
        ISchema<T> Where(string expression, dynamic parameters);

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        /// <param name="parameters">参数</param>
        ISchema<T> Where(string expression, Parameters parameters);

        /// <summary>
        /// 设置排序条件
        /// </summary>
        /// <param name="expression">排序表达式</param>
        /// <param name="order">排序类型</param>
        ISchema<T> OrderBy(Expression<Func<T, object>> expression, Order order);

        /// <summary>
        /// 设置排序条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        ISchema<T> OrderBy(string expression);

        /// <summary>
        /// 设置分组条件
        /// </summary>
        /// <param name="expression">分组表达式</param>
        ISchema<T> GroupBy(Expression<Func<T, object>> expression);

        /// <summary>
        /// 设置分组条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        ISchema<T> GroupBy(string expression);

        /// <summary>
        /// 设置分组过滤条件
        /// </summary>
        /// <param name="expression">分组条件表达式</param>
        ISchema<T> Having(Expression<Func<T, bool?>> expression);

        /// <summary>
        /// 设置分组过滤条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        ISchema<T> Having(string expression);

        /// <summary>
        /// 设置锁
        /// </summary>
        /// <param name="type">锁类型</param>
        ISchema<T> Lock(Lock type);

        /// <summary>
        /// 设置查询跳过记录数
        /// </summary>
        /// <param name="count">记录数</param>
        ISchema<T> Skip(int count);

        /// <summary>
        /// 设置查询目标记录数
        /// </summary>
        /// <param name="count">记录数</param>
        ISchema<T> Take(int count);

        /// <summary>
        /// 设置去重
        /// </summary>
        ISchema<T> Distinct();
    }

    /// <summary>
    /// 表示对数据库对象的操作
    /// </summary>
    /// <typeparam name="T1">数据库对象映射的实体类型1</typeparam>
    /// <typeparam name="T2">数据库对象映射的实体类型2</typeparam>
    public interface ISchema<T1, T2> 
        where T1 : IEntity 
        where T2 : IEntity
    {
        /// <summary>
        /// 生成 Sql 查询
        /// </summary>
        /// <param name="expression">投影表达式</param>
        /// <returns>Sql 查询</returns>
        Query Query(Expression<Func<T1, T2, object>> expression);

        /// <summary>
        /// 查询单一结果
        /// </summary>
        /// <param name="expression">投影表达式</param>
        Task<dynamic> Single(Expression<Func<T1, T2, dynamic>> expression);

        /// <summary>
        /// 查询结果集合
        /// </summary>
        /// <param name="expression">投影表达式</param>
        Task<IEnumerable<dynamic>> Select(Expression<Func<T1, T2, dynamic>> expression);

        /// <summary>
        /// 设置连接
        /// </summary>
        /// <param name="join">连接方式</param>
        /// <param name="expression">条件表达式</param>
        ISchema<T1, T2> Join(Join join, Expression<Func<T1, T2, bool?>> expression);

        /// <summary>
        /// 设置连接
        /// </summary>
        /// <param name="join">连接方式</param>
        /// <param name="expression">Sql 命令</param>
        ISchema<T1, T2> Join(Join join, string expression);

        /// <summary>
        /// 设置连接
        /// </summary>
        /// <param name="join">连接方式</param>
        /// <param name="expression">Sql 命令</param>
        /// <param name="parameters">参数</param>
        ISchema<T1, T2> Join(Join join, string expression, dynamic parameters);

        /// <summary>
        /// 设置连接
        /// </summary>
        /// <param name="join">连接方式</param>
        /// <param name="expression">Sql 命令</param>
        /// <param name="parameters">参数</param>
        ISchema<T1, T2> Join(Join join, string expression, Parameters parameters);

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="expression">查询表达式</param>
        ISchema<T1, T2> Where(Expression<Func<T1, T2, bool?>> expression);

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        ISchema<T1, T2> Where(string expression);

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        /// <param name="parameters">参数</param>
        ISchema<T1, T2> Where(string expression, dynamic parameters);

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        /// <param name="parameters">参数</param>
        ISchema<T1, T2> Where(string expression, Parameters parameters);

        /// <summary>
        /// 设置排序条件
        /// </summary>
        /// <param name="expression">排序表达式</param>
        /// <param name="order">排序类型</param>
        ISchema<T1, T2> OrderBy(Expression<Func<T1, T2, object>> expression, Order order);

        /// <summary>
        /// 设置排序条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        ISchema<T1, T2> OrderBy(string expression);

        /// <summary>
        /// 设置分组条件
        /// </summary>
        /// <param name="expression">分组表达式</param>
        ISchema<T1, T2> GroupBy(Expression<Func<T1, T2, object>> expression);

        /// <summary>
        /// 设置分组条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        ISchema<T1, T2> GroupBy(string expression);

        /// <summary>
        /// 设置分组过滤条件
        /// </summary>
        /// <param name="expression">分组条件表达式</param>
        ISchema<T1, T2> Having(Expression<Func<T1, T2, bool?>> expression);

        /// <summary>
        /// 设置分组过滤条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        ISchema<T1, T2> Having(string expression);

        /// <summary>
        /// 设置查询跳过记录数
        /// </summary>
        /// <param name="count">记录数</param>
        ISchema<T1, T2> Skip(int count);

        /// <summary>
        /// 设置查询目标记录数
        /// </summary>
        /// <param name="count">记录数</param>
        ISchema<T1, T2> Take(int count);

        /// <summary>
        /// 设置去重
        /// </summary>
        ISchema<T1, T2> Distinct();
    }

    /// <summary>
    /// 表示对数据库对象的操作
    /// </summary>
    /// <typeparam name="T1">数据库对象映射的实体类型1</typeparam>
    /// <typeparam name="T2">数据库对象映射的实体类型2</typeparam>
    /// <typeparam name="T3">数据库对象映射的实体类型3</typeparam>
    /// 
    public interface ISchema<T1, T2, T3>
         where T1 : IEntity
         where T2 : IEntity
         where T3 : IEntity
    {
        /// <summary>
        /// 生成 Sql 查询
        /// </summary>
        /// <param name="expression">投影表达式</param>
        /// <returns>Sql 查询</returns>
        Query Query(Expression<Func<T1, T2, T3, object>> expression);

        /// <summary>
        /// 查询单一结果
        /// </summary>
        /// <param name="expression">投影表达式</param>
        Task<dynamic> Single(Expression<Func<T1, T2, T3, dynamic>> expression);

        /// <summary>
        /// 查询结果集合
        /// </summary>
        /// <param name="expression">投影表达式</param>
        Task<IEnumerable<dynamic>> Select(Expression<Func<T1, T2, T3, dynamic>> expression);

        /// <summary>
        /// 设置连接
        /// </summary>
        /// <param name="join">连接方式</param>
        /// <param name="expression">条件表达式</param>
        ISchema<T1, T2, T3> Join<U1, U2>(Join join, Expression<Func<U1, U2, bool?>> expression)
             where U1 : IEntity
             where U2 : IEntity;

        /// <summary>
        /// 设置连接
        /// </summary>
        /// <param name="join">连接方式</param>
        /// <param name="expression">Sql 命令</param>
        ISchema<T1, T2, T3> Join<U1, U2>(Join join, string expression)
             where U1 : IEntity
             where U2 : IEntity;

        /// <summary>
        /// 设置连接
        /// </summary>
        /// <param name="join">连接方式</param>
        /// <param name="expression">Sql 命令</param>
        /// <param name="parameters">参数</param>
        ISchema<T1, T2, T3> Join<U1, U2>(Join join, string expression, dynamic parameters)
             where U1 : IEntity
             where U2 : IEntity;

        /// <summary>
        /// 设置连接
        /// </summary>
        /// <param name="join">连接方式</param>
        /// <param name="expression">Sql 命令</param>
        /// <param name="parameters">参数</param>
        ISchema<T1, T2, T3> Join<U1, U2>(Join join, string expression, Parameters parameters)
             where U1 : IEntity
             where U2 : IEntity;

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="expression">查询表达式</param>
        ISchema<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool?>> expression);

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        ISchema<T1, T2, T3> Where(string expression);

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        /// <param name="parameters">参数</param>
        ISchema<T1, T2, T3> Where(string expression, dynamic parameters);

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        /// <param name="parameters">参数</param>
        ISchema<T1, T2, T3> Where(string expression, Parameters parameters);

        /// <summary>
        /// 设置排序条件
        /// </summary>
        /// <param name="expression">排序表达式</param>
        /// <param name="order">排序类型</param>
        ISchema<T1, T2, T3> OrderBy(Expression<Func<T1, T2, T3, object>> expression, Order order);

        /// <summary>
        /// 设置排序条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        ISchema<T1, T2, T3> OrderBy(string expression);

        /// <summary>
        /// 设置分组条件
        /// </summary>
        /// <param name="expression">分组表达式</param>
        ISchema<T1, T2, T3> GroupBy(Expression<Func<T1, T2, T3, object>> expression);

        /// <summary>
        /// 设置分组条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        ISchema<T1, T2, T3> GroupBy(string expression);

        /// <summary>
        /// 设置分组过滤条件
        /// </summary>
        /// <param name="expression">分组条件表达式</param>
        ISchema<T1, T2, T3> Having(Expression<Func<T1, T2, T3, bool?>> expression);

        /// <summary>
        /// 设置分组过滤条件
        /// </summary>
        /// <param name="expression">Sql 命令</param>
        ISchema<T1, T2, T3> Having(string expression);

        /// <summary>
        /// 设置查询跳过记录数
        /// </summary>
        /// <param name="count">记录数</param>
        ISchema<T1, T2, T3> Skip(int count);

        /// <summary>
        /// 设置查询目标记录数
        /// </summary>
        /// <param name="count">记录数</param>
        ISchema<T1, T2, T3> Take(int count);

        /// <summary>
        /// 设置去重
        /// </summary>
        ISchema<T1, T2, T3> Distinct();
    }
}
