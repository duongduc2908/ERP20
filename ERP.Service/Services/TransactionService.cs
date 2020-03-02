using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Data.ModelsERP;
using ERP.Service.Services.IServices;
using ERP.Data.ModelsERP.ModelView.Transaction;
using ERP.Data.ModelsERP.ModelView;

namespace ERP.Service.Services
{
    public class TransactionService : GenericService<transaction>, ITransactionService
    {
        private readonly ITransactionRepository _repository;
        public TransactionService(ITransactionRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<transaction> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        public PagedResults<transactionviewmodel> GetAllPageSearch(int pageNumber, int pageSize, string search_name)
        {
            return this._repository.GetAllPageSearch(pageNumber:pageNumber, pageSize:pageSize, search_name);
        }
      
        public List<dropdown> GetTransactionType()
        {
            return this._repository.GetTransactionType();
        }
        public List<dropdown> GetTransactionPriority()
        {
            return this._repository.GetTransactionPriority();
        }
        public List<dropdown> GetTransactionStatus()
        {
            return this._repository.GetTransactionStatus();
        }
    }
}