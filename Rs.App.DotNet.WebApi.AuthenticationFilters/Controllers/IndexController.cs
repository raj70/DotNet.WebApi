using Rs.App.DotNet.WebApi.AuthenticationFilters.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Rs.App.DotNet.WebApi.AuthenticationFilters.Controllers
{
    public class IndexController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Get()
        {
            return Ok("Hi Get all: Anonymous");
        }


        [HttpGet]
        [Route("Index/{id:int}")]
        [ApiKeyAuthenticationFilter]
        public IHttpActionResult Get(int id)
        {
            return Ok("Hi Api Key" + id);
        }
    }
}
