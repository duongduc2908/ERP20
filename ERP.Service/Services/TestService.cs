using ERP.Common.GenericService;
using ERP.Data.ModelsERP;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class TestService: GenericService<customer>, ITestService
    {
        private readonly ICustomerRepository _repository;
        public TestService(ICustomerRepository repository) : base(repository)
        {
            this._repository = repository;
        }
    }
}