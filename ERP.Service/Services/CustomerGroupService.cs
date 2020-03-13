using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.CustomerGroup;
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

        public PagedResults<customergroupviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? cg_id, string name)
        {
            return this._repository.GetAllPageSearch(pageNumber, pageSize, cg_id, name);
        }
        public List<piechartview> GetPieChart()
        {
            return this._repository.GetPieChart();
        }
        public bool CheckUniqueName(string name,int id)
        {
            return this._repository.CheckUniqueName(name,id);
        }
        
    }
}