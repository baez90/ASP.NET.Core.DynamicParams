using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ASP.NET.Core.DynamicParams.Internals
{
    /// <summary>
    ///     Internal helper class for storing lists of ControllerParameterDescriptors
    /// </summary>
    internal class ActionParameters
    {
        public List<ControllerParameterDescriptor> SingleObjectParameters { get; set; }

        public List<ControllerParameterDescriptor> CollectionParameters { get; set; }
    }
}