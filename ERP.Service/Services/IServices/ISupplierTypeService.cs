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
    public interface ISupplierTypeService : IGenericService<supplier_type>
    {
        PagedResults<supplier_type> GetAllPage(int pageNumber, int pageSize);
        PagedResults<supplier_type> GetAllPageById(int pageNumber, int pageSize, int id);
        PagedResults<supplier_type> GetSuplierType(string search_name);
    }
}
