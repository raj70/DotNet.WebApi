using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Rs.App.DotNet.WebApi.Delegates.Handlers
{
    //https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Forwarded

    public class ClientIPAddressHandler : DelegatingHandler
    {
        private static readonly string FORWARDED = "Forwarded";
        private static readonly string FORWARDED_FOR = "X-Forwarded-For";

        public static readonly string CLIENT_IP = "x-client-ip";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string ipAddress = string.Empty;

            if (request.Headers.Contains(FORWARDED))
            {
                // example : Forwarded: by=<identifier>;for=<identifier>;host=<host>;proto=<http|https>
                var forwardedValue = request.Headers.GetValues(FORWARDED).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
                ipAddress = GetIPFromForwarded(forwardedValue);
            }

            if(string.IsNullOrWhiteSpace(ipAddress) && request.Headers.Contains(FORWARDED_FOR))
            {
                // example : X-Forwarded-For: <client>, <proxy1>, <proxy2>
                var forwaredFor = request.Headers.GetValues(FORWARDED_FOR).FirstOrDefault(x=> !string.IsNullOrWhiteSpace(x));
                ipAddress = GetIPFromForwardedFor(forwaredFor);
            }

            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                request.Properties.Add(CLIENT_IP, ipAddress);
            }

            return base.SendAsync(request, cancellationToken);
        }

        private string GetIPFromForwarded(string forwardedValue)
        {
            // example : Forwarded: by=<identifier>;for=<identifier>;host=<host>;proto=<http|https>
            var ipAddress = string.Empty;
            if(forwardedValue != null)
            {
                var fwd = forwardedValue.Split(';').Select(x => x.Trim());
                if (fwd.Count() > 0)
                {
                    // by=<identifier>
                    var byPortion = fwd.FirstOrDefault(x => x.ToLowerInvariant().Contains("by"))
                                       .Split('=').Select(x => x.Trim())
                                       .ToList();
                    // by=<identifier>
                    if (byPortion.Count() > 0)
                    {
                        ipAddress = byPortion[1];
                    }
                }
            }

            return ipAddress;
        }

        private string GetIPFromForwardedFor(string value)
        {
            // example : X-Forwarded-For: <client>, <proxy1>, <proxy2>
            var ipAddress = string.Empty;
            if (value != null)
            {
                var fwd = value.Split(',').FirstOrDefault();
                ipAddress = fwd;
            }

            return ipAddress;
        }
    }

    public static class HttpRequestMessageIPExtension
    {
        public static string GetClientIpAddress(this HttpRequestMessage requestMessage)
        {
            var ipAddress = string.Empty;

            if(requestMessage != null)
            {
                if(requestMessage.Properties.TryGetValue(ClientIPAddressHandler.CLIENT_IP, out object ipAdd))
                {
                    ipAddress = ipAdd.ToString();
                }
            }            

            return ipAddress; 
        }
    }
}