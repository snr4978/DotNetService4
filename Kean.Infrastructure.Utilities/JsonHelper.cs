using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kean.Infrastructure.Utilities
{
    /// <summary>
    /// JSON 序列化和反序列化辅助类
    /// </summary>
    public sealed class JsonHelper
    {
        /// <summary>
        /// Json 序列化
        /// </summary>
        /// <param name="value">序列化元素</param>
        /// <returns>Json 表达式</returns>
        public static string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// Json 序列化
        /// </summary>
        /// <param name="value">序列化元素</param>
        /// <param name="dateTimeFormat">时间格式</param>
        /// <returns>Json 表达式</returns>
        public static string Serialize(object value, string dateTimeFormat)
        {
            return JsonConvert.SerializeObject(value, new IsoDateTimeConverter() { DateTimeFormat = dateTimeFormat });
        }

        /// <summary>
        /// Json 反序列化
        /// </summary>
        /// <param name="jsonString">Json 表达式</param>
        /// <returns>序列化元素</returns>
        public static object Deserialize(string jsonString)
        {
            return JsonConvert.DeserializeObject(jsonString);
        }

        /// <summary>
        /// Json 反序列化
        /// </summary>
        /// <typeparam name="T">序列化元素类型</typeparam>
        /// <param name="jsonString">Json 表达式</param>
        /// <returns>序列化元素</returns>
        public static T Deserialize<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}