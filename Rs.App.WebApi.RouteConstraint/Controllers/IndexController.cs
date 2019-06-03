using System;
using System.Web.Http;

namespace Rs.App.WebApi.RouteConstraint.Controllers
{
    [Route("api/Index")]
    public class IndexController : ApiController
    {
        public IndexController()
        {
        }

        [HttpGet, Route("{accountId:validAccount}")]
        public string Get(string accountId)
        {
            return "Hi " + accountId;
        }


    }
}
