using Rs.App.DotNet.WebApi.RouterFilters.ActionResults;
using Rs.App.DotNet.WebApi.RouterFilters.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rs.App.DotNet.WebApi.RouterFilters.Controllers
{
    public class IndexController : ApiController
    {
        [HttpGet]
        [Route("Index")]
        [ActionTimerFilter("Index_Get")]
        [ClientCacheControlFilter(ClientCacheControl.Private, 10)]
        public string Get()
        {
            Trace.WriteLine("Get Index is called " + DateTime.Now);
            return "Hi ";
        }


        [Route("Index/{index:int}")]
        public IHttpActionResult Get(int index)
        {
            var response = Ok("Hi Get by id").AddHeader("x-developer", "rajen shrestha");

            return response;
        }
    }
}
