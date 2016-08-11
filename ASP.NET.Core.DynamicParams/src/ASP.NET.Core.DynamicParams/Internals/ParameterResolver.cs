using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ASP.NET.Core.DynamicParams.Attributes;
using ASP.NET.Core.DynamicParams.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ASP.NET.Core.DynamicParams.Internals
{
    /// <summary>
    ///     Internal helper class for parameter recognition
    /// </summary>
    internal class ParameterResolver
    {
        private readonly IReadOnlyList<ControllerParameterDescriptor> _paramDescriptors;

        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="context">Context which holds the action parameters</param>
        /// <param name="detectionMode">mode how parameters should be recognized</param>
        public ParameterResolver(ActionContext context, ParamDetectionMode detectionMode)
        {
            _paramDescriptors = context.ActionDescriptor.Parameters.Cast<ControllerParameterDescriptor>()
                .Where(descriptor => descriptor != null)
                .ToList();

            switch (detectionMode)
            {
                case ParamDetectionMode.Explicit:
                    ActionParameters = GetActionParametersExplicit();
                    break;
                case ParamDetectionMode.Implicit:
                    ActionParameters = GetActionParametersImplicit();
                    break;
                default:
                    ActionParameters = GetActionParametersImplicit();
                    break;
            }
        }

        /// <summary>
        ///     Resolved parameters
        /// </summary>
        internal ActionParameters ActionParameters { get; }

        /// <summary>
        ///     Helper method to recognize parameters based on explicit attributes
        /// </summary>
        /// <returns>ActionParameters instance containing all recognized parameters</returns>
        private ActionParameters GetActionParametersExplicit()
        {
            return new ActionParameters
            {
                SingleObjectParameters = _paramDescriptors
                    .Where(descriptor => descriptor.ParameterInfo.GetCustomAttribute<ToDynamicAttribute>() != null)
                    .ToList(),
                CollectionParameters = _paramDescriptors
                    .Where(descriptor => descriptor.ParameterInfo.GetCustomAttribute<ToDynamicListAttribute>() != null)
                    .ToList()
            };
        }

        /// <summary>
        ///     Helper method to recognize based on implicit reflection
        /// </summary>
        /// <returns>ActionParameters instance containing all recognized parameters</returns>
        private ActionParameters GetActionParametersImplicit()
        {
            return new ActionParameters
            {
                SingleObjectParameters = _paramDescriptors.Where(descriptor => typeof(object).IsAssignableFrom(descriptor.ParameterInfo.ParameterType)).ToList(),
                CollectionParameters = _paramDescriptors.Where(descriptor => typeof(List<dynamic>).IsAssignableFrom(descriptor.ParameterInfo.ParameterType)).ToList()
            };
        }
    }
}