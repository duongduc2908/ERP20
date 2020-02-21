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
    public interface ISupplierService : IGenericService<supplier>
    {
        PagedResults<supplier> GetAllPage(int pageNumber, int pageSize);
        PagedResults<supplier> GetAllPageById(int pageNumber, int pageSize, int id);
        PagedResults<supplier> GetSupliers(string search_name);
        PagedResults<string> GetAllName();
    }
}
