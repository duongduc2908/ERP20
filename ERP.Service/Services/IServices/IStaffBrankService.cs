using System;
using System.Collections.Generic;
using System.Linq;
using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;

namespace ERP.Service.Services.IServices
{
    public interface IStaffBrankService : IGenericService<staff_brank>
    {
        PagedResults<staff_brank> CreatePagedResults(int pageNumber, int pageSize);
    }
}
