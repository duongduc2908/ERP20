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
    public class ProductCategoryService : GenericService<product_category>, IProductCategoryService
    {
        private readonly IProductCategoryRepository _repository;
        public ProductCategoryService(IProductCategoryRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public List<dropdown> GetAllName(int company_id)
        {
            return this._repository.GetAllName(company_id);
        }
    }
}