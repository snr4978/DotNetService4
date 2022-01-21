using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kean.Infrastructure.Database
{
    /// <summary>
    /// 单列表达式
    /// </summary>
    internal sealed class ColumnExpression : ExpressionVisitor
    {
        private Dictionary<string, string> _schema; // 对象名
        private string _column; // 列

        /// <summary>
        /// 解析表达式
        /// </summary>
        internal static string Build(Expression expression, Dictionary<string, string> schema)
        {
            var visitor = new ColumnExpression
            {
                _schema = schema
            };
            visitor.Visit(expression);
            if (visitor._column != null)
            {
                return visitor._column;
            }
            throw new Exception();
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is ParameterExpression pe && pe.NodeType == ExpressionType.Parameter)
            {
                _column = _schema == null ? $"[{node.Member.Name}]" : $"[{_schema[pe.Name]}].[{node.Member.Name}]";
            }
            return node;
        }

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
