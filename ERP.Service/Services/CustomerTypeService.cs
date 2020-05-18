using ERP.Common.GenericService;
using ERP.Data.ModelsERP;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class CustomerTypeService : GenericService<customer_type>, ICustomerTypeService
    {
        private readonly ICustomerTypeRepository _repository;
        public CustomerTypeService(ICustomerTypeRepository repository) : base(repository)
        {
            this._repository = repository;
        }
    }
}