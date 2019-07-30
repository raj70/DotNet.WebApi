using Rs.App.DotNet.WebApi.AuthenticationFilters.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Rs.App.DotNet.WebApi.AuthenticationFilters.Filters
{
    public class BasicAuthenticationFilterAttribute : AbstractAuthenticationFilterAttribute
    {
        public BasicAuthenticationFilterAttribute() : base(sendChallenge: true)
        {
            SupportedTokenScheme = "Basic";
        }

        protected override async Task<IPrincipal> AuthenticateAsync(string credentials, CancellationToken cancellationToken)
        {
            Tuple<string, string> userNameAndPasword = ExtractUserNameAndPassword(credentials);

            if (userNameAndPasword.Item1 == null || userNameAndPasword.Item2 == null)
            {
                return null;
            }

            string userName = userNameAndPasword.Item1;
            string password = userNameAndPasword.Item2;

            // 
            if (!FakeAuthService.IsValid(userName, password))
            {
                return null;
            }

            var claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.AuthenticationInstant, DateTime.UtcNow.ToString("o")),
                new Claim("urn:MyCustomClaim", "Hi basic")
            };

            //SupportedTokenScheme is used show at the backend, which authorization is used (if we have more than one auth filters)
            var identity = new ClaimsIdentity(claimCollection, SupportedTokenScheme);
            var principal = new ClaimsPrincipal(identity);

            return await Task.FromResult(principal);
        }

        public static Tuple<string, string> ExtractUserNameAndPassword(string credential)
        {
            string password = null;
            var subject = (Encoding.GetEncoding("iso-8859-1").GetString(Convert.FromBase64String(credential)));
            if (String.IsNullOrEmpty(subject))
            {
                return new Tuple<string, string>(null, null);
            }

            if (subject.Contains(":"))
            {
                var index = subject.IndexOf(':');
                password = subject.Substring(index + 1);
                subject = subject.Substring(0, index);
            }

            return new Tuple<string, string>(subject, password);
        }
    }
}
