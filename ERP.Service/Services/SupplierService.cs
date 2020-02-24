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
    public class SupplierService : GenericService<supplier>, ISupplierService
    {
        private readonly ISupplierRepository _repository;
        public SupplierService(ISupplierRepository repository) : base(repository)
        {
            this._repository = repository;
        }
        public PagedResults<supplier> GetAllPage(int pageNumber, int pageSize)
        {
            return this._repository.GetAllPage(pageNumber, pageSize);
        }
        public PagedResults<supplier> GetAllPageById(int pageNumber, int pageSize, int id)
        {
            return this._repository.GetAllPageById(pageNumber, pageSize, id);
        }
        public List<dropdown> GetAllName()
        {
            return this._repository.GetAllName();
        }


        public PagedResults<supplier> GetSupliers(string search_name)
        {
            return this._repository.GetSupliers(search_name);
        }
    }
}