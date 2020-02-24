using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service.Services.IServices
{
    public interface IProductService : IGenericService<product>
    {
        PagedResults<productviewmodel> GetAllPage(int pageNumber, int pageSize);
        PagedResults<productviewmodel> GetAllPageById( int id);
        
       
        PagedResults<productviewmodel> GetProducts(int pageNumber, int pageSize,string search_name, int? category_id);
        List<dropdown> GetUnit();
    }
}
