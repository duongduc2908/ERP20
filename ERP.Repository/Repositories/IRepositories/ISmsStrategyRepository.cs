using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView.Sms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface ISmsStrategyRepository : IGenericRepository<sms_strategy>
    {
        PagedResults<smsstrategyviewmodel> GetAllPageSearch(int pageNumber, int pageSize, string search_name);
    }
}
