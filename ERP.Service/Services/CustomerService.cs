using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Customer;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.Service;
using ERP.Data.ModelsERP.ModelView.Sms;
using ERP.Data.ModelsERP.ModelView.Transaction;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class CustomerService : GenericService<customer>, ICustomerService
    {
        private readonly ICustomerRepository _repository;
        public CustomerService(ICustomerRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<customerviewmodel> GetAllPage(int pageNumber, int pageSize)
        {
            return this._repository.GetAllPage(pageNumber, pageSize);
        }
        public PagedResults<customerviewmodel> GetAllPageBySource(int pageNumber, int pageSize, int source_id)
        {
            return this._repository.GetAllPageBySource(pageNumber, pageSize,source_id);
        }
        public PagedResults<customerviewmodel> GetAllPageByType(int pageNumber, int pageSize,int cu_type)
        {
            return this._repository.GetAllPageByType(pageNumber, pageSize, cu_type);
        }
        public PagedResults<customerviewmodel> GetAllPageByGroup(int pageNumber, int pageSize,int customer_group_id)
        {
            return this._repository.GetAllPageByGroup(pageNumber, pageSize, customer_group_id);
        }
        public PagedResults<customerviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, DateTime? start_date, DateTime? end_date, string name,int company_id)
        {
            return this._repository.GetAllPageSearch(pageNumber, pageSize, source_id, cu_type, customer_group_id,start_date,end_date, name,company_id);
        }
        public PagedResults<customeraddressviewmodel> GetCustomerByCurator(int pageSize, int pageNumber, int? cu_curator_id, string search_name, int company_id)
        {
            return this._repository.GetCustomerByCurator(pageSize, pageNumber, cu_curator_id,search_name,company_id);
        }
        public bool Check_location(ship_address sa)
        {
            return this._repository.Check_location(sa);
        }
        public PagedResults<smscustomerviewmodel> GetAllPageSearchSms(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name, int company_id)
        {
            return this._repository.GetAllPageSearchSms(pageNumber, pageSize, source_id, cu_type, customer_group_id, name,company_id);
        }
        public PagedResults<servicesearchcustomerviewmodel> GetAllPageSearchService(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name, int company_id)
        {
            return this._repository.GetAllPageSearchService(pageNumber, pageSize, source_id, cu_type, customer_group_id,name,company_id);
        }
       
        public customerviewmodel GetInfor(int cu_id)
        {
            return this._repository.GetInfor(cu_id);
        }
        public servicesearchcustomerviewmodel GetServiceInforCustomer(int cu_id)
        {
            return this._repository.GetServiceInforCustomer(cu_id);
        }

        public transactioncustomerviewmodel GetInforCustomerTransaction(int cu_id)
        {
            return this._repository.GetInforCustomerTransaction(cu_id);
        }
        public List<dropdown> GetAllType(int company_id)
        {
            return this._repository.GetAllType(company_id);
        }
        public List<dropdown> GetAllDropdown(int company_id)
        {
            return this._repository.GetAllDropdown(company_id);
        }
        public PagedResults<customerviewexport> ExportCustomer(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, DateTime? start_date, DateTime? end_date, string name, int company_id)
        {
            return this._repository.ExportCustomer(pageNumber, pageSize, source_id, cu_type, customer_group_id,start_date, end_date, name,company_id);
        }

    }
}