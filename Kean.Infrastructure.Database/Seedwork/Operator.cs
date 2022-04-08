using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace Kean.Infrastructure.Database
{
    /// <summary>
    /// 操作符
    /// </summary>
    public static class Operator
    {
        /// <summary>
        /// 根据表达式类型获取操作符
        /// </summary>
        /// <param name="type">表达式类型</param>
        public static string Get(ExpressionType type)
        {
            return type switch
            {
                ExpressionType.AndAlso => "AND",
                ExpressionType.OrElse => "OR",
                ExpressionType.Not => "NOT",
                ExpressionType.Equal => "=",
                ExpressionType.NotEqual => "<>",
                ExpressionType.GreaterThan => ">",
                ExpressionType.GreaterThanOrEqual => ">=",
                ExpressionType.LessThan => "<",
                ExpressionType.LessThanOrEqual => "<=",
                ExpressionType.Add => "+",
                ExpressionType.Subtract => "-",
                ExpressionType.Multiply => "*",
                ExpressionType.Divide => "/",
                ExpressionType.Modulo => "%",
                _ => null,
            };
        }

        /// <summary>
        /// 根据调用的方法获取操作符
        /// </summary>
        /// <param name="expression">调用方法表达式</param>
        /// <param name="left">左侧表达式</param>
        /// <param name="right">右侧表达式</param>
        public static string Get(MethodCallExpression expression, out Expression left, out Expression right)
        {
            switch (expression.Method.Name)
            {
                case "Contains":
                    if (expression.Method.DeclaringType == typeof(string))
                    {
                        left = expression.Object;
                        right = expression.Arguments[0];
                        return "LIKE";
                    }
                    else if (expression.Method.DeclaringType == typeof(Enumerable))
                    {
                        left = expression.Arguments[1];
                        right = expression.Arguments[0];
                        return "IN";
                    }
                    else if (expression.Method.DeclaringType.IsAssignableTo(typeof(IEnumerable)))
                    {
                        left = expression.Arguments[0];
                        right = expression.Object;
                        return "IN";
                    }
                    else if (expression.Method.DeclaringType == typeof(Query))
                    {
                        left = expression.Arguments[0];
                        right = expression.Object;
                        return "IN";
                    }
                    break;
                case "StartsWith":
                case "EndsWith":
                    if (expression.Method.DeclaringType == typeof(string))
                    {
                        left = expression.Object;
                        right = expression.Arguments[0];
                        return "LIKE";
                    }
                    break;
            }
            left = null;
            right = null;
            return null;
        }
    }
}
