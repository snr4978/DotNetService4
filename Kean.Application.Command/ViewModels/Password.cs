using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kean.Application.Command.ViewModels
{
    /// <summary>
    /// 密码视图
    /// </summary>
    public sealed class Password : JsonConverter<Password>
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 当前密码
        /// </summary>
        public string Current { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string Replacement { get; set; }

        /// <summary>
        /// 返回当前密码字符串
        /// </summary>
        public override string ToString() => Current;

        /// <summary>
        /// Json 序列化
        /// </summary>
        public override void Write(Utf8JsonWriter writer, Password value, JsonSerializerOptions options) => writer.WriteStringValue(value);

        /// <summary>
        /// Json 反序列化
        /// </summary>
        public override Password Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.GetString();

        /// <summary>
        /// 从字符串隐式转换
        /// </summary>
        public static implicit operator Password(string password) => new() { Current = password };

        /// <summary>
        /// 隐式转换为字符串
        /// </summary>
        public static implicit operator string(Password password) => password.Current;
    }
}
