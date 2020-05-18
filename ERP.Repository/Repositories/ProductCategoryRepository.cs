using ERP.Common.GenericRepository;
using ERP.Common.Models;
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
    public class ProductCategoryRepository : GenericRepository<product_category>, IProductCategoryRepository
    {
        public ProductCategoryRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public List<dropdown> GetAllName(int company_id)
        {
            List<dropdown> lst_res = new List<dropdown>();
            var list = _dbContext.product_category.Where(x => x.company_id == company_id).ToList();
            foreach (var p in list) {
                dropdown dr = new dropdown();
                dr.id = p.pc_id;
                dr.name = p.pc_name;
                lst_res.Add(dr);
            }
            return lst_res;
        }
    }
}