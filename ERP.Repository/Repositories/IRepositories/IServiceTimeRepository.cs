﻿using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface IServiceTimeRepository : IGenericRepository<service_time>
    {
        PagedResults<service_time> CreatePagedResults(int pageNumber, int pageSize);
        List<dropdown> GetRepeatType();
    }
}