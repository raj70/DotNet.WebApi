using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Rs.App.DotNet.WebApi.RouterFilters.ActionResults
{
    //https://gist.github.com/bradwilson/8586562

    public class AddHeaderResult<T> : IHttpActionResult where T: IHttpActionResult
    {
        public AddHeaderResult(T innerResult, string headerName, string headerValue)
        {
            InnerResult = innerResult;
            HeaderName = headerName;
            HeaderValue = headerValue;
        }

        public T InnerResult { get; private set; }
        public string HeaderName { get; private set; }
        public string HeaderValue { get; private set; }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response =await InnerResult.ExecuteAsync(cancellationToken);
            response.Headers.Add(HeaderName, HeaderValue);
            return response;
        }
    }

    public static class HttpActionResultHeaderExtension
    {
        public static AddHeaderResult<T> AddHeader<T>(this T actionResult, string headerName, string headerValue) where T: IHttpActionResult
        {
            return new AddHeaderResult<T>(actionResult, headerName, headerValue);
        }
    }
}