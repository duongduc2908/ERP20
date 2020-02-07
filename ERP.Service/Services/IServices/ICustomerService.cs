using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services.IServices
{
    public interface ICustomerService : IGenericService<customer>
    {
        PagedResults<customer> CreatePagedResults(int pageNumber, int pageSize);
        PagedResults<customer> GetInfor(string search_name);
        
    }
}