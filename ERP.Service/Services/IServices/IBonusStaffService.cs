using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System.Collections.Generic;

namespace ERP.Service.Services.IServices
{
    public interface IBonusStaffService : IGenericService<bonus_staff>
    {
        PagedResults<bonus_staff> CreatePagedResults(int pageNumber, int pageSize);
        List<dropdown> GetAllDropDown();
    }
}
