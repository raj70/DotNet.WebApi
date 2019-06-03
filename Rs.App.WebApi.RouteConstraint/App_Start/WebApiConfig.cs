using System.Web.Http;
using System.Web.Http.Routing;
using Rs.App.WebApi.RouteConstraint.Constraints;

namespace Rs.App.WebApi.RouteConstraint
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            //config.MapHttpAttributeRoutes();

            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("validAccount", typeof(AccountConstraint));
            config.MapHttpAttributeRoutes(constraintResolver);


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
