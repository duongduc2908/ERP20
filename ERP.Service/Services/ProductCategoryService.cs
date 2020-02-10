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
        public PagedResults<product_category> GetAllPage(int pageNumber, int pageSize)
        {
            return this._repository.GetAllPage(pageNumber, pageSize);
        }
        public PagedResults<product_category> GetAllPageById(int pageNumber, int pageSize, int id)
        {
            return this._repository.GetAllPageById(pageNumber, pageSize, id);
        }

        public PagedResults<product_category> GetProductCategorys(string search_name)
        {
            throw new NotImplementedException();
        }
    }
}