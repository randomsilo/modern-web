
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LoyalGuard.Api
{
    public class BrashApiAuthModel
    {
        public int ApiAuthId { get; set; }
        public string ApiAuthName { get; set; }
        public string ApiAuthPass { get; set; }
    }

    public interface IBrashApiAuthService
    {
        Task<BrashApiAuthModel> Authenticate(string apiAuthName, string apiAuthPass);
    }

    public class BrashApiAuthService : IBrashApiAuthService
    {
        private List<BrashApiAuthModel> _accounts = new List<BrashApiAuthModel>();

        public BrashApiAuthService AddAuthAccount(BrashApiAuthModel account)
        {
            _accounts.Add(account);
            return this;
        }

        public async Task<BrashApiAuthModel> Authenticate(string apiAuthName, string apiAuthPass)
        {
            var auth = await Task.Run(() => _accounts.SingleOrDefault(x => x.ApiAuthName == apiAuthName && x.ApiAuthPass == apiAuthPass));

            if (auth == null)
                return null;

            return auth;
        }
    }

    public class BrashBasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IBrashApiAuthService _apiAuthService;

        public BrashBasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IBrashApiAuthService apiAuthService)
            : base(options, logger, encoder, clock)
        {
            _apiAuthService = apiAuthService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            BrashApiAuthModel user = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var apiAuthName = credentials[0];
                var apiAuthPass = credentials[1];

                user = await _apiAuthService.Authenticate(apiAuthName, apiAuthPass);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (user == null)
                return AuthenticateResult.Fail("Invalid AuthName or Password");

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.ApiAuthId.ToString()),
                new Claim(ClaimTypes.Name, user.ApiAuthName),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
