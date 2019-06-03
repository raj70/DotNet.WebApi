using System.Web;
using System.Web.Http;

namespace Rs.App.WebApi.RouteConstraint
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
