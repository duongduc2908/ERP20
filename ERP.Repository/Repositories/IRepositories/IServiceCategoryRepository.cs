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
    public interface IServiceCategoryRepository : IGenericRepository<service_category>
    {
        PagedResults<service_category> CreatePagedResults(int pageNumber, int pageSize);
        List<dropdown> GetAllName();
    }
}
