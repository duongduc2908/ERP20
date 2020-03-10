using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface IOrderProductRepository : IGenericRepository<order_product>
    {
        PagedResults<order_product> CreatePagedResults(int pageNumber, int pageSize);
        PagedResults<orderproductviewmodel> GetAllOrderProduct(int customer_order_id);
        PagedResults<statisticsorderviewmodel> ResultStatisticsOrder(int pageNumber, int pageSize, int staff_id, bool month, bool week, bool day, string search_name);
    }
}
