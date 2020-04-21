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
        public PagedResults<serviceviewmodel> GetAllPageSearch(int pageNumber, int pageSize, string search_name)
        {
            return this._repository.GetAllPageSearch(pageNumber,pageSize,search_name);
        }
        public PagedResults<serviceinforviewmodel> GetAllPageInforService(int pageNumber, int pageSize, string search_name)
        {
            return this._repository.GetAllPageInforService(pageNumber,pageSize,search_name);
        }
        public List<dropdown> GetType()
        {
            return this._repository.GetType();
        }
        public serviceviewmodel GetById(int se_id)
        {
            return this._repository.GetById(se_id);
        }
        public List<dropdown> GetAllDropdown()
        {
            return this._repository.GetAllDropdown();
        }
    }
}