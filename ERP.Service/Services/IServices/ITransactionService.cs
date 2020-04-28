using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service.Services.IServices
{
    public interface ITransactionService : IGenericService<transaction>
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
