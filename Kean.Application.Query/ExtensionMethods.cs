using AutoMapper;
using Kean.Infrastructure.Database;

namespace Kean.Application.Query
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    internal static class ExtensionMethods
    {
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TViewModel">视图类型</typeparam>
        /// <param name="schema">数据库对象</param>
        /// <param name="sort">排序信息</param>
        /// <param name="mapper">映射器</param>
        /// <returns>数据库对象</returns>
        internal static ISchema<TEntity> Sort<TEntity, TViewModel>(this ISchema<TEntity> schema, string sort, IMapper mapper) where TEntity : IEntity
        {
            if (!string.IsNullOrEmpty(sort))
            {
                var order = sort[0] == '~' ? Order.Descending : Order.Ascending;
                var column = order == Order.Descending ? sort[1..] : sort;
                var expression = mapper.GetPropertyMapExpression<TEntity, TViewModel>(column);
                if (expression != null)
                {
                    schema = schema.OrderBy(expression, order);
                }
            }
            return schema;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="schema">数据库对象</param>
        /// <param name="offset">偏移</param>
        /// <param name="limit">限制</param>
        /// <returns>数据库对象</returns>
        internal static ISchema<TEntity> Page<TEntity>(this ISchema<TEntity> schema, int? offset, int? limit) where TEntity : IEntity
        {
            if (offset.HasValue)
            {
                schema = schema.Skip(offset.Value);
            }
            if (limit.HasValue)
            {
                schema = schema.Take(limit.Value);
            }
            return schema;
        }
    }
}
