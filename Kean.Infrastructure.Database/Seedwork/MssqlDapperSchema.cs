using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kean.Infrastructure.Database
{
    /// <summary>
    /// 基于 Dapper 的  SQL Server 数据库对象的操作
    /// </summary>
    /// <typeparam name="T">数据库对象映射的实体类型</typeparam>
    internal sealed class MssqlDapperSchema<T> : ISchema<T>
             where T : IEntity
    {
        private const string PREFIX = "@"; // 前缀

        private readonly IDbContext _context; // 上下文
        private readonly string _schema; // 对象
        private IEnumerable<(string property, string name, string alias, IdentifierAttribute identifier)> _columns; //字段
        private string _where; // 条件语句
        private string _order; // 排序语句
        private string _group; // 分组语句
        private string _having; // 分组条件语句
        private string _lock; // 锁类型
        private int _skip; // 跳过数
        private int _take; // 结果数
        private bool _distinct; // 去重标记
        private Parameters _param; // 参数（查询）
        private Parameters _value; // 值（更新）

        internal MssqlDapperSchema(IDbContext context, string name = null)
        {
            _context = context;
            _schema = $"[{name ?? typeof(T).Name.Split('`')[0]}]";
            Initialize();
        }

        internal void Initialize()
        {
            _columns = typeof(T).GetProperties().Select(p =>
            (
                p.Name,
                $"[{p.Name}]",
                $"[{p.Name}]",
                p.GetCustomAttribute<IdentifierAttribute>()
            ));
            _where = null;
            _order = null;
            _group = null;
            _having = null;
            _lock = null;
            _skip = 0;
            _take = 0;
            _distinct = false;
            _param = null;
            _value = null;
        }

        private string BuildInsert()
        {
            return string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                _schema,
                string.Join(',', _columns.Where(c => c.identifier?.Identity != true).Select(c => c.name)),
                string.Join(',', _columns.Where(c => c.identifier?.Identity != true).Select(c => $"{PREFIX}{c.property}"))
            );
        }

        private string BuildUpdate()
        {
            var i = 0;
            return string.Format("UPDATE {0} SET {1} WHERE {2}",
                _schema,
                string.Join(',', _value.ParameterNames.Select(v => $"[{v}]={PREFIX}v{i++}")),
                _where
            );
        }

        private string BuildDelete()
        {
            return string.Format("DELETE FROM {0}{1}",
                _schema,
                _where == null ? string.Empty : $" WHERE {_where}"
            ); ;
        }

        private string BuildSelect()
        {
            var buffer = new StringBuilder("SELECT");
            if (_skip == 0)
            {
                if (_take > 0)
                {
                    buffer.Append($" TOP {_take}");
                }
            }
            if (_distinct)
            {
                buffer.Append(" DISTINCT");
            }
            buffer.Append($" {string.Join(',', _columns.Select(c => c.name == c.alias ? c.name : $"{c.name} AS {c.alias}"))}");
            if (_skip > 0)
            {
                buffer.Append($",ROW_NUMBER() OVER (ORDER BY {_order ?? _columns.First().name}) AS [ROW_NUM]");
            }
            buffer.Append($" FROM {_schema}");
            if (_lock != null)
            {
                buffer.Append($" WITH({_lock})");
            }
            if (_where != null)
            {
                buffer.Append($" WHERE {_where}");
            }
            if (_group != null)
            {
                buffer.Append($" GROUP BY {_group}");
            }
            if (_having != null)
            {
                buffer.Append($" HAVING {_having}");
            }
            if (_skip == 0)
            {
                if (_order != null)
                {
                    buffer.Append($" ORDER BY {_order}");
                }
                return buffer.ToString();
            }
            else
            {
                return string.Format("SELECT{0} {1} FROM ({2}) AS T WHERE [ROW_NUM] > {3}",
                    _take == 0 ? string.Empty : $" TOP {_take}",
                    string.Join(',', _columns.Select(c => c.alias)),
                    buffer,
                    _skip);
            }
        }

        public Query Query(Expression<Func<T, object>> expression)
        {
            var mapping = MappingExpression.Build(expression, null);
            _columns = mapping.Select(i =>
            (
                default(string),
                i.Item1,
                i.Item2,
                default(IdentifierAttribute)
            ));
            var query = new Query
            {
                Expression = BuildSelect(),
                Parameters = _param
            };
            Initialize();
            return query;
        }

        public async Task<object> Add(T entity)
        {
            var sql = BuildInsert();
            var identifier = _columns.SingleOrDefault(c => c.identifier?.Identity == true);
            if (identifier.property == null)
            {
                await _context.ExecuteAsync(sql, entity, _context.Transaction);
                Initialize();
                return null;
            }
            else
            {
                var property = typeof(T).GetProperty(identifier.property);
                //sql = $"{sql};SELECT @@IDENTITY";
                sql = $"{sql};SELECT SCOPE_IDENTITY()";
                var id = Convert.ChangeType(await _context.ExecuteScalarAsync(sql, entity, _context.Transaction), property.PropertyType);
                property.SetValue(entity, id);
                Initialize();
                return id;
            }
        }

        public async Task<int> Update(T entity)
        {
            var i = 0;
            _param = new Parameters();
            _where = string.Join(" AND ", _columns
                .Where(c => c.identifier != null)
                .Select(c =>
                {
                    _param.Add($"p{i}", typeof(T).GetProperty(c.property).GetValue(entity));
                    return $"{c.name}={PREFIX}p{i++}";
                }));
            i = 0;
            _value = new Parameters();
            _param.AddDynamicParams(_columns
                .Where(c => c.identifier == null)
                .ToDictionary(
                    c => $"v{i++}",
                    c =>
                    {
                        var val = typeof(T).GetProperty(c.property).GetValue(entity);
                        _value.Add(c.property, val);
                        return val;
                    }));
            var sql = BuildUpdate();
            var result = await _context.ExecuteAsync(sql, _param, _context.Transaction);
            Initialize();
            return result;
        }

        public async Task<int> Update(Parameters parameters)
        {
            var i = 0;
            _value = parameters;
            _param.AddDynamicParams(parameters
                .ToDictionary(
                    c => $"v{i++}",
                    c => c.Value
                )
            );
            var sql = BuildUpdate();
            var result = await _context.ExecuteAsync(sql, _param, _context.Transaction);
            Initialize();
            return result;
        }

        public async Task<int> Update(dynamic parameters)
        {
            _value = new Parameters();
            _value.AddDynamicParams(parameters);
            return await Update(_value);
        }

        public async Task<int> Delete()
        {
            var sql = BuildDelete();
            var result = await _context.ExecuteAsync(sql, _param, _context.Transaction);
            Initialize();
            return result;
        }

        public async Task<int> Delete(T entity)
        {
            var i = 0;
            _param = new Parameters();
            _where = string.Join(" AND ", _columns
                .Where(c => c.identifier != null)
                .Select(c =>
                {
                    _param.Add($"p{i}", typeof(T).GetProperty(c.property).GetValue(entity));
                    return $"{c.name}={PREFIX}p{i++}";
                }));
            var sql = BuildDelete();
            var result = await _context.ExecuteAsync(sql, _param, _context.Transaction);
            Initialize();
            return result;
        }

        public async Task<T> Single()
        {
            Take(1);
            var records = await Select();
            return records.SingleOrDefault();
        }

        public async Task<dynamic> Single(Expression<Func<T, dynamic>> expression)
        {
            Take(1);
            var records = await Select(expression);
            return records.SingleOrDefault();
        }

        public async Task<IEnumerable<T>> Select()
        {
            var sql = BuildSelect();
            var records = await _context.QueryAsync<T>(sql, _param, _context.Transaction);
            Initialize();
            return records;
        }

        public async Task<IEnumerable<dynamic>> Select(Expression<Func<T, dynamic>> expression)
        {
            var mapping = MappingExpression.Build(expression, null);
            _columns = mapping.Select(i =>
            (
                default(string),
                i.Item1,
                i.Item2,
                default(IdentifierAttribute)
            ));
            var sql = BuildSelect();
            var records = await _context.QueryAsync<dynamic>(sql, _param, _context.Transaction);
            Initialize();
            return records;
        }

        public ISchema<T> Where(Expression<Func<T, bool?>> expression)
        {
            if (_where == null)
            {
                _where = ConditionExpression.Build(expression, null, PREFIX, ref _param);
            }
            else
            {
                _where = $"{_where} AND {ConditionExpression.Build(expression, null, PREFIX, ref _param)}";
            }
            return this;
        }

        public ISchema<T> Where(string expression)
        {
            if (_where == null)
            {
                _where = expression;
            }
            else
            {
                _where = $"{_where} AND {expression}";
            }
            return this;
        }

        public ISchema<T> Where(string expression, dynamic parameters)
        {
            if (_where == null)
            {
                _where = expression;
            }
            else
            {
                _where = $"{_where} AND {expression}";
            }
            _param ??= new Parameters();
            _param.AddDynamicParams(parameters);
            return this;
        }

        public ISchema<T> Where(string expression, Parameters parameters)
        {
            if (_where == null)
            {
                _where = expression;
            }
            else
            {
                _where = $"{_where} AND {expression}";
            }
            if (_param == null)
            {
                _param = parameters;
            }
            else
            {
                foreach (var item in parameters)
                {
                    _param.Add(item.Key, item.Value);
                }
            }
            return this;
        }

        public ISchema<T> OrderBy(Expression<Func<T, object>> expression, Order order)
        {
            var buffer = ColumnExpression.Build(expression, null);
            if (order == Order.Descending)
            {
                buffer = $"{buffer} DESC";
            }
            if (_order == null)
            {
                _order = buffer;
            }
            else
            {
                _order = $"{_order},{buffer}";
            }
            return this;
        }

        public ISchema<T> OrderBy(string expression)
        {
            _order = expression;
            return this;
        }

        public ISchema<T> GroupBy(Expression<Func<T, object>> expression)
        {
            var buffer = ColumnExpression.Build(expression, null);
            if (_group == null)
            {
                _group = buffer;
            }
            else
            {
                _group = $"{_group},{buffer}";
            }
            return this;
        }

        public ISchema<T> GroupBy(string expression)
        {
            _group = expression;
            return this;
        }

        public ISchema<T> Having(Expression<Func<T, bool?>> expression)
        {
            _having = ConditionExpression.Build(expression, null, PREFIX, ref _param);
            return this;
        }

        public ISchema<T> Having(string expression)
        {
            _having = expression;
            return this;
        }

        public ISchema<T> Lock(Lock type)
        {
            _lock = type.ToString().ToUpper();
            return this;
        }

        public ISchema<T> Skip(int count)
        {
            _skip = count;
            return this;
        }

        public ISchema<T> Take(int count)
        {
            _take = count;
            return this;
        }

        public ISchema<T> Distinct()
        {
            _distinct = true;
            return this;
        }
    }

    /// <summary>
    /// 基于 Dapper 的  SQL Server 数据库对象的操作
    /// </summary>
    /// <typeparam name="T1">数据库对象映射的实体类型1</typeparam>
    /// <typeparam name="T2">数据库对象映射的实体类型2</typeparam>
    internal sealed class MssqlSchema<T1, T2> : ISchema<T1, T2>
             where T1 : IEntity
             where T2 : IEntity
    {
        private const string PREFIX = "@"; // 前缀

        private readonly IDbContext _context; // 上下文
        private readonly IDictionary<string, string> _schema; // 对象
        private IEnumerable<(string name, string alias)> _columns; //字段
        private string _join; // 连接语句
        private string _where; // 条件语句
        private string _order; // 排序语句
        private string _group; // 分组语句
        private string _having; // 分组条件语句
        private int _skip; // 跳过数
        private int _take; // 结果数
        private bool _distinct; // 去重标记
        private Parameters _param; // 参数

        internal MssqlSchema(IDbContext context, string name1 = null, string name2 = null)
        {
            _context = context;
            _schema = new Dictionary<string, string>
            {
                { typeof(T1).FullName, $"[{name1 ?? typeof(T1).Name.Split('`')[0]}]" },
                { typeof(T2).FullName, $"[{name2 ?? typeof(T2).Name.Split('`')[0]}]" }
            };
            Initialize();
        }

        internal void Initialize()
        {
            _columns = typeof(T1).GetProperties().Select(p =>
            (
                $"[T1].[{p.Name}]",
                $"[{p.Name}]"
            ))
            .Concat(typeof(T2).GetProperties().Select(p =>
            (
                $"[T2].[{p.Name}]",
                $"[{p.Name}]"
            )));
            _join = null;
            _where = null;
            _order = null;
            _group = null;
            _having = null;
            _skip = 0;
            _take = 0;
            _distinct = false;
            _param = null;
        }

        private string BuildSelect()
        {
            if (_join == null)
            {
                throw new Exception("未设置连接条件");
            }
            var buffer = new StringBuilder("SELECT");
            if (_skip == 0)
            {
                if (_take > 0)
                {
                    buffer.Append($" TOP {_take}");
                }
            }
            if (_distinct)
            {
                buffer.Append(" DISTINCT");
            }
            buffer.Append($" {string.Join(',', _columns.Select(c => c.name == c.alias ? c.name : $"{c.name} AS {c.alias}"))}");
            if (_skip > 0)
            {
                buffer.Append($",ROW_NUMBER() OVER (ORDER BY {_order ?? _columns.First().name}) AS [ROW_NUM]");
            }
            buffer.Append($" FROM {_join}");
            if (_where != null)
            {
                buffer.Append($" WHERE {_where}");
            }
            if (_group != null)
            {
                buffer.Append($" GROUP BY {_group}");
            }
            if (_having != null)
            {
                buffer.Append($" HAVING {_having}");
            }
            if (_skip == 0)
            {
                if (_order != null)
                {
                    buffer.Append($" ORDER BY {_order}");
                }
                return buffer.ToString();
            }
            else
            {
                return string.Format("SELECT{0} {1} FROM ({2}) AS T WHERE [ROW_NUM] > {3}",
                    _take == 0 ? string.Empty : $" TOP {_take}",
                    string.Join(',', _columns.Select(c => $"{c.alias}")),
                    buffer,
                    _skip);
            }
        }

        public Query Query(Expression<Func<T1, T2, object>> expression)
        {
            var mapping = MappingExpression.Build(expression, new ExpressionParameters(expression, "T1", "T2"));
            _columns = mapping.Select(i =>
            (
                i.Item1,
                i.Item2
            ));
            var query = new Query
            {
                Expression = BuildSelect(),
                Parameters = _param
            };
            Initialize();
            return query;
        }

        public async Task<dynamic> Single(Expression<Func<T1, T2, dynamic>> expression)
        {
            Take(1);
            var records = await Select(expression);
            return records.SingleOrDefault();
        }

        public async Task<IEnumerable<dynamic>> Select(Expression<Func<T1, T2, dynamic>> expression)
        {
            var mapping = MappingExpression.Build(expression, new ExpressionParameters(expression, "T1", "T2"));
            _columns = mapping.Select(i =>
            (
                i.Item1,
                i.Item2
            ));
            var sql = BuildSelect();
            var records = await _context.QueryAsync<dynamic>(sql, _param, _context.Transaction);
            Initialize();
            return records;
        }

        public ISchema<T1, T2> Join(Join join, Expression<Func<T1, T2, bool?>> expression)
        {
            Join(join, ConditionExpression.Build(expression, new ExpressionParameters(expression, "T1", "T2"), PREFIX, ref _param));
            return this;
        }

        public ISchema<T1, T2> Join(Join join, string expression)
        {
            _join = $"{_schema[typeof(T1).FullName]} AS [T1] {join.ToString().ToUpper()}{(join == Database.Join.Inner ? string.Empty : " OUTER")} JOIN {_schema[typeof(T2).FullName]} AS [T2] ON {expression}";
            return this;
        }

        public ISchema<T1, T2> Join(Join join, string expression, dynamic parameters)
        {
            Join(join, expression);
            _param ??= new Parameters();
            _param.AddDynamicParams(parameters);
            return this;
        }

        public ISchema<T1, T2> Join(Join join, string expression, Parameters parameters)
        {
            Join(join, expression);
            if (_param == null)
            {
                _param = parameters;
            }
            else
            {
                foreach (var item in parameters)
                {
                    _param.Add(item.Key, item.Value);
                }
            }
            return this;
        }

        public ISchema<T1, T2> Where(Expression<Func<T1, T2, bool?>> expression)
        {
            if (_where == null)
            {
                _where = ConditionExpression.Build(expression, new ExpressionParameters(expression, "T1", "T2"), PREFIX, ref _param);
            }
            else
            {
                _where = $"{_where} AND {ConditionExpression.Build(expression, new ExpressionParameters(expression, "T1", "T2"), PREFIX, ref _param)}";
            }
            return this;
        }

        public ISchema<T1, T2> Where(string expression)
        {
            if (_where == null)
            {
                _where = expression;
            }
            else
            {
                _where = $"{_where} AND {expression}";
            }
            return this;
        }

        public ISchema<T1, T2> Where(string expression, dynamic parameters)
        {
            if (_where == null)
            {
                _where = expression;
            }
            else
            {
                _where = $"{_where} AND {expression}";
            }
            _param ??= new Parameters();
            _param.AddDynamicParams(parameters);
            return this;
        }

        public ISchema<T1, T2> Where(string expression, Parameters parameters)
        {
            if (_where == null)
            {
                _where = expression;
            }
            else
            {
                _where = $"{_where} AND {expression}";
            }
            if (_param == null)
            {
                _param = parameters;
            }
            else
            {
                foreach (var item in parameters)
                {
                    _param.Add(item.Key, item.Value);
                }
            }
            return this;
        }

        public ISchema<T1, T2> OrderBy(Expression<Func<T1, T2, object>> expression, Order order)
        {
            var buffer = ColumnExpression.Build(expression, new ExpressionParameters(expression, "T1", "T2"));
            if (order == Order.Descending)
            {
                buffer = $"{buffer} DESC";
            }
            if (_order == null)
            {
                _order = buffer;
            }
            else
            {
                _order = $"{_order},{buffer}";
            }
            return this;
        }

        public ISchema<T1, T2> OrderBy(string expression)
        {
            _order = expression;
            return this;
        }

        public ISchema<T1, T2> GroupBy(Expression<Func<T1, T2, object>> expression)
        {
            var buffer = ColumnExpression.Build(expression, new ExpressionParameters(expression, "T1", "T2"));
            if (_group == null)
            {
                _group = buffer;
            }
            else
            {
                _group = $"{_group},{buffer}";
            }
            return this;
        }

        public ISchema<T1, T2> GroupBy(string expression)
        {
            _group = expression;
            return this;
        }

        public ISchema<T1, T2> Having(Expression<Func<T1, T2, bool?>> expression)
        {
            _having = ConditionExpression.Build(expression, new ExpressionParameters(expression, "T1", "T2"), PREFIX, ref _param);
            return this;
        }

        public ISchema<T1, T2> Having(string expression)
        {
            _having = expression;
            return this;
        }

        public ISchema<T1, T2> Skip(int count)
        {
            _skip = count;
            return this;
        }

        public ISchema<T1, T2> Take(int count)
        {
            _take = count;
            return this;
        }

        public ISchema<T1, T2> Distinct()
        {
            _distinct = true;
            return this;
        }
    }

    /// <summary>
    /// 基于 Dapper 的  SQL Server 数据库对象的操作
    /// </summary>
    /// <typeparam name="T1">数据库对象映射的实体类型1</typeparam>
    /// <typeparam name="T2">数据库对象映射的实体类型2</typeparam>
    /// <typeparam name="T3">数据库对象映射的实体类型3</typeparam>
    internal sealed class MssqlSchema<T1, T2, T3> : ISchema<T1, T2, T3>
             where T1 : IEntity
             where T2 : IEntity
             where T3 : IEntity
    {
        private const string PREFIX = "@"; // 前缀

        private readonly IDbContext _context; // 上下文
        private readonly IDictionary<string, string> _schema; // 对象
        private IEnumerable<(string name, string alias)> _columns; //字段
        private string _join; // 连接语句
        private string _where; // 条件语句
        private string _order; // 排序语句
        private string _group; // 分组语句
        private string _having; // 分组条件语句
        private int _skip; // 跳过数
        private int _take; // 结果数
        private bool _distinct; // 去重标记
        private Parameters _param; // 参数
        private List<string> _alias; // 已连接对象

        internal MssqlSchema(IDbContext context, string name1 = null, string name2 = null, string name3 = null)
        {
            _context = context;
            _schema = new Dictionary<string, string>
            {
                { typeof(T1).FullName, $"[{name1 ?? typeof(T1).Name.Split('`')[0]}]" },
                { typeof(T2).FullName, $"[{name2 ?? typeof(T2).Name.Split('`')[0]}]" },
                { typeof(T3).FullName, $"[{name3 ?? typeof(T3).Name.Split('`')[0]}]" }
            };
            Initialize();
        }

        internal void Initialize()
        {
            _columns = typeof(T1).GetProperties().Select(p =>
            (
                $"[T1].[{p.Name}]",
                $"[{p.Name}]"
            ))
            .Concat(typeof(T2).GetProperties().Select(p =>
            (
                $"[T2].[{p.Name}]",
                $"[{p.Name}]"
            )))
            .Concat(typeof(T3).GetProperties().Select(p =>
            (
                $"[T3].[{p.Name}]",
                $"[{p.Name}]"
            )));
            _join = null;
            _where = null;
            _order = null;
            _group = null;
            _having = null;
            _skip = 0;
            _take = 0;
            _distinct = false;
            _param = null;
            (_alias ??= new List<string>()).Clear();
        }

        private string BuildSelect()
        {
            if (_join == null)
            {
                throw new Exception("未设置连接条件");
            }
            var buffer = new StringBuilder("SELECT");
            if (_skip == 0)
            {
                if (_take > 0)
                {
                    buffer.Append($" TOP {_take}");
                }
            }
            if (_distinct)
            {
                buffer.Append(" DISTINCT");
            }
            buffer.Append($" {string.Join(',', _columns.Select(c => c.name == c.alias ? c.name : $"{c.name} AS {c.alias}"))}");
            if (_skip > 0)
            {
                buffer.Append($",ROW_NUMBER() OVER (ORDER BY {_order ?? _columns.First().name}) AS [ROW_NUM]");
            }
            buffer.Append($" FROM {_join}");
            if (_where != null)
            {
                buffer.Append($" WHERE {_where}");
            }
            if (_group != null)
            {
                buffer.Append($" GROUP BY {_group}");
            }
            if (_having != null)
            {
                buffer.Append($" HAVING {_having}");
            }
            if (_skip == 0)
            {
                if (_order != null)
                {
                    buffer.Append($" ORDER BY {_order}");
                }
                return buffer.ToString();
            }
            else
            {
                return string.Format("SELECT{0} {1} FROM ({2}) AS T WHERE [ROW_NUM] > {3}",
                    _take == 0 ? string.Empty : $" TOP {_take}",
                    string.Join(',', _columns.Select(c => $"{c.alias}")),
                    buffer,
                    _skip);
            }
        }

        public Query Query(Expression<Func<T1, T2, T3, object>> expression)
        {
            var mapping = MappingExpression.Build(expression, new ExpressionParameters(expression, "T1", "T2", "T3"));
            _columns = mapping.Select(i =>
            (
                i.Item1,
                i.Item2
            ));
            var query = new Query
            {
                Expression = BuildSelect(),
                Parameters = _param
            };
            Initialize();
            return query;
        }

        public async Task<dynamic> Single(Expression<Func<T1, T2, T3, dynamic>> expression)
        {
            Take(1);
            var records = await Select(expression);
            return records.SingleOrDefault();
        }

        public async Task<IEnumerable<dynamic>> Select(Expression<Func<T1, T2, T3, dynamic>> expression)
        {
            var mapping = MappingExpression.Build(expression, new ExpressionParameters(expression, "T1", "T2", "T3"));
            _columns = mapping.Select(i =>
            (
                i.Item1,
                i.Item2
            ));
            var sql = BuildSelect();
            var records = await _context.QueryAsync<dynamic>(sql, _param, _context.Transaction);
            Initialize();
            return records;
        }

        public ISchema<T1, T2, T3> Join<U1, U2>(Join join, Expression<Func<U1, U2, bool?>> expression)
             where U1 : IEntity
             where U2 : IEntity
        {
            var dic = new Dictionary<string, string>() { { typeof(T1).FullName, "T1" }, { typeof(T2).FullName, "T2" }, { typeof(T3).FullName, "T3" } };
            var alias = new string[] { dic[typeof(U1).FullName], dic[typeof(U2).FullName] };
            Join<U1, U2>(join, ConditionExpression.Build(expression, new ExpressionParameters(expression, alias), PREFIX, ref _param));
            return this;
        }

        public ISchema<T1, T2, T3> Join<U1, U2>(Join join, string expression)
             where U1 : IEntity
             where U2 : IEntity
        {
            var dic = new Dictionary<string, string>() { { typeof(T1).FullName, "T1" }, { typeof(T2).FullName, "T2" }, { typeof(T3).FullName, "T3" } };
            var alias = new string[] { dic[typeof(U1).FullName], dic[typeof(U2).FullName] };
            if (_alias.Count == 0)
            {
                _alias.AddRange(alias);
                _join = $"{_schema[typeof(U1).FullName]} AS [{alias[0]}] {join.ToString().ToUpper()}{(join == Database.Join.Inner ? string.Empty : " OUTER")} JOIN {_schema[typeof(U2).FullName]} AS [{alias[1]}]";
            }
            else
            {
                _join = $"{_join} {join.ToString().ToUpper()}{(join == Database.Join.Inner ? string.Empty : " OUTER")} JOIN";
                if (_alias.Exists(s => s == alias[1]))
                {
                    _alias.Add(alias[0]);
                    _join = $"{_join} {_schema[typeof(U1).FullName]} AS [{alias[0]}]";
                }
                else
                {
                    _alias.Add(alias[1]);
                    _join = $"{_join} {_schema[typeof(U2).FullName]} AS [{alias[1]}]";
                }
            }
            _join = $"{_join} ON {expression}";
            return this;
        }

        public ISchema<T1, T2, T3> Join<U1, U2>(Join join, string expression, dynamic parameters)
             where U1 : IEntity
             where U2 : IEntity
        {
            Join<U1, U2>(join, expression);
            _param ??= new Parameters();
            _param.AddDynamicParams(parameters);
            return this;
        }

        public ISchema<T1, T2, T3> Join<U1, U2>(Join join, string expression, Parameters parameters)
             where U1 : IEntity
             where U2 : IEntity
        {
            Join<U1, U2>(join, expression);
            if (_param == null)
            {
                _param = parameters;
            }
            else
            {
                foreach (var item in parameters)
                {
                    _param.Add(item.Key, item.Value);
                }
            }
            return this;
        }

        public ISchema<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool?>> expression)
        {
            if (_where == null)
            {
                _where = ConditionExpression.Build(expression, new ExpressionParameters(expression, "T1", "T2", "T3"), PREFIX, ref _param);
            }
            else
            {
                _where = $"{_where} AND {ConditionExpression.Build(expression, new ExpressionParameters(expression, "T1", "T2", "T3"), PREFIX, ref _param)}";
            }
            return this;
        }

        public ISchema<T1, T2, T3> Where(string expression)
        {
            if (_where == null)
            {
                _where = expression;
            }
            else
            {
                _where = $"{_where} AND {expression}";
            }
            return this;
        }

        public ISchema<T1, T2, T3> Where(string expression, dynamic parameters)
        {
            if (_where == null)
            {
                _where = expression;
            }
            else
            {
                _where = $"{_where} AND {expression}";
            }
            _param ??= new Parameters();
            _param.AddDynamicParams(parameters);
            return this;
        }

        public ISchema<T1, T2, T3> Where(string expression, Parameters parameters)
        {
            if (_where == null)
            {
                _where = expression;
            }
            else
            {
                _where = $"{_where} AND {expression}";
            }
            if (_param == null)
            {
                _param = parameters;
            }
            else
            {
                foreach (var item in parameters)
                {
                    _param.Add(item.Key, item.Value);
                }
            }
            return this;
        }

        public ISchema<T1, T2, T3> OrderBy(Expression<Func<T1, T2, T3, object>> expression, Order order)
        {
            var buffer = ColumnExpression.Build(expression, new ExpressionParameters(expression, "T1", "T2", "T3"));
            if (order == Order.Descending)
            {
                buffer = $"{buffer} DESC";
            }
            if (_order == null)
            {
                _order = buffer;
            }
            else
            {
                _order = $"{_order},{buffer}";
            }
            return this;
        }

        public ISchema<T1, T2, T3> OrderBy(string expression)
        {
            _order = expression;
            return this;
        }

        public ISchema<T1, T2, T3> GroupBy(Expression<Func<T1, T2, T3, object>> expression)
        {
            var buffer = ColumnExpression.Build(expression, new ExpressionParameters(expression, "T1", "T2", "T3"));
            if (_group == null)
            {
                _group = buffer;
            }
            else
            {
                _group = $"{_group},{buffer}";
            }
            return this;
        }

        public ISchema<T1, T2, T3> GroupBy(string expression)
        {
            _group = expression;
            return this;
        }

        public ISchema<T1, T2, T3> Having(Expression<Func<T1, T2, T3, bool?>> expression)
        {
            _having = ConditionExpression.Build(expression, new ExpressionParameters(expression, "T1", "T2", "T3"), PREFIX, ref _param);
            return this;
        }

        public ISchema<T1, T2, T3> Having(string expression)
        {
            _having = expression;
            return this;
        }

        public ISchema<T1, T2, T3> Skip(int count)
        {
            _skip = count;
            return this;
        }

        public ISchema<T1, T2, T3> Take(int count)
        {
            _take = count;
            return this;
        }

        public ISchema<T1, T2, T3> Distinct()
        {
            _distinct = true;
            return this;
        }
    }
}
