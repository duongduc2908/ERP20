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
using ERP.Data.ModelsERP.ModelView.ExportDB;

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
        public PagedResults<transactionviewmodel> GetAllPageSearch(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name)
        {
            return this._repository.GetAllPageSearch(pageNumber:pageNumber, pageSize:pageSize,start_date, end_date, search_name);
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
        public transactionviewmodel GetById(int tra_id)
        {
            return this._repository.GetById(tra_id);
        }
        public List<dropdown> GetTransactionRate()
        {
            return this._repository.GetTransactionRate();
        }
        public List<transactionstatisticrateviewmodel> GetTransactionStatisticRate(int staff_id)
        {
            return this._repository.GetTransactionStatisticRate(staff_id);
        }
        public PagedResults<transactionview> ExportTransaction(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name)
        {
            return this._repository.ExportTransaction(pageNumber: pageNumber, pageSize: pageSize,start_date, end_date, search_name);
        }
    }
}