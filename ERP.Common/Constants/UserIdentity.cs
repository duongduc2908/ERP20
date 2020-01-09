using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace ERP.Common.Constants
{
    public class UserIdentity: ApiController
    {
        public static string Name { get; set; }
        public static string Email { get; set; }
        public static string Roles { get; set; }
       
        public static string get_properti()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var name = identity.Claims.ToList().FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            var role = identity.Claims.ToList().FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            return "";
        }
            
    }
}