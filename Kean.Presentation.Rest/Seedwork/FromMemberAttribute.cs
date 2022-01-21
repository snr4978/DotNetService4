using Kean.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 指定参数仅来自传入的实体成员
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromMemberAttribute : ModelBinderAttribute
    {
        /// <summary>
        /// 初始化 Kean.Presentation.Rest.FromMemberAttribute 类的新实例
        /// </summary>
        public FromMemberAttribute() : base(typeof(ModelBinder)) { }

        /// <summary>
        /// 实体绑定器
        /// </summary>
        private class ModelBinder : IModelBinder
        {
            /*
             * 实现 Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder.BindModelAsync(ModelBindingContext bindingContext) 方法
             */
            public async Task BindModelAsync(ModelBindingContext bindingContext)
            {
                if (bindingContext == null)
                {
                    throw new ArgumentNullException(nameof(bindingContext));
                }
                var json = string.Empty;
                using (var reader = new StreamReader(bindingContext.HttpContext.Request.Body))
                {
                    json = await reader.ReadToEndAsync();
                }
                var value = JsonHelper.Deserialize<JObject>(json)[bindingContext.FieldName];
                if (value != null)
                {
                    bindingContext.Result = ModelBindingResult.Success(value.ToObject(bindingContext.ModelType));
                }
                bindingContext.HttpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(json));
            }
        }
    }
}
