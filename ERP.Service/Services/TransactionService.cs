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
        public PagedResults<transactionviewmodel> GetAllPageSearch(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name,int company_id, int curr_id)
        {
            return this._repository.GetAllPageSearch(pageNumber:pageNumber, pageSize:pageSize,start_date, end_date, search_name,company_id,curr_id);
        }
      
        public List<dropdown> GetTransactionType(int company_id)
        {
            return this._repository.GetTransactionType(company_id);
        }
        public List<dropdown> GetTransactionPriority(int company_id)
        {
            return this._repository.GetTransactionPriority(company_id);
        }
        public List<dropdown> GetTransactionStatus(int company_id)
        {
            return this._repository.GetTransactionStatus(company_id);
        }
        public transactionviewmodel GetById(int tra_id)
        {
            return this._repository.GetById(tra_id);
        }
        public List<dropdown> GetTransactionRate(int company_id)
        {
            return this._repository.GetTransactionRate(company_id);
        }
        public List<transactionstatisticrateviewmodel> GetTransactionStatisticRate(int staff_id, int company_id)
        {
            return this._repository.GetTransactionStatisticRate(staff_id,company_id);
        }
        public PagedResults<transactionview> ExportTransaction(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name,int company_id)
        {
            return this._repository.ExportTransaction(pageNumber: pageNumber, pageSize: pageSize,start_date, end_date, search_name, company_id);
        }
    }
}