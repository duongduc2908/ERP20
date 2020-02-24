using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
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
        public PagedResults<customerviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name)
        {
            return this._repository.GetAllPageSearch(pageNumber, pageSize, source_id, cu_type, customer_group_id, name);
        }
        

        public PagedResults<customer> GetInfor(string search_name)
        {
            return this._repository.GetInfor(search_name);
        }
        public PagedResults<customerviewmodel> GetInfor(int cu_id)
        {
            return this._repository.GetInfor(cu_id);
        }
        public List<dropdown> GetAllType()
        {
            return this._repository.GetAllType();
        }

    }
}