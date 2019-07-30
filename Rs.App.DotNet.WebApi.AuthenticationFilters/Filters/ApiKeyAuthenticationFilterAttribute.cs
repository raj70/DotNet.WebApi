using Rs.App.DotNet.WebApi.AuthenticationFilters.Handlers;
using Rs.App.DotNet.WebApi.AuthenticationFilters.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace Rs.App.DotNet.WebApi.AuthenticationFilters.Filters
{
    public class ApiKeyAuthenticationFilterAttribute : Attribute, IAuthenticationFilter
    {
        private string SupportedKeyScheme = "ApiKey";
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var currentRequest = context.Request;

            var key = currentRequest.GetApikey();

            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            // if the schema is other type, then stop here
            if (!SupportedKeyScheme.Equals(currentRequest.Headers.Authorization.Scheme))
            {
                return;
            }

            IPrincipal principal = await ValidateKeyAndCreatePrincipalAsync(key);
            if (principal != null)
            {
                context.Principal = principal;
            }
            else
            {
                context.ErrorResult = new AuthenticationFailureResult("Not a valid Api Key", context.Request);
            }

        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        private async Task<IPrincipal> ValidateKeyAndCreatePrincipalAsync(string key)
        {
            if (key.Length != 8)
            {
                return null;
            }

            var customerAccountId = key.Substring(0, 3);
            var claimCollection = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, key),
                    new Claim(ClaimTypes.AuthenticationInstant, DateTime.UtcNow.ToString("o")),
                    new Claim("urn:ClientAccount", customerAccountId)
                };

            var identity = new ClaimsIdentity(claimCollection, SupportedKeyScheme);
            var principal = new ClaimsPrincipal(identity);

            return await Task.FromResult(principal);
        }
    }
}