using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface IBankBranchRepository : IGenericRepository<bank_branch>
    {
        PagedResults<bank_branch> CreatePagedResults(int pageNumber, int pageSize);
        List<dropdown> GetAllDropDown(int? id);
    }
}
