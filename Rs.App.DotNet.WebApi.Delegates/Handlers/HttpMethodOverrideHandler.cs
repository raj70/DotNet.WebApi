using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Rs.App.DotNet.WebApi.Delegates.Handlers
{
    public class HttpMethodOverrideHandler: DelegatingHandler
    {
        private string[] httpMethods = { "PUT", "PATCH", "DELETE", "HEAD", "VIEW" };

        private readonly static string XHeader = "X-HTTP-Method-Override";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            
            if(request.Method == HttpMethod.Post && request.Headers.Contains(XHeader))
            {
                var xheaderMethod = request.Headers.GetValues(XHeader).FirstOrDefault();
                Trace.WriteLine(xheaderMethod);
                if (xheaderMethod != null)
                {
                    if(httpMethods.Contains(xheaderMethod, StringComparer.InvariantCultureIgnoreCase))
                    {
                        request.Method = new HttpMethod(xheaderMethod);
                    }
                    else
                    {
                        
                        var errorResponse = new HttpResponseMessage(System.Net.HttpStatusCode.MethodNotAllowed);
                        return Task.FromResult(errorResponse);
                    }
                }
               
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}