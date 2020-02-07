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
    public class CustomerOrderService : GenericService<customer_order>, ICustomerOrderService
    {
        private readonly ICustomerOrderRepository _repository;
        public CustomerOrderService(ICustomerOrderRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<customer_order> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
    }
}