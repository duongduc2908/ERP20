using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Service;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class ServiceService : GenericService<service>, IServiceService
    {
        private readonly IServiceRepository _repository;
        public ServiceService(IServiceRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<service> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        public PagedResults<serviceviewmodel> GetAllPageSearch(int pageNumber, int pageSize, string search_name,int company_id)
        {
            return this._repository.GetAllPageSearch(pageNumber,pageSize,search_name, company_id);
        }
        public PagedResults<serviceinforviewmodel> GetAllPageInforService(int pageNumber, int pageSize, string search_name,int company_id)
        {
            return this._repository.GetAllPageInforService(pageNumber,pageSize,search_name, company_id);
        }
        public List<dropdown> GetType(int company_id)
        {
            return this._repository.GetType(company_id);
        }
        public serviceviewmodel GetById(int se_id)
        {
            return this._repository.GetById(se_id);
        }
        public List<dropdown> GetAllDropdown(int company_id)
        {
            return this._repository.GetAllDropdown(company_id);
        }
    }
}