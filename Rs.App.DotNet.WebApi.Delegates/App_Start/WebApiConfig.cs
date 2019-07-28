using Rs.App.DotNet.WebApi.Delegates.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Rs.App.DotNet.WebApi.Delegates
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.MessageHandlers.Add(new FullTimeHandler());
            config.MessageHandlers.Add(new ApiKeyHandler());
            config.MessageHandlers.Add(new RemoveBadHeaderHandler());
            config.MessageHandlers.Add(new HttpMethodOverrideHandler());
            config.MessageHandlers.Add(new ClientIPAddressHandler());

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
