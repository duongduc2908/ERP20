using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using ERP.API.Models;
using ERP.Controller.Models;
using ERP.Data.DbContext;
using System.Text;
using ERP.Service.Services.IServices;
using ERP.Common.Constants;

namespace ERP.API.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly ERPDbContext _dbContext = new ERPDbContext();
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var user = _dbContext.staffs.FirstOrDefault(t => t.sta_username.Contains(context.UserName) && t.sta_password.Contains(context.Password));
            if (user == null)
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
                return;
            }
            var role="";
            if (user.group_role_id == 1)
            {
                role = Roles.ADMIN;
            }
            else role = Roles.USER;
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.sta_fullname));
            identity.AddClaim(new Claim("Email", user.sta_email));
            context.Validated(identity);
        }
    }
}