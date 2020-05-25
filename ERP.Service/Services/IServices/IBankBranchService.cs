﻿using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System.Collections.Generic;

namespace ERP.Service.Services.IServices
{
    public interface IBankBranchService : IGenericService<bank_branch>
    {
        PagedResults<bank_branch> CreatePagedResults(int pageNumber, int pageSize);
        List<dropdown> GetAllDropDown(int? id);
    }
}
