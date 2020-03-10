using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service.Services.IServices
{
    public interface IOrderProductService : IGenericService<order_product>
    {
        PagedResults<order_product> CreatePagedResults(int pageNumber, int pageSize);
        PagedResults<orderproductviewmodel> GetAllOrderProduct(int customer_order_id);
        PagedResults<statisticsorderviewmodel> ResultStatisticsOrder(int pageNumber, int pageSize, int staff_id, bool month, bool week, bool day, string search_name);
    }
}
