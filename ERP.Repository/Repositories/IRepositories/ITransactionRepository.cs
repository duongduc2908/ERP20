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
        PagedResults<transactionviewmodel> GetAllPageSearch(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name);
        PagedResults<transactionview> ExportTransaction(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name);
        List<dropdown> GetTransactionType();
        transactionviewmodel GetById(int tra_id);
        List<dropdown> GetTransactionPriority();
        List<dropdown> GetTransactionStatus();
        List<dropdown> GetTransactionRate();
        List<transactionstatisticrateviewmodel> GetTransactionStatisticRate(int staff_id);
    }
}
