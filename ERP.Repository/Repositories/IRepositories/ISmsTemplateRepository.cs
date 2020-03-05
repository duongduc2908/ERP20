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
    public interface ISmsTemplateRepository : IGenericRepository<sms_template>
    {
        PagedResults<smstemplatemodelview> CreatePagedResults(int pageNumber, int pageSize, string search_name);
        PagedResults<smstemplatestrategyviewmodel> CreatePagedSmsTrategy(int pageNumber, int pageSize);
        PagedResults<field> Get_All_Fields(int pageNumber, int pageSize);
    }
}
