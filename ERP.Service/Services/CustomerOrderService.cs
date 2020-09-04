using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.CustomerOrder;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.OrderService;
using ERP.Data.ModelsERP.ModelView.Service;
using ERP.Data.ModelsERP.ModelView.Statistics;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;

namespace ERP.Service.Services
{
    public class CustomerOrderService : GenericService<customer_order>, ICustomerOrderService
    {
        private readonly ICustomerOrderRepository _repository;
        public CustomerOrderService(ICustomerOrderRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<customerorderviewmodel> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        public customerordermodelview GetAllOrderById(int id)
        {
            return this._repository.GetAllOrderById(id);
        }
        public servicercustomerorderviewmodel GetAllOrderServiceById(int id)
        {
            return this._repository.GetAllOrderServiceById(id);
        }
        public List<order_service_view> GetServiceByDay(string role ,int id, DateTime start_date, DateTime to_date)
        {
            return this._repository.GetServiceByDay(role,id, start_date, to_date);
        }
       
        public PagedResults<customerorderviewmodel> GetAllSearch(int pageNumber, int pageSize, int? payment_type_id,int? cuo_status, DateTime? start_date, DateTime? end_date, string name,int company_id)
        {
            return this._repository.GetAllSearch(pageNumber, pageSize,  payment_type_id, cuo_status,start_date, end_date, name, company_id);
        }
         public PagedResults<servicercustomerorderviewmodel> GetAllSearchCustomerOrderService(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name,int company_id)
        {
            return this._repository.GetAllSearchCustomerOrderService(pageNumber, pageSize,  start_date,end_date, search_name, company_id);
        }

        public PagedResults<customerorderproductview> ExportCustomerOrderProduct(int pageNumber, int pageSize, int? payment_type_id, DateTime? start_date, DateTime? end_date, string name,int company_id)
        {
            return this._repository.ExportCustomerOrderProduct(pageNumber, pageSize,  payment_type_id,start_date, end_date, name, company_id);
        }
        public List<dropdown> GetAllPayment()
        {
            return this._repository.GetAllPayment();
        }
        public List<dropdown_salary> Get_staff_free(work_time_view c, string fullName,int company_id)
        {
            return this._repository.Get_staff_free(c, fullName,company_id);
        }
        
        public PagedResults<customerorderviewmodel> ResultStatisticsCustomerOrder(int pageNumber, int pageSize, int staff_id, bool month, bool week, bool day,int company_id)
        {
            return this._repository.ResultStatisticsCustomerOrder(pageNumber, pageSize, staff_id,month,  week, day, company_id);
        }
        
        public statisticsbyrevenueviewmodel ResultStatisticsByRevenue(int staff_id)
        {
            return this._repository.ResultStatisticsByRevenue(staff_id);
        }
        public List<revenue> ResultStatisticByMonth(int staff_id)
        {
            return this._repository.ResultStatisticByMonth(staff_id);
        }
        
        public List<dropdown> GetAllStatus()
        {
            return this._repository.GetAllStatus();
        }
    }
}