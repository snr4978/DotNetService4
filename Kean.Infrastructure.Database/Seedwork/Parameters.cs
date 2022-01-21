using Dapper;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kean.Infrastructure.Database
{
	/// <summary>
	/// Sql 参数
	/// 继承于 Dapper.DynamicParameters 并隐藏 templates 相关；实现 IEnumerable 接口
	/// </summary>
	public sealed class Parameters : DynamicParameters, IEnumerable<KeyValuePair<string, object>>
	{
		/// <summary>
		/// 覆盖基类方法
		/// "Append a whole object full of params to the dynamic"
		/// </summary>
		public new void AddDynamicParams(object param)
		{
			if (param != null)
			{
				if (param is IEnumerable<KeyValuePair<string, object>> ie)
				{
					foreach (var item in ie)
					{
						Add(item.Key, item.Value);
					}
				}
				else
				{
					foreach (var item in param.GetType().GetProperties())
					{
						Add(item.Name, item.GetValue(param));
					}
				}
			}
		}

		/// <summary>
		/// 返回循环访问集合的枚举数
		/// </summary>
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return ParameterNames.Select(i => new KeyValuePair<string, object>(i, Get<object>(i))).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
