using System;
using System.Collections.Generic;
using System.Linq;
using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;

namespace ERP.Service.Services.IServices
{
    public interface IBankService : IGenericService<bank>
    {
        PagedResults<bank> CreatePagedResults(int pageNumber, int pageSize);
        List<dropdown> GetAllDropDown(int? id);
    }
}
