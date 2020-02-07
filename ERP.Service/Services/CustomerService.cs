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

        public PagedResults<customer> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        public PagedResults<customer> GetInfor(string search_name)
        {
            return this._repository.GetInfor(search_name);
        }
        
    }
}