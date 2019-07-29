using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Rs.App.DotNet.WebApi.RouterFilters.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ModelStateValidationAttribute : ActionFilterAttribute
    {
        public bool BodyRequired { get; private set; }
        public ModelStateValidationAttribute(bool bodyRequired)
        {
            BodyRequired = bodyRequired;
        }

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, actionContext.ModelState);
            }
            else if (BodyRequired)
            {
                foreach (var b in actionContext.ActionDescriptor.ActionBinding.ParameterBindings)
                {
                    if (b.WillReadBody)
                    {
                        if (!actionContext.ActionArguments.ContainsKey(b.Descriptor.ParameterName)
                            || actionContext.ActionArguments[b.Descriptor.ParameterName] == null)
                        {
                            actionContext.Response = actionContext.Request.CreateErrorResponse(
                                                HttpStatusCode.BadRequest, b.Descriptor.ParameterName + " is required.");
                        }
                        // since only one FromBody can exist, we can abort the loop after a body param is found
                        break;
                    }
                }
            }
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}