using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface IBankCategoryRepository : IGenericRepository<bank_category>
    {
        PagedResults<bank_category> CreatePagedResults(int pageNumber, int pageSize);
        List<dropdown> GetAllDropDown();
    }
}