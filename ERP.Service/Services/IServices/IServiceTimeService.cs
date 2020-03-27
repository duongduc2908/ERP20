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
    public interface IServiceTimeService : IGenericService<service_time>
    {
        PagedResults<service_time> CreatePagedResults(int pageNumber, int pageSize);
    }
}
