using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface IOrderServiceRepository : IGenericRepository<order_service>
    {
        PagedResults<order_service> CreatePagedResults(int pageNumber, int pageSize);
    }
}
