using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Rs.App.DotNet.WebApi.AuthenticationFilters.Services
{
    public static class ApiKeyValidator
    {
        public static Task<bool> IsValid(string key, out string clientAccount)
        {
            bool isValid = false;
            clientAccount = "";

            if (key.Length == 8)
            {
                var customerAccountId = key.Substring(0, 3);
                var accountId = key.Remove(0, 3);
                clientAccount = customerAccountId;

                isValid = true;
            }

            return Task.FromResult(isValid);
        }
    }
}