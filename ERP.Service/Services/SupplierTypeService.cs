using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class SupplierTypeService : GenericService<supplier_type>, ISupplierTypeService
    {
        private readonly ISupplierTypeRepository _repository;
        public SupplierTypeService(ISupplierTypeRepository repository) : base(repository)
        {
            this._repository = repository;
        }
        public PagedResults<supplier_type> GetAllPage(int pageNumber, int pageSize)
        {
            return this._repository.GetAllPage(pageNumber, pageSize);
        }
        public PagedResults<supplier_type> GetAllPageById(int pageNumber, int pageSize, int id)
        {
            return this._repository.GetAllPageById(pageNumber, pageSize, id);
        }

        public PagedResults<supplier_type> GetSuplierType(string search_name)
        {
            return this._repository.GetSuplierType(search_name);
        }
    }
}