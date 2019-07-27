using System.Web;
using System.Web.Mvc;

namespace Rs.App.DotNet.WebApi.DelegateHandler
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
