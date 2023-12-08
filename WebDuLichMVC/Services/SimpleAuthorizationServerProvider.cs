using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using WebDuLichMVC.Controllers.Api;
namespace WebDuLichMVC.Services
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context) {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context) {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new []{"*"});
            using (AuthRepository _repo = new AuthRepository())
            {
                IdentityUser user = await _repo.FindUser(context.UserName, context.Password);
                if(user == null) {
                    context.SetError("invaild_grant","The user name or password is incorrect.");
                }
            }
        }
        
    }
}