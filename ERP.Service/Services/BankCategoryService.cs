using System;
using System.Collections.Generic;
using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Company;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;

namespace ERP.Service.Services
{
    public class BankCategoryService : GenericService<bank_category>, IBankCategoryService
    {
        private readonly IBankCategoryRepository _repository;
        public BankCategoryService(IBankCategoryRepository repository) : base(repository)
        {
            this._repository = repository;
        }


        public PagedResults<bank_category> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }

        public List<dropdown> GetAllDropDown(string search)
        {
            return this._repository.GetAllDropDown(search);
        }
    }
}