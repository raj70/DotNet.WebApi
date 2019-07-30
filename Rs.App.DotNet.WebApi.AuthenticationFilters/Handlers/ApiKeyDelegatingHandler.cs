using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Rs.App.DotNet.WebApi.AuthenticationFilters.Handlers
{
    public class ApiKeyDelegatingHandler : DelegatingHandler
    {
        public static readonly string API_KEY = "X-API-Key";
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var key = string.Empty;
            var currentHeaders = request.Headers;

            if (currentHeaders.Contains(API_KEY))
            {
                key = request.Headers.GetValues(API_KEY).ToString();
            }
            else if (currentHeaders.Authorization != null
                && currentHeaders.Authorization.Scheme.ToLowerInvariant() == "ApiKey".ToLowerInvariant())
            {
                key = currentHeaders.Authorization.Parameter;
            }
            else
            {
                // from previous exericse 
                var queryString = request.GetQueryNameValuePairs();
                var value = queryString.Where(x => x.Key.ToLower() == API_KEY.ToLower()).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(value.Value))
                {
                    key = value.Value;
                }
            }

            request.Properties.Add(API_KEY, key);

            return base.SendAsync(request, cancellationToken);
        }
    }


    public static class RequestMessageApiKeyExtension
    {
        public static string GetApikey(this HttpRequestMessage request)
        {
            string key = string.Empty;
            if (request == null)
            {
                key = string.Empty;
            }

            if (request.Properties.TryGetValue(ApiKeyDelegatingHandler.API_KEY, out object keyvalue))
            {
                key = keyvalue.ToString();
            }

            return key;
        }
    }
}