using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 指定参数仅来自文件上传
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromFileAttribute : ModelBinderAttribute
    {
        /// <summary>
        /// 初始化 Kean.Presentation.Rest.FromFileAttribute 类的新实例
        /// </summary>
        public FromFileAttribute() : base(typeof(ModelBinder)) { }

        /// <summary>
        /// 实体绑定器
        /// </summary>
        private class ModelBinder : IModelBinder
        {
            /*
             * 实现 Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder.BindModelAsync(ModelBindingContext bindingContext) 方法
             */
            public Task BindModelAsync(ModelBindingContext bindingContext)
            {
                if (bindingContext == null)
                {
                    throw new ArgumentNullException(nameof(bindingContext));
                }
                bindingContext.Result = ModelBindingResult.Success(bindingContext.HttpContext.Request.Form.Files);
                return Task.CompletedTask;
            }
        }
    }
}
