using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ASP.NET.Core.DynamicParams.Attributes;
using ASP.NET.Core.DynamicParams.Internals;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;

namespace ASP.NET.Core.DynamicParams.Filter
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class DynamicParamsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var descriptors =
            context.ActionDescriptor.Parameters.Cast<ControllerParameterDescriptor>()
                .Where(descriptor => descriptor != null)
                .ToList();

            var objectParams =descriptors
                    .Where(descriptor => descriptor.ParameterInfo.GetCustomAttribute<ToDynamicAttribute>() != null)
                    .ToList();

            var listParams = descriptors
                .Where(descriptor => descriptor.ParameterInfo.GetCustomAttribute<ToDynamicListAttribute>() != null)
                .ToList();

            foreach (var listParam in listParams)
            {
                var jArray = context.ActionArguments[listParam.Name] as IEnumerable<object>;
                if (jArray == null) continue;
                context.ActionArguments[listParam.Name] = jArray.Cast<JObject>().Select(token => new JObjectDynamic(token) as dynamic).ToList();
            }

            foreach (var param in objectParams)
            {
                var jObjectParam = context.ActionArguments[param.Name] as JToken;
                if (jObjectParam != null)
                {
                    context.ActionArguments[param.Name] = new JObjectDynamic(jObjectParam);
                }
            }
        }
    }
}