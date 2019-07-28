using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace Rs.App.WebApi.RouteConstraint.Constraints
{
    public class EnumerationConstraint : IHttpRouteConstraint
    {
        private readonly Type _enum;

        public EnumerationConstraint(string type)
        {
            var t = GetType(type);
            if (t == null || !t.IsEnum)
            {
                throw new ArithmeticException("Argument is not an enum type");
            }
            _enum = t;
        }

        public bool Match(HttpRequestMessage request, 
            IHttpRoute route, 
            string parameterName, 
            IDictionary<string, object> values, 
            HttpRouteDirection routeDirection)
        {
            bool isMatch = false;

            if(values.TryGetValue(parameterName, out object value) && value != null)
            {
                var stringValue = value as string;
                if (!string.IsNullOrWhiteSpace(stringValue))
                {
                    stringValue = stringValue.ToLower();
                    var e = _enum.GetEnumNames().FirstOrDefault(x => x.ToLower() == stringValue);
                    if(e!= null)
                    {
                        isMatch = true;
                    }
                }
            }

            return isMatch;
        }

        private static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);

            if(type == null)
            {
                foreach(var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = a.GetType(typeName);
                    if(type != null)
                    {
                        break;
                    }
                }
            }

            return type;
        }
    }
}