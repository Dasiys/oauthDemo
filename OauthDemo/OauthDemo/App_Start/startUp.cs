using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace OauthDemo
{
    public partial  class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            var OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new CNBlogsAuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true                
            };
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }

    public class CNBlogsAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = null;
            string clientSerect = null;
            context.TryGetBasicCredentials(out clientId, out clientSerect);
            if (clientId == "1234" && clientSerect == "5678")
            {
                context.Validated(clientId);
            }
            return base.ValidateClientAuthentication(context);
        }

        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var OAuthIdentity=new ClaimsIdentity(context.Options.AuthenticationType);
            OAuthIdentity.AddClaim(new Claim(ClaimTypes.Name,"Yeweimi"));
            var ticket=new AuthenticationTicket(OAuthIdentity,new AuthenticationProperties());
            context.Validated(ticket);
            return base.GrantClientCredentials(context);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            if (context.UserName != "yeweimi" || context.Password != "123456")
            {
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            }
            else
            {
                var oauthIdentity=new ClaimsIdentity(context.Options.AuthenticationType);
                oauthIdentity.AddClaim(new Claim(ClaimTypes.Name,context.UserName));
                var ticket=new AuthenticationTicket(oauthIdentity,new AuthenticationProperties());
                context.Validated(ticket);
            }
            return base.GrantResourceOwnerCredentials(context);
        }
    }

    /// <summary>
    /// refreshToken
    /// </summary>
    public class CNBlogsRefreshTokenProvider : AuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string,string> _refreshTokens=new ConcurrentDictionary<string, string>();
        public override void Create(AuthenticationTokenCreateContext context)
        {
            string tokenValue = Guid.NewGuid().ToString("N");
            context.Ticket.Properties.IssuedUtc=DateTime.UtcNow;
            context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddDays(60);
            _refreshTokens[tokenValue] = context.SerializeTicket();

            context.SetToken(tokenValue);
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            string value;
            if (_refreshTokens.TryRemove(context.Token, out value))
            {
                
            }
            base.Receive(context);
        }
    }
}