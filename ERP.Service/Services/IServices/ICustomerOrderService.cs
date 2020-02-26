using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service.Services.IServices
{
    public interface ICustomerOrderService : IGenericService<customer_order>
    {
        PagedResults<customerorderviewmodel> CreatePagedResults(int pageNumber, int pageSize);
        customerordermodelview GetAllOrderById(int id);
        PagedResults<customerorderviewmodel> GetAllSearch(int pageNumber, int pageSize, int? payment_type_id, string name);
        int ResultStatisticsByMonth(int staff_id);
        int ResultStatisticsByWeek(int staff_id);
        int ResultStatisticsByDay(int staff_id);
    }
}
