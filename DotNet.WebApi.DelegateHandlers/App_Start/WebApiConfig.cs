using Rs.App.DotNet.WebApi.DelegateHandler.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Rs.App.DotNet.WebApi.DelegateHandler
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.MessageHandlers.Add(new FullTimeHandler());
            config.MessageHandlers.Add(new ApiKeyHandler());
            config.MessageHandlers.Add(new RemoveBadHeaderHandler());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
