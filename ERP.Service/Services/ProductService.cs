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
    public class ProductService : GenericService<product>, IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository) : base(repository)
        {
            this._repository = repository;
        }
        public PagedResults<productviewmodel> GetAllPage(int pageNumber, int pageSize)
        {
            return this._repository.GetAllPage(pageNumber, pageSize);
        }
        public PagedResults<productviewmodel> GetAllPageById( int id)
        {
            return this._repository.GetAllPageById(id);
        }
        
        public PagedResults<product> GetProducts(string search_name)
        {
            return this._repository.GetProducts(search_name);
        }
    }
}