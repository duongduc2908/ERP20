using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.CustomerOrder;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.OrderService;
using ERP.Data.ModelsERP.ModelView.Service;
using ERP.Data.ModelsERP.ModelView.Statistics;
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
        servicercustomerorderviewmodel GetAllOrderServiceById(int id);
        List<order_service_view> GetServiceByDay(int id, DateTime start_date, DateTime to_date);
        PagedResults<customerorderviewmodel> GetAllSearch(int pageNumber, int pageSize, int? payment_type_id, DateTime? start_date, DateTime? end_date, string name);
        PagedResults<servicercustomerorderviewmodel> GetAllSearchCustomerOrderService(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name);
        PagedResults<customerorderview> ExportCustomerOrder(int pageNumber, int pageSize, int? payment_type_id, DateTime? start_date, DateTime? end_date, string name);
        PagedResults<customerorderviewmodel> ResultStatisticsCustomerOrder(int pageNumber, int pageSize, int staff_id, bool month, bool week, bool day);
        List<dropdown> GetAllPayment();
        List<dropdown> Get_staff_free(work_time_view c, string fullName);
        statisticsbyrevenueviewmodel ResultStatisticsByRevenue(int staff_id);
        List<revenue> ResultStatisticByMonth(int staff_id);

        List<dropdown> GetAllStatus();
    }
}
