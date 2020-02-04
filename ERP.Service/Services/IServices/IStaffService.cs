﻿using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data;
using ERP.Data.ModelsERP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service.Services.IServices
{
    public interface IStaffService : IGenericService<staff>
    {
        PagedResults<staff> CreatePagedResults(int pageNumber, int pageSize);
        void ChangePassword(ChangePasswordBindingModel model, int id);
    }
}
