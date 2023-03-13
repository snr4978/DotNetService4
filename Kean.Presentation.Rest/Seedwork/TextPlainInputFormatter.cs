using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// Content-Type: text/plain 输入支持
    /// </summary>
    public sealed class TextPlainInputFormatter : TextInputFormatter
    {
        /// <summary>
        /// 初始化 Kean.Presentation.Rest.TextPlainInputFormatter 类的新实例
        /// </summary>
        public TextPlainInputFormatter()
        {
            SupportedMediaTypes.Add("text/plain");
            SupportedEncodings.Add(Encoding.UTF8);
        }

        /*
         * 实现 Microsoft.AspNetCore.Mvc.Formatters.TextInputFormatter.ReadRequestBodyAsync 方法
         */
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            using var reader = context.ReaderFactory(context.HttpContext.Request.Body, encoding);
            var text = await reader.ReadToEndAsync();
            return InputFormatterResult.Success(text);
        }
    }
}
