using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace Rs.App.DotNet.WebApi.RouterFilters.Filters
{
    public enum ClientCacheControl
    {
        Public,     // can be cached by intermediate devices even if authentication was used;
        Private,    // browser-only, no intermediate caching, typically for per-user data
        NoCache     // no caching by browser or intermediate devices
    };
    //https://developers.google.com/web/fundamentals/performance/optimizing-content-efficiency/http-caching
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ClientCacheControlFilterAttribute : ActionFilterAttribute
    {
        public ClientCacheControl CacheType;
        public double CacheSeconds { get; }
        public ClientCacheControlFilterAttribute(double seconds = 60.0) : this(ClientCacheControl.Private, seconds)
        {
            CacheSeconds = seconds;
        }

        public ClientCacheControlFilterAttribute(ClientCacheControl cacheControl, double seconds = 60.0)
        {
            CacheType = cacheControl;
            CacheSeconds = seconds;
        }

        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            await base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);

            if(actionExecutedContext.Response == null)
            {
                return;
            }

            if(CacheType == ClientCacheControl.NoCache)
            {
                actionExecutedContext.Response.Headers.CacheControl = new CacheControlHeaderValue
                {
                    NoStore = true
                };

                actionExecutedContext.Response.Headers.Pragma.ParseAdd("no-cache");

                if (!actionExecutedContext.Response.Headers.Date.HasValue)
                {
                    actionExecutedContext.ActionContext.Response.Headers.Date = DateTime.UtcNow;
                }


                if (actionExecutedContext.Response.Content !=null)
                {
                    actionExecutedContext.Response.Content.Headers.Expires = actionExecutedContext.Response.Headers.Date;
                }
            }
            else
            {
                actionExecutedContext.Response.Headers.CacheControl = new CacheControlHeaderValue() {
                    Private = (CacheType == ClientCacheControl.Private),
                    NoCache = false,
                    MaxAge = TimeSpan.FromSeconds(CacheSeconds)
                };

                if (!actionExecutedContext.Response.Headers.Date.HasValue)
                {
                    actionExecutedContext.Response.Headers.Date = DateTimeOffset.UtcNow;
                }

                if (actionExecutedContext.Response.Content != null)
                    actionExecutedContext.Response.Content.Headers.Expires =
                        actionExecutedContext.Response.Headers.Date.Value.AddSeconds(CacheSeconds);
            }
        }

    }
}