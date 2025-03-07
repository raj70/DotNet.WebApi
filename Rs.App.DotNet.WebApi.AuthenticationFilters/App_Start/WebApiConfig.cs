﻿using Rs.App.DotNet.WebApi.AuthenticationFilters.Filters;
using Rs.App.DotNet.WebApi.AuthenticationFilters.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Rs.App.DotNet.WebApi.AuthenticationFilters
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.SuppressHostPrincipal();

            config.MessageHandlers.Add(new ApiKeyDelegatingHandler());


            config.Filters.Add(new BasicAuthenticationFilterAttribute());
            config.Filters.Add(new AuthorizeAttribute());

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
