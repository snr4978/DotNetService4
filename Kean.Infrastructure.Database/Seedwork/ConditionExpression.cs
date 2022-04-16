using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Kean.Infrastructure.Database
{
    /// <summary>
    /// 条件表达式
    /// </summary>
    internal sealed class ConditionExpression : ExpressionVisitor
    {
        private readonly StringBuilder _sql = new(); // Sql 语句
        private Dictionary<string, string> _schema; // 对象名
        private string _prefix; // 参数前缀
        private Parameters _param; // 参数
        private string _operator; // 操作符（暂存）
        private string _method; // 方法（暂存）
        private int _sub; // 子查询数

        /// <summary>
        /// 解析表达式
        /// </summary>
        internal static string Build(Expression expression, Dictionary<string, string> schema, string prefix, ref Parameters param)
        {
            var visitor = new ConditionExpression
            {
                _schema = schema,
                _prefix = prefix,
                _param = (param ??= new Parameters())
            };
            visitor.Visit(expression);
            return visitor._sql.ToString();
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        private void SetValue(Expression expression)
        {
            var key = $"p{_param.Count()}";
            var value = GetValue(expression);
            switch (value)
            {
                default:
                    _sql.AppendFormat("{0}{1}", _prefix, key);
                    _param.Add(key, value);
                    break;
                case null:
                    _sql.Replace(" = ", " IS ", _sql.Length - 3, 3).Replace(" <> ", " IS NOT ", _sql.Length - 4, 4);
                    _sql.Append("NULL");
                    break;
                case bool b:
                    _sql.AppendFormat("{0}{1}", _prefix, key);
                    _param.Add(key, b ? 1 : 0);
                    break;
                case Enum e:
                    _sql.AppendFormat("{0}{1}", _prefix, key);
                    _param.Add(key, Convert.ToInt32(e));
                    break;
                case string s:
                    _sql.AppendFormat("{0}{1}", _prefix, key);
                    if (_operator == "LIKE")
                    {
                        _param.Add(key, _method switch
                        {
                            "StartsWith" => $"{s}%",
                            "EndsWith" => $"%{s}",
                            _ => $"%{s}%"
                        });
                    }
                    else
                    {
                        _param.Add(key, value);
                    }
                    break;
                case Query q:
                    if (q.Parameters?.Any() == true)
                    {
                        _sql.AppendFormat("({0})", q.Expression.Replace($"{_prefix}p", $"{_prefix}p_s{_sub}_"));
                        foreach (var item in q.Parameters)
                        {
                            _param.Add($"{_prefix}p_s{_sub}_{item.Key[1..]}", item.Value);
                        }
                    }
                    else
                    {
                        _sql.AppendFormat("({0})", q.Expression);
                    }
                    _sub++;
                    break;
            }
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        private static object GetValue(Expression expression)
        {
            if ((expression is UnaryExpression ue) && ue.NodeType == ExpressionType.Convert)
            {
                expression = ue.Operand;
            }
            switch (expression)
            {
                case MemberExpression me:
                    object value = null;
                    var stack = new Stack<MemberExpression>();
                    var temp = me;
                    while (temp != null)
                    {
                        stack.Push(temp);
                        temp = temp.Expression as MemberExpression;
                    }
                    while (stack.Count > 0)
                    {
                        temp = stack.Pop();
                        if (temp.Expression is ConstantExpression c)
                        {
                            value = c.Value;
                        }
                        switch (temp.Member)
                        {
                            case PropertyInfo pi:
                                value = pi.GetValue(value);
                                break;
                            case FieldInfo fi:
                                value = fi.GetValue(value);
                                break;
                        }
                    }
                    return value;
                case ConstantExpression ce:
                    return ce.Value;
                default:
                    return Expression.Lambda(expression).Compile().DynamicInvoke();
            }
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is ParameterExpression pe && pe.NodeType == ExpressionType.Parameter)
            {
                if (_schema == null)
                {
                    _sql.AppendFormat("[{0}]", node.Member.Name);
                }
                else
                {
                    _sql.AppendFormat("[{0}].[{1}]", _schema[pe.Name], node.Member.Name);
                }
            }
            else
            {
                SetValue(node);
            }
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var op = Operator.Get(node, out Expression left, out Expression right);
            if (op == null)
            {
                SetValue(node);
            }
            else
            {
                _method = node.Method.Name;
                _operator = op;
                _sql.Append('(');
                Visit(left);
                _sql.AppendFormat(" {0} ", _operator);
                Visit(right);
                _sql.Append(')');
            }
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            _sql.Append('(');
            Visit(node.Left);
            if (node.Right is ConstantExpression ce && ce.Value == null)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Equal:
                        _sql.Append(" IS NULL");
                        break;
                    case ExpressionType.NotEqual:
                        _sql.Append(" IS NOT NULL");
                        break;
                    default:
                        _sql.AppendFormat(" {0} ", Operator.Get(node.NodeType));
                        Visit(node.Right);
                        break;
                }
            }
            else
            {
                _sql.AppendFormat(" {0} ", Operator.Get(node.NodeType));
                Visit(node.Right);
            }
            _sql.Append(')');
            return node;
        }

        protected override Expression VisitNew(NewExpression node)
        {
            SetValue(node);
            return node;
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            SetValue(node);
            return node;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Not)
            {
                _sql.AppendFormat("{0} ", Operator.Get(ExpressionType.Not));
                Visit(node.Operand);
            }
            else
            {
                Visit(node.Operand);
            }
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            switch (node.Value)
            {
                default:
                    _sql.Append(node.Value);
                    break;
                case null:
                    _sql.Replace(" = ", " IS ", _sql.Length - 3, 3).Replace(" <> ", " IS NOT ", _sql.Length - 4, 4);
                    _sql.Append("NULL");
                    break;
                case bool b:
                    _sql.Append(b ? 1 : 0);
                    break;
                case Enum e:
                    _sql.Append(Convert.ToInt32(e));
                    break;
                case string s:
                    if (_operator == "LIKE")
                    {
                        _sql.AppendFormat(_method switch
                        {
                            "StartsWith" => "'{0}%'",
                            "EndsWith" => "'%{0}'",
                            _ => "'%{0}%'"
                        }, s);
                    }
                    else
                    {
                        _sql.AppendFormat("'{0}'", s);
                    }
                    break;
            }
            return node;
        }
    }
}
