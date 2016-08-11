using System;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace ASP.NET.Core.DynamicParams.Internals
{
    internal class JDynamic : DynamicObject
    {
        private readonly JToken _jObject;

        internal JDynamic(JToken jObject)
        {
            if(jObject == null) throw new ArgumentException();
            _jObject = jObject;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length != 1)
            {
                result = null;
                return false;
            }
            try
            {
                result = new JDynamic(_jObject[indexes[0]]);
                return true;
            }
            catch (ArgumentException )
            {
                result = null;
                return false;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var innerToken = _jObject[binder.Name];
            var success = innerToken != null;
            result = new JDynamic(innerToken);
            return success;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = _jObject.ToObject(binder.ReturnType);
            return result != null;
        }
    }
}