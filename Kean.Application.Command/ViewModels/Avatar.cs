using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Kean.Application.Command.ViewModels
{
    /// <summary>
    /// 头像视图
    /// </summary>
    public sealed class Avatar : JsonConverter<Avatar>
    {
        /// <summary>
        /// base64 编码
        /// </summary>
        public string Base64Encoding { get; set; }

        /// <summary>
        /// 返回 base64 编码字符串
        /// </summary>
        public override string ToString() => Base64Encoding;

        /// <summary>
        /// Json 序列化
        /// </summary>
        public override void Write(Utf8JsonWriter writer, Avatar value, JsonSerializerOptions options) => writer.WriteStringValue(value);

        /// <summary>
        /// Json 反序列化
        /// </summary>
        public override Avatar Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.GetString();

        /// <summary>
        /// 从字符串隐式转换
        /// </summary>
        public static implicit operator Avatar(string avatar) => new() { Base64Encoding = avatar };

        /// <summary>
        /// 隐式转换为字符串
        /// </summary>
        public static implicit operator string(Avatar avatar) => avatar.Base64Encoding;
    }
}
