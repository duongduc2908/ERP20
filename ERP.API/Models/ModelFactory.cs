﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;

namespace ManagementLibrary.API.Models
{
    public class ModelFactory
{
        private UrlHelper _UrlHelper;
    //Code removed for brevity

    public RoleReturnModel Create(IdentityRole appRole) {

		return new RoleReturnModel
	   {
		   Url = _UrlHelper.Link("GetRoleById", new { id = appRole.Id }),
		   Id = appRole.Id,
		   Name = appRole.Name
	   };
	}
}

public class RoleReturnModel
{
	public string Url { get; set; }
	public string Id { get; set; }
	public string Name { get; set; }
}
}