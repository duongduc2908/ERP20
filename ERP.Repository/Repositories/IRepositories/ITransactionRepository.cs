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

namespace ERP.Repository.Repositories.IRepositories
{
    public interface ITransactionRepository : IGenericRepository<transaction>
    {
        PagedResults<transaction> CreatePagedResults(int pageNumber, int pageSize);
        PagedResults<transactionviewmodel> GetAllPageSearch(int pageNumber, int pageSize, string search_name);
        List<dropdown> GetTransactionType();
        List<dropdown> GetTransactionPriority();
        List<dropdown> GetTransactionStatus();
    }
}
