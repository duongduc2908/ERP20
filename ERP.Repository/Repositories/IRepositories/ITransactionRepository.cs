using ERP.Common.GenericRepository;
using ERP.Common.Models;
using System;
using ERP.Data.ModelsERP;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Data.ModelsERP.ModelView.Transaction;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.ExportDB;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface ITransactionRepository : IGenericRepository<transaction>
    {
        PagedResults<transaction> CreatePagedResults(int pageNumber, int pageSize);
        PagedResults<transactionviewmodel> GetAllPageSearch(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name, int company_id);
        PagedResults<transactionview> ExportTransaction(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name, int company_id);
        List<dropdown> GetTransactionType(int company_id);
        transactionviewmodel GetById(int tra_id);
        List<dropdown> GetTransactionPriority(int company_id);
        List<dropdown> GetTransactionStatus(int company_id);
        List<dropdown> GetTransactionRate(int company_id);
        List<transactionstatisticrateviewmodel> GetTransactionStatisticRate(int staff_id, int company_id);
    }
}
