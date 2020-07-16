using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.CustomerGroup;
using ERP.Data.ModelsERP.ModelView.Statistics;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class CustomerGroupService : GenericService<customer_group>, ICustomerGroupService
    {
        private readonly ICustomerGroupRepository _repository;
        public CustomerGroupService(ICustomerGroupRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<customergroupviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? cg_id, string name,int company_id)
        {
            return this._repository.GetAllPageSearch(pageNumber, pageSize, cg_id, name, company_id);
        }
        public List<piechartview> GetPieChart(int companyid)
        {
            return this._repository.GetPieChart(companyid);
        }
        public bool CheckUniqueName(string name,int id)
        {
            return this._repository.CheckUniqueName(name,id);
        }
        public customergroupviewmodel GetById(int cg_id)
        {
            return this._repository.GetById(cg_id);
        }
        public List<statisticrevenueviewmodel> GetRevenueCustomerGroup(int staff_id)
        {
            return this._repository.GetRevenueCustomerGroup(staff_id);
        }
        public List<dropdown> GetAllDropdown(int company_id)
        {
            return this._repository.GetAllDropdown(company_id);
        }
        
    }
}