using ERP.Common.GenericRepository;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class CustomerPhoneRepository : GenericRepository<customer_phone>, ICustomerPhoneRepository
    {
        public CustomerPhoneRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }

    }
}