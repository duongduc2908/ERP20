using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class ClientMasterRepository : IDisposable
    {
        // SECURITY_DBEntities it is your context class
        ERPDbContext context = new ERPDbContext();
        //This method is used to check and validate the Client credentials
        public ClientMaster ValidateClient(string ClientID, string ClientSecret)
        {
            return context.ClientMasters.FirstOrDefault(user =>
             user.ClientID == ClientID
            && user.ClientSecret == ClientSecret);
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}