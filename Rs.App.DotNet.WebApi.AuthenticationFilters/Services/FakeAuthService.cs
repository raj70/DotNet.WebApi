using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rs.App.DotNet.WebApi.AuthenticationFilters.Services
{
    public class FakeAuthService
    {
        public static bool IsValid(string username, string password)
        {
            bool isValid = false;
            if (User().TryGetValue(username, out string value))
            {
                isValid = value.Equals(password);
            }

            return isValid;
        }


        public static Dictionary<string, string> User()
        {
            var t = new Dictionary<string, string>();
            t.Add("rajen", "passworD-1234");
            t.Add("raj", "passworD-1234");
            t.Add("jen", "passworD-1234");

            return t;
        }
    }
}