﻿using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services.IServices
{
    public interface ICustomerService : IGenericService<customer>
    {
        PagedResults<customerviewmodel> GetAllPage(int pageNumber, int pageSize);
        PagedResults<customerviewmodel> GetAllPageBySource(int pageNumber, int pageSize, int source_id);
        PagedResults<customerviewmodel> GetAllPageByType(int pageNumber, int pageSize, int cu_type);
        PagedResults<customerviewmodel> GetAllPageByGroup(int pageNumber, int pageSize, int customer_group_id);
        PagedResults<customerviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name);
        
        customerviewmodel GetInfor(int cu_id);
        List<dropdown> GetAllType();
        
    }
}