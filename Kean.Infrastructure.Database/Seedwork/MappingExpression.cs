using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kean.Infrastructure.Database
{
    /// <summary>
    /// 投影表达式
    /// </summary>
    public sealed class MappingExpression : ExpressionVisitor
    {
        private readonly List<(string, string)> _columns = new(); // 投影列
        private Dictionary<string, string> _schema; // 对象名
        private string _alias; // 别名（暂存）
        private string _function; // 函数（暂存）

        /// <summary>
        /// 解析表达式
        /// </summary>
        public static List<(string, string)> Build(Expression expression, Dictionary<string, string> schema)
        {
            var visitor = new MappingExpression
            {
                _schema = schema
            };
            visitor.Visit(expression);
            if (visitor._columns.Count > 0)
            {
                return visitor._columns;
            }
            throw new Exception();
        }

        /// <summary>
        /// 实现 ExpressionVisitor 类的 VisitMember 方法
        /// </summary>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is ParameterExpression pe && pe.NodeType == ExpressionType.Parameter)
            {
                var column = _schema == null ? $"[{node.Member.Name}]" : $"[{_schema[pe.Name]}].[{node.Member.Name}]";
                _columns.Add((
                    _function == null ? column : $"{_function}({column})",
                    $"[{_alias ?? node.Member.Name}]"
                ));
            }
            _alias = null;
            _function = null;
            return node;
        }

        /// <summary>
        /// 实现 ExpressionVisitor 类的 VisitMethodCall 方法
        /// </summary>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Function))
            {
                _function= node.Method.Name.ToUpper();
                Visit(node.Arguments[0]);
            }
            return node;
        }

        /// <summary>
        /// 实现 ExpressionVisitor 类的 VisitNew 方法
        /// </summary>
        protected override Expression VisitNew(NewExpression node)
        {
            for (int i = 0; i < node.Members.Count; i++)
            {
                _alias = node.Members[i].Name;
                Visit(node.Arguments[i]);
            }
            return node;
        }

        /// <summary>
        /// 实现 ExpressionVisitor 类的 VisitUnary 方法
        /// </summary>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Convert)
            {
                Visit(node.Operand);
            }
            return node;
        }
    }
}
