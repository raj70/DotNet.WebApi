using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Rs.App.DotNet.WebApi.Delegates.Handlers
{
    public class ApiKeyHandler : DelegatingHandler
    {
        public static readonly string API_KEY = "api_key";
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var key = string.Empty;

            if (request.Headers.Contains(API_KEY))
            {
                key = request.Headers.GetValues(API_KEY).ToString();
            }
            else
            {
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


    public static class RequestMessageExtension
    {
        public static string GetApikey(this HttpRequestMessage request)
        {
            string key = string.Empty;
            if (request == null)
            {
                key = string.Empty;
            }

            if (request.Properties.TryGetValue(ApiKeyHandler.API_KEY, out object keyvalue))
            {
                key = keyvalue.ToString();
            }

            return key;
        }
    }
}