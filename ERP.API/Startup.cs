using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ERP.API.Controllers.Dashboard;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using SocketIOClient;

[assembly: OwinStartup(typeof(ERP.API.Startup))]

namespace ERP.API
{
    public partial class Startup:BaseController
    {
        public void Configuration(IAppBuilder app)
        {
            

            app.UseCors(CorsOptions.AllowAll);
            ConfigureAuth(app);
        }
    }
}
