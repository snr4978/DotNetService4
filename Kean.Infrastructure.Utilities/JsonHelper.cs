using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Kean.Infrastructure.Utilities
{
    /// <summary>
    /// JSON 序列化和反序列化辅助类
    /// </summary>
    public sealed class JsonHelper
    {
        /// <summary>
        /// 建立 Json 对象
        /// </summary>
        /// <returns>Json 对象</returns>
        public static JObject CreateObject()
        {
            return new JObject();
        }

        /// <summary>
        /// 建立 Json 对象
        /// </summary>
        /// <param name="jsonString">Json 表达式</param>
        /// <returns>Json 对象</returns>
        public static JObject CreateObject(string jsonString)
        {
            try
            {
                return JObject.Parse(jsonString);
            }
            catch
            {
                return new JObject();
            }
        }

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
        /// <typeparam name="T">序列化元素类型</typeparam>
        /// <param name="jsonString">Json 表达式</param>
        /// <returns>序列化元素</returns>
        public static T Deserialize<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}