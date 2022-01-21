using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// Swagger UI 过滤器
    /// </summary>
    public class SwaggerFilter : IOperationFilter
    {
        /*
         * 实现 Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter.Apply(OpenApiOperation operation, OperationFilterContext context) 方法
         */
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // 向 Swagger UI 中添加 Token 参数
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                operation.Parameters ??= new List<OpenApiParameter>();
                for (int i = 0; i < operation.Parameters.Count; i++)
                {
                    if (descriptor.MethodInfo.GetParameters().FirstOrDefault(p => p.Name == operation.Parameters[i].Name && p.GetCustomAttributes(true).Any(a => a is FromMiddlewareAttribute)) != null)
                    {
                        operation.Parameters.RemoveAt(i);
                        i--;
                    }
                }
                if (!descriptor.MethodInfo.GetCustomAttributes(true).Any(a => a is AnonymousAttribute))
                {
                    operation.Parameters.Insert(0, new()
                    {
                        In = ParameterLocation.Header,
                        Name = "token",
                        Required = true
                    });
                }
            }
        }
    }
}
