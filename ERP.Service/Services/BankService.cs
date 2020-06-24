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
    public class BankService : GenericService<bank>, IBankService
    {
        private readonly IBankRepository _repository;
        public BankService(IBankRepository repository) : base(repository)
        {
            this._repository = repository;
        }


        public PagedResults<bank> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        
        public List<dropdown> GetAllDropDown(int? id, string search)
        {
            return this._repository.GetAllDropDown(id,search);
        }
    }
}