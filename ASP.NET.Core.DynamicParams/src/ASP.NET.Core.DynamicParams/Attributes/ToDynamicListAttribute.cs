using System;

namespace ASP.NET.Core.DynamicParams.Attributes
{
    /// <summary>
    ///     Meta attribute to mark params explicit to be converted to a list of dynamics
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ToDynamicListAttribute : Attribute
    {
    }
}