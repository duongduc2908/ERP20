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
    public interface IDepartmentRepository : IGenericRepository<department>
    {
        PagedResults<department> CreatePagedResults(int pageNumber, int pageSize);
        List<dropdown> Get_Level_One();
        List<dropdown> Get_Children(int id);
    }
}
