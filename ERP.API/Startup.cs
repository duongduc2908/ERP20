using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            //IPAddress address = IPAddress.Parse("127.0.0.1");
            //try
            //{
            //    // 1. listen
            //    TcpListener listener = new TcpListener(address, PORT_NUMBER);
            //    listener.Start();
            //    socket = listener.AcceptSocket();

            //    // 2. receive
            //    byte[] data = new byte[BUFFER_SIZE];
            //    socket.Receive(data);

            //    string str = encoding.GetString(data);

            //    // 3. send
            //    socket.Send(encoding.GetBytes("Hello " + str));

            //    // 4. close
            //    socket.Close();
            //    listener.Stop();
            //}
            //catch(SocketException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            
            app.UseCors(CorsOptions.AllowAll);
            ConfigureAuth(app);
        }
    }
}
