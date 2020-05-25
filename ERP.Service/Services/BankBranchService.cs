using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;

namespace ERP.Service.Services
{
    public class BankBranchService : GenericService<bank_branch>, IBankBranchService
    {
        private readonly IBankBranchRepository _repository;
        public BankBranchService(IBankBranchRepository repository) : base(repository)
        {
            this._repository = repository;
        }


        public PagedResults<bank_branch> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }

        public List<dropdown> GetAllDropDown(int? id)
        {
            return this._repository.GetAllDropDown(id);
        }
    }
}