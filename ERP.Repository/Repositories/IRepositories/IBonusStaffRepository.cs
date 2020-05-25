using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Company;
using System;
using System.Collections.Generic;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface IBonusStaffRepository : IGenericRepository<bonus_staff>
    {
        PagedResults<bonus_staff> CreatePagedResults(int pageNumber, int pageSize);
        List<dropdown> GetAllDropDown();
    }
}
