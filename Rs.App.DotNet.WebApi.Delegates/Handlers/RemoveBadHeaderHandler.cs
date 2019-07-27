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
    public class RemoveBadHeaderHandler: DelegatingHandler
    {
        readonly string[] headerToRemove = { "server", "x-powered-by", "x-aspnet-version" };
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var response = await base.SendAsync(request, cancellationToken);

            headerToRemove.ToList().ForEach(x => {
                Trace.WriteLine(x);
                response.Headers.Remove(x);
            });

            return response;
        }
    }
}