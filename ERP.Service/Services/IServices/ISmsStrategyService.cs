using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView.Sms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service.Services.IServices
{
    public interface ISmsStrategyService : IGenericService<sms_strategy>
    {
        PagedResults<smsstrategyviewmodel> GetAllPageSearch(int pageNumber,int pageSize,string search_name);
    }
}
