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
    public interface ISupplierRepository : IGenericRepository<supplier>
    {
        PagedResults<supplier> GetAllPage(int pageNumber, int pageSize);
        PagedResults<supplier> GetAllPageById(int pageNumber, int pageSize, int id);
        PagedResults<supplier> GetSupliers(string search_name);
        List<dropdown> GetAllName();
    }
}
