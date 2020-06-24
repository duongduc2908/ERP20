using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface IBankRepository : IGenericRepository<bank>
    {
        PagedResults<bank> CreatePagedResults(int pageNumber, int pageSize);
        List<dropdown> GetAllDropDown(int? id, string search);
    }
}
