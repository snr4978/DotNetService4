using AutoMapper;
using Kean.Application.Query.ViewModels;
using Kean.Infrastructure.Database.Repository.Default.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Kean.Application.Query
{
    /// <summary>
    /// 模型映射配置
    /// </summary>
    public class AutoMapper : Profile
    {
        /// <summary>
        /// 初始化 Kean.Application.Query.AutoMapper 类的新实例
        /// </summary>
        public AutoMapper()
        {
            CreateMap<T_SYS_SECURITY, Blacklist>()
                .ForMember(viewmodel => viewmodel.Address, entity => entity.MapFrom(security => security.SECURITY_VALUE))
                .ForMember(viewmodel => viewmodel.Timestamp, entity => entity.MapFrom(security => security.SECURITY_TIMESTAMP));

            CreateMap<T_SYS_MENU, Menu>()
                .ForMember(viewmodel => viewmodel.Id, entity => entity.MapFrom(menu => menu.MENU_ID))
                .ForMember(viewmodel => viewmodel.Parent, entity => entity.MapFrom(user => user.MENU_PARENT_ID))
                .ForMember(viewmodel => viewmodel.Header, entity => entity.MapFrom(user => user.MENU_HEADER))
                .ForMember(viewmodel => viewmodel.Url, entity => entity.MapFrom(user => user.MENU_URL))
                .ForMember(viewmodel => viewmodel.Icon, entity => entity.MapFrom(user => user.MENU_ICON));

            CreateMap<T_SYS_ROLE, Role>()
                .ForMember(viewmodel => viewmodel.Id, entity => entity.MapFrom(role => role.ROLE_ID))
                .ForMember(viewmodel => viewmodel.Name, entity => entity.MapFrom(role => role.ROLE_NAME))
                .ForMember(viewmodel => viewmodel.Remark, entity => entity.MapFrom(role => role.ROLE_REMARK));

            CreateMap<T_SYS_USER, User>()
                .ForMember(viewmodel => viewmodel.Id, entity => entity.MapFrom(user => user.USER_ID))
                .ForMember(viewmodel => viewmodel.Name, entity => entity.MapFrom(user => user.USER_NAME))
                .ForMember(viewmodel => viewmodel.Account, entity => entity.MapFrom(user => user.USER_ACCOUNT))
                .ForMember(viewmodel => viewmodel.Avatar, entity => entity.MapFrom(user => user.USER_AVATAR));

            CreateMap<dynamic, Message>()
                .ForMember(viewmodel => viewmodel.Id, entity => entity.MapFrom((message, _) => message.MESSAGE_ID))
                .ForMember(viewmodel => viewmodel.Time, entity => entity.MapFrom((message, _) => message.MESSAGE_TIME))
                .ForMember(viewmodel => viewmodel.Subject, entity => entity.MapFrom((message, _) => message.MESSAGE_SUBJECT))
                .ForMember(viewmodel => viewmodel.Content, entity => entity.MapFrom((message, _) => message.MESSAGE_CONTENT))
                .ForMember(viewmodel => viewmodel.Flag, entity => entity.MapFrom((message, _) => message.MESSAGE_FLAG))
                .ForMember(viewmodel => viewmodel.Source, entity => entity.MapFrom((message, _) => new User { Id = message.USER_ID, Name = message.USER_NAME, Avatar = message.USER_AVATAR }));
        }
    }

    /// <summary>
    /// AutoMapper 扩展方法
    /// </summary>
    internal static class AutoMapperExtension
    {
        /// <summary>
        /// 获取映射表达式
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="mapper">映射</param>
        /// <param name="property">属性</param>
        /// <returns>Lambda 表达式</returns>
        internal static Expression<Func<TSource, object>> GetPropertyMapExpression<TSource, TDestination>(this IMapper mapper, string property)
        {
            if (mapper.ConfigurationProvider is global::AutoMapper.Internal.IGlobalConfiguration configuration)
            {
                var map = configuration.FindTypeMapFor<TSource, TDestination>().PropertyMaps.FirstOrDefault(p => p.DestinationName.Equals(property, StringComparison.OrdinalIgnoreCase));
                if (map?.CustomMapExpression != null)
                {
                    return Expression.Lambda<Func<TSource, object>>(Expression.Convert(map.CustomMapExpression.Body, typeof(object)), map.CustomMapExpression.Parameters);
                }
            }
            return null;
        }

        /// <summary>
        /// 获取映射表达式
        /// </summary>
        /// <typeparam name="TSource1">源类型1</typeparam>
        /// <typeparam name="TSource2">源类型2</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="mapper">映射</param>
        /// <param name="property">属性</param>
        /// <returns>Lambda 表达式</returns>
        internal static Expression<Func<TSource1, TSource2, object>> GetPropertyMapExpression<TSource1, TSource2, TDestination>(this IMapper mapper, string property)
        {
            if (mapper.ConfigurationProvider is global::AutoMapper.Internal.IGlobalConfiguration configuration)
            {
                var map = configuration.FindTypeMapFor<dynamic, TDestination>().PropertyMaps.FirstOrDefault(p => p.DestinationName.Equals(property, StringComparison.OrdinalIgnoreCase));
                if (map?.CustomMapFunction != null
                    && map.CustomMapFunction.Body is InvocationExpression invocationExpression
                    && invocationExpression.Expression is MemberExpression memberExpression
                    && memberExpression.Expression is ConstantExpression constantExpression
                    && constantExpression.Value.GetType().GetField("mappingFunction")?.GetValue(constantExpression.Value) is Delegate mappingFunction
                    && mappingFunction.Method.GetCustomAttribute(typeof(SourceAttribute)) is SourceAttribute sourceAttribute)
                {
                    var parameters = new Dictionary<Type, ParameterExpression>()
                    {
                        [typeof(TSource1)] = Expression.Parameter(typeof(TSource1), "p1"),
                        [typeof(TSource2)] = Expression.Parameter(typeof(TSource2), "p2")
                    };
                    return Expression.Lambda<Func<TSource1, TSource2, object>>(
                        Expression.Convert(Expression.PropertyOrField(parameters[sourceAttribute.Type], sourceAttribute.Path), typeof(object)),
                        parameters.Values);
                }
            }
            return null;
        }
    }

    /// <summary>
    /// 指示映射函数中源类型的具体信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    internal class SourceAttribute : Attribute
    {
        /// <summary>
        /// 初始化 Kean.Application.Query.SourceAttribute 类的新实例
        /// </summary>
        /// <param name="type">源类型</param>
        /// <param name="name">源路径</param>
        internal SourceAttribute(Type type, string path)
        {
            Type = type;
            Path = path;
        }

        /// <summary>
        /// 源类型
        /// </summary>
        internal Type Type { get; }

        /// <summary>
        /// 源路径
        /// </summary>
        internal string Path { get; }
    }
}
