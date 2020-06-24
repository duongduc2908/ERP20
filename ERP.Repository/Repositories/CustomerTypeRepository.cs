using ERP.Common.GenericRepository;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class CustomerTypeRepository : GenericRepository<customer_type>, ICustomerTypeRepository
    {
        public CustomerTypeRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public List<dropdown> GetAllDropdown(int company_id)
        {
            List<dropdown> res = new List<dropdown>();
            List<customer_type> lts_s = _dbContext.customer_type.Where(x => x.company_id == company_id).ToList();
            foreach (customer_type so in lts_s)
            {
                dropdown dr = new dropdown();
                dr.id = so.cut_id;
                dr.name = so.cut_name;
                res.Add(dr);
            }
            return res;
        }
    }
}
