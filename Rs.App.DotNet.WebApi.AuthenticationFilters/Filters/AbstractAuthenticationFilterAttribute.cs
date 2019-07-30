using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace Rs.App.DotNet.WebApi.AuthenticationFilters.Filters
{
    // https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-filters
    public abstract class AbstractAuthenticationFilterAttribute : Attribute, IAuthenticationFilter
    {
        protected string SupportedTokenScheme;

        public bool AllowMultiple => false;

        public bool SendChallenge { get; set; }

        public AbstractAuthenticationFilterAttribute(bool sendChallenge = false)
        {
        }

        // https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-filters#implementing-the-authenticateasync-method
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // STEP 1: extract your credentials from the request.  Generally this should be the 
            //         Authorization header, which the rest of this template assumes, but
            //         could come from any part of the request headers.
            var authHeader = context.Request.Headers.Authorization;
            // if there are no credentials, abort out
            if (authHeader == null)
                return;

            // STEP 2: if the token scheme isn't understood by this authenticator, abort out
            var tokenType = authHeader.Scheme;
            if (!tokenType.Equals(SupportedTokenScheme))
                return;

            // STEP 3: Given a valid token scheme, verify credentials are present
            var credentials = authHeader.Parameter;
            if (String.IsNullOrEmpty(credentials))
            {
                // no credentials sent with the scheme, abort out of the pipeline with an error result
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", context.Request);
                return;
            }

            // STEP 4: validate the credentials.  Return an error if invalid, else set the IPrincipal 
            //         on the context.
            IPrincipal principal = await AuthenticateAsync(credentials, cancellationToken);
            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", context.Request);
            }
            else
            {
                // We have a valid, authenticated user; save off the IPrincipal instance
                context.Principal = principal;
            }
        }

        protected abstract Task<IPrincipal> AuthenticateAsync(string credentials,
            CancellationToken cancellationToken);


        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            if (SendChallenge)
            {
                var challenge = new AuthenticationHeaderValue(SupportedTokenScheme);
                context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            }

            return Task.FromResult(0);
        }

       
    }
}