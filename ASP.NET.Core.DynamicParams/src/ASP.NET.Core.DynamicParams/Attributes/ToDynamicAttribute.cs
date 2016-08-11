using System;

namespace ASP.NET.Core.DynamicParams.Attributes
{
    /// <summary>
    ///     Meta attribute to mark a param which should be converted to a JDynamic
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ToDynamicAttribute : Attribute
    {
    }
}