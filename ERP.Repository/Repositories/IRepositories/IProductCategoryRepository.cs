using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface IProductCategoryRepository : IGenericRepository<product_category>
    {
        PagedResults<product_category> GetAllPage(int pageNumber, int pageSize);
        PagedResults<product_category> GetAllPageById(int pageNumber, int pageSize, int id);
        PagedResults<product_category> GetProductCategorys(string search_name);
        PagedResults<dropdown> GetAllName();
    }
}
