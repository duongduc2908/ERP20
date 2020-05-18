﻿using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service.Services.IServices
{
    public interface IServiceService : IGenericService<service>
    {
        PagedResults<service> CreatePagedResults(int pageNumber, int pageSize);
        PagedResults<serviceviewmodel> GetAllPageSearch(int pageNumber, int pageSize,string search_name,int comapny_id);
        serviceviewmodel GetById(int se_id);
        List<dropdown> GetAllDropdown(int comapny_id);
        PagedResults<serviceinforviewmodel> GetAllPageInforService(int pageNumber, int pageSize,string search_name, int comapny_id);
        List<dropdown> GetType(int comapny_id);
        
    }
}
