using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Company;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class CompanyService : GenericService<company>, ICompanyService
    {
        private readonly ICompanyRepository _repository;
        public CompanyService(ICompanyRepository repository) : base(repository)
        {
            this._repository = repository;
        }
        

        public PagedResults<company> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        public PagedResults<companyviewmodel> GetAllSearch(int pageNumber, int pageSize, string search_name)
        {
            return this._repository.GetAllSearch(pageNumber, pageSize, search_name);
        }
        public companyviewmodel GetById(int id, bool? login = false)
        {
            return this._repository.GetById(id, login);
        }
        public List<dropdown> GetAllDropDown()
        {
            return this._repository.GetAllDropDown();
        }
    }
}