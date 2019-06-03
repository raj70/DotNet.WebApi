using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace Rs.App.WebApi.RouteConstraint.Constraints
{
    public class AccountConstraint : IHttpRouteConstraint
    {
        public AccountConstraint()
        {
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route,
            string parameterName,
            IDictionary<string, object> values,
            HttpRouteDirection routeDirection)
        {
            bool isMatch = false;
            //values.TryGetValue(parameterName, out value)
            object value = values[parameterName];
            if (value != null)
            {
                isMatch = IsValidAccount(value.ToString());
            }

            return isMatch;
        }

        public static bool IsValidAccount(string sAccount)
        {
            return (!String.IsNullOrEmpty(sAccount) &&
                sAccount.StartsWith("1234") &&
                sAccount.Length > 5);
        }
    }
}
