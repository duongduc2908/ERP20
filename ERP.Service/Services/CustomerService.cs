using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
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

        public PagedResults<customer> GetAllPage(int pageNumber, int pageSize)
        {
            return this._repository.GetAllPage(pageNumber, pageSize);
        }
        public PagedResults<customer> GetAllPageBySource(int pageNumber, int pageSize, int source_id)
        {
            return this._repository.GetAllPageBySource(pageNumber, pageSize,source_id);
        }
        public PagedResults<customer> GetAllPageByType(int pageNumber, int pageSize,int cu_type)
        {
            return this._repository.GetAllPageByType(pageNumber, pageSize, cu_type);
        }
        public PagedResults<customer> GetAllPageByGroup(int pageNumber, int pageSize,int customer_group_id)
        {
            return this._repository.GetAllPageByGroup(pageNumber, pageSize, customer_group_id);
        }
        public PagedResults<customer> GetInfor(string search_name)
        {
            return this._repository.GetInfor(search_name);
        }
        
    }
}