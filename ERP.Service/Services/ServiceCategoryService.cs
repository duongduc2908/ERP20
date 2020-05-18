using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class ServiceCategoryService : GenericService<service_category>, IServiceCategoryService
    {
        private readonly IServiceCategoryRepository _repository;
        public ServiceCategoryService(IServiceCategoryRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<service_category> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        public List<dropdown> GetAllName(int company_id)
        {
            return this._repository.GetAllName(company_id);
        }
        
    }
}