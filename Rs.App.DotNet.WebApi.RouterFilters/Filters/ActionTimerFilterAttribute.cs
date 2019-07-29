using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Rs.App.DotNet.WebApi.RouterFilters.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ActionTimerFilterAttribute: ActionFilterAttribute
    {
        public const string Header = "X-API-Timer";
        public const string TimerPropertyName = "RouterTimerFilter_";
        public string TimeName { get; }

        public ActionTimerFilterAttribute(string name = null)
        {
            TimeName = name;
        }
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var name = TimeName;
            if (string.IsNullOrEmpty(name))
            {
                name = actionContext.ActionDescriptor.ActionName;
            }

            actionContext.Request.Properties.Add(TimerPropertyName + name, Stopwatch.StartNew());

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var name = TimeName;
            if (string.IsNullOrEmpty(name))
            {
                name = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            }

            var time = 0L;
            if(actionExecutedContext.Request.Properties.TryGetValue(TimerPropertyName + name, out object timer))
            {
                var timerWatch = timer as Stopwatch;
                time = timerWatch.ElapsedMilliseconds;

                Trace.WriteLine(actionExecutedContext.Request.Method + " " + actionExecutedContext.ActionContext.ActionDescriptor + " " + time);                
            }
            await base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);

            actionExecutedContext.Response.Headers.Add(Header, time.ToString());
        }
    }
}