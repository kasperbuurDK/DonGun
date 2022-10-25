using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using ServerSideApiSsl.Database;

namespace ServerSideApiSsl
{
    public class BasicAuthenticationHandler<T> : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ISqlDbService<T> _userRepository;
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder,
            ISystemClock clock, ISqlDbService<T> userRepository) :
           base(options, logger, encoder, clock)
        {
            _userRepository = userRepository;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authHeader = Request.Headers["Authorization"].ToString();
            if (authHeader is not null && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                string token = authHeader["Basic ".Length..].Trim();
                string credentialsEncodedString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                string[] credentials = credentialsEncodedString.Split(':');
                if (_userRepository.Authenticate(credentials[0], credentials[1]))
                {
                    Claim[] claims = new[] { new Claim("name", credentials[0]), new Claim(ClaimTypes.Role, "user") };
                    ClaimsIdentity identity = new(claims, "Basic");
                    ClaimsPrincipal claimsPrincipal = new(identity);
                    return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
                }
                Response.StatusCode = 401;
                Response.Headers.Add("WWW-Authenticate", "Basic realm=\"DunGunCharaService.net\"");
                return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
            Response.StatusCode = 401;
            Response.Headers.Add("WWW-Authenticate", "Basic realm=\"DunGunCharaService.net\"");
            return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }
    }
}
