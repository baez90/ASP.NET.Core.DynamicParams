using System;
using System.Collections.Generic;
using System.Linq;
using ASP.NET.Core.DynamicParams.Internals;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;

namespace ASP.NET.Core.DynamicParams.Filter
{
    /// <summary>
    ///     Custom ActionFilterAttribute to replace JObjects from Newtonsoft JSON with JDynamic
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class DynamicParamsAttribute : ActionFilterAttribute
    {
        private readonly ParamDetectionMode _detectionMode;

        /// <summary>
        ///     Default attribute constructor
        /// </summary>
        /// <param name="detectionMode">
        ///     ParamDetectionMode to be able to control the kind of param detection.
        ///     In case of implicit detection mode the recognicion may take more time but you don't have to mark all parameters with ToDynamicAttribute or ToDynamicListAttribute.
        ///     In case of explicit detection mode the recognition will be faster and you will be able to select the params which should be replaced.
        /// </param>
        public DynamicParamsAttribute(ParamDetectionMode detectionMode = ParamDetectionMode.Explicit)
        {
            _detectionMode = detectionMode;
        }

        /// <summary>
        ///     Overload of ActionFilterAttribute
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var resolver = new ParameterResolver(context, _detectionMode);
            foreach (var listParam in resolver.ActionParameters.CollectionParameters)
            {
                var jArray = context.ActionArguments[listParam.Name] as IEnumerable<object>;
                if (jArray == null) continue;
                context.ActionArguments[listParam.Name] = jArray.Cast<JObject>().Select(token => new JDynamic(token) as dynamic).ToList();
            }

            foreach (var param in resolver.ActionParameters.SingleObjectParameters)
            {
                var jObjectParam = context.ActionArguments[param.Name] as JToken;
                if (jObjectParam != null)
                {
                    context.ActionArguments[param.Name] = new JDynamic(jObjectParam);
                }
            }
        }
    }

    /// <summary>
    ///     Mode how controller action parameters are getting resolved
    /// </summary>
    public enum ParamDetectionMode
    {
        Explicit,
        Implicit
    }
}