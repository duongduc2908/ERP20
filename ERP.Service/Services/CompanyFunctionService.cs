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
    public class CompanyFunctionService : GenericService<company_funtion>, ICompanyFunctionService
    {
        private readonly ICompanyFunctionRepository _repository;
        public CompanyFunctionService(ICompanyFunctionRepository repository) : base(repository)
        {
            this._repository = repository;
        }
    }
}