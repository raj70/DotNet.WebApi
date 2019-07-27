using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Rs.App.DotNet.WebApi.DelegateHandler.Handlers
{
    public class FullTimeHandler: DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var timeSpan = Stopwatch.StartNew();
           
            var response = await base.SendAsync(request, cancellationToken);

            var timeTook =  timeSpan.ElapsedMilliseconds;
            response.Headers.Add("X-API-Time", timeTook.ToString());
                        
            return response;
        }
    }
}