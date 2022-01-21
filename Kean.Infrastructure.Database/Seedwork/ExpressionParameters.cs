using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Kean.Infrastructure.Database
{
    /// <summary>
    /// 表达式参数
    /// </summary>
    internal sealed class ExpressionParameters : Dictionary<string, string>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        internal ExpressionParameters(Expression expression, params string[] alias)
        {
            if (expression is LambdaExpression le)
            {
                if (le.Parameters.Count(p => p.Name == "_") > 1)
                {
                    for (int i = 0; i < le.Parameters.Count; i++)
                    {
                        if (le.Parameters[i].Name != "_")
                        {
                            Add(le.Parameters[i].Name, alias[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < le.Parameters.Count; i++)
                    {
                        Add(le.Parameters[i].Name, alias[i]);
                    }
                }
            }
        }
    }
}
