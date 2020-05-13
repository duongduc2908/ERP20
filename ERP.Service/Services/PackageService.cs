using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Package;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class PackageService : GenericService<package>, IPackageService
    {
        private readonly IPackageRepository _repository;
        public PackageService(IPackageRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<package> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        public PagedResults<packageviewmodel> GetAllSearch(int pageNumber, int pageSize,  string search_name)
        {
            return this._repository.GetAllSearch(pageNumber, pageSize,  search_name);
        }
        public packageviewmodel GetById(int id)
        {
            return this._repository.GetById(id);
        }
        public List<dropdown> GetAllDropDown()
        {
            return this._repository.GetAllDropDown();
        }
    }
}