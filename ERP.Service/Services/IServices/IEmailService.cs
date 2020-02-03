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
    public interface IEmailService : IGenericService<email>
    {
        PagedResults<email> CreatePagedResults(int pageNumber, int pageSize);
    }
}
