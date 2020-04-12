using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Customer;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.Service;
using ERP.Data.ModelsERP.ModelView.Sms;
using ERP.Data.ModelsERP.ModelView.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services.IServices
{
    public interface ICustomerService : IGenericService<customer>
    {
        PagedResults<customerviewmodel> GetAllPage(int pageNumber, int pageSize);
        PagedResults<customerviewmodel> GetAllPageBySource(int pageNumber, int pageSize, int source_id);
        PagedResults<customerviewmodel> GetAllPageByType(int pageNumber, int pageSize, int cu_type);
        PagedResults<customerviewmodel> GetAllPageByGroup(int pageNumber, int pageSize, int customer_group_id);
        PagedResults<customerviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, DateTime? start_date, DateTime? end_date, string name);
        PagedResults<customeraddressviewmodel> GetCustomerByCurator(int pageSize, int pageNumber, int? cu_curator_id, string search_name);
        PagedResults<servicesearchcustomerviewmodel> GetAllPageSearchService(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name);
        PagedResults<smscustomerviewmodel> GetAllPageSearchSms(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name);
        PagedResults<customerview> ExportCustomer(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, DateTime? start_date, DateTime? end_date, string name);
        customerviewmodel GetInfor(int cu_id);
        servicesearchcustomerviewmodel GetServiceInforCustomer(int cu_id);
        List<dropdown> GetAllType();
        transactioncustomerviewmodel GetInforCustomerTransaction(int cu_id);


    }
}