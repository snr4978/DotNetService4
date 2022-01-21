using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// Restful Api 批量操作资源时的数据结构
    /// </summary>
    /// <typeparam name="T">项目数据类型</typeparam>
    public sealed class Batch<T>
    {
        /// <summary>
        /// 获取操作方法
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BatchMethod Method { get; set; }

        /// <summary>
        /// 获取或设置数据
        /// </summary>
        public IEnumerable<T> Data { get; set; }
    }
}
