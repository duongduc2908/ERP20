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
    public interface ITrainingRepository : IGenericRepository<training>
    {
        PagedResults<training> CreatePagedResults(int pageNumber, int pageSize);
        PagedResults<training> GetAllSearch(int pageNumber, int pageSize, string search_name);
        training GetById(int tn_id);
        List<dropdown> GetAllName();
    }
}
