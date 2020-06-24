using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using ERP.Data.DbContext;
using ERP.Common.Constants;
using ERP.Data.ModelsERP;
using ERP.Repository.Repositories;
using AutoMapper;
using Newtonsoft.Json;

namespace ERP.API.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private ERPDbContext _dbContext { get; set; }
        private CompanyRepository companyres;
        //Store the base address of the web api
        //You need to change the PORT number where your WEB API service is running
        public ApplicationOAuthProvider()
        {
            _dbContext = ERPDbContext.Create();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            _dbContext = new ERPDbContext();
            ClientMaster client = context.OwinContext.Get<ClientMaster>("ta:client");
            var hash_pass = HashMd5.convertMD5(context.Password.Trim());
            var url_thumbnai = "";
            staff staff_user = new staff();
            group_role gr_role = new group_role();
            staff_user = _dbContext.staffs.FirstOrDefault(t => t.sta_username.Contains(context.UserName.Trim()) && t.sta_password.Contains(hash_pass));
            gr_role = _dbContext.group_role.FirstOrDefault(t => t.gr_id==staff_user.group_role_id);
            companyres = new CompanyRepository(_dbContext);
            var company = companyres.GetById(Convert.ToInt32(staff_user.company_id),true);
            if (staff_user == null)
            {
                context.SetError("invalid_grant", "Provided staff_username and password is incorrect");
                return;
            }
            else if(staff_user.sta_status == 0)
            {
                context.SetError("Error", "Tài khoản đã bị khóa.");
                return;
            }
            if(staff_user.sta_thumbnai != null)
            {
                url_thumbnai = staff_user.sta_thumbnai;
            }
            var role="";
            if (gr_role.gr_name.Contains("admin"))
            {
                role = Roles.ADMIN;
            }
            else role = Roles.USER;
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
            identity.AddClaim(new Claim(ClaimTypes.Name, staff_user.sta_fullname));
            if (staff_user.sta_email != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Email, staff_user.sta_email));
            }
            identity.AddClaim(new Claim("Id", staff_user.sta_id.ToString()));
            if (staff_user.company_id != null)
                identity.AddClaim(new Claim("CompanyId", staff_user.company_id.ToString()));
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                    },
                    {
                        "staff_Name",staff_user.sta_fullname
                    },
                    {
                        "url_thumbnai", url_thumbnai
                    },
                    {
                        "sta_login",  staff_user.sta_login.ToString()
                    },
                    {
                        "sta_id",staff_user.sta_id.ToString()
                    },
                    {
                        "Role", role
                    },
                    {
                        "company_id", staff_user.company_id.ToString()
                    },
                    {
                        "list_package_function", JsonConvert.SerializeObject(company.list_function).ToString()
        }
                }) ;
            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            // The TryGetBasicCredentials method checks the Authorization header and
            // Return the ClientId and clientSecret
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.SetError("invalid_client", "Client credentials could not be retrieved through the Authorization header.");
                return Task.FromResult<object>(null);
            }
            //Check the existence of by calling the ValidateClient method
            ClientMaster client = (new ClientMasterRepository()).ValidateClient(clientId, clientSecret);
            if (client == null)
            {
                // Client could not be validated.
                context.SetError("invalid_client", "Client credentials are invalid.");
                return Task.FromResult<object>(null);
            }
            else
            {
                if (!client.Active)
                {
                    context.SetError("invalid_client", "Client is inactive.");
                    return Task.FromResult<object>(null);
                }
                // Client has been verified.
                context.OwinContext.Set<ClientMaster>("ta:client", client);
                context.OwinContext.Set<string>("ta:clientAllowedOrigin", client.AllowedOrigin);
                context.OwinContext.Set<string>("ta:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());
                context.Validated();
                return Task.FromResult<object>(null);
            }
        }
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["client_id"];
            var currentClient = context.ClientId;
            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }
            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));
            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);
            return Task.FromResult<object>(null);
        }
    }
}