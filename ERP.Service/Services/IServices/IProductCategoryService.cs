using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service.Services.IServices
{
    public interface IProductCategoryService : IGenericService<product_category>
    {
        PagedResults<product_category> GetAllPage(int pageNumber, int pageSize);
        PagedResults<product_category> GetAllPageById(int pageNumber, int pageSize, int id);
        PagedResults<product_category> GetProductCategorys(string search_name);
        PagedResults<string> GetAllName();
    }
}
