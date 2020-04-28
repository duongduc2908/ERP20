﻿using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.CustomerGroup;
using ERP.Data.ModelsERP.ModelView.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface ICustomerGroupRepository : IGenericRepository<customer_group>
    {
        PagedResults<customergroupviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? cg_id, string name);
        List<piechartview> GetPieChart();
        bool CheckUniqueName(string name,int id);

        List<statisticrevenueviewmodel> GetRevenueCustomerGroup(int staff_id);
        List<dropdown> GetAllDropdown();
        customergroupviewmodel GetById(int cg_id);


    }
}
