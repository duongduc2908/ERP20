using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace ERP.Data.Identity
{
    public class AppIdentityClaims: ClaimsIdentity
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserRole { get; set; }
        public AppIdentityClaims(ClaimsIdentity claimsIdentity): base(claimsIdentity) {
            this.UserName = claimsIdentity.FindFirst(ClaimTypes.Name).Value;
            this.UserEmail = claimsIdentity.FindFirst(ClaimTypes.Email).Value;
            this.UserRole = claimsIdentity.FindFirst(ClaimTypes.Role).Value;
        }
        public AppIdentityClaims(string authenticationType): base(authenticationType)
        {
        }

    }
}