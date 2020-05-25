using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Company;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;

namespace ERP.Service.Services
{
    public class RelativesStaffService : GenericService<relatives_staff>, IRelativesStaffService
    {
        private readonly IRelativesStaffRepository _repository;
        public RelativesStaffService(IRelativesStaffRepository repository) : base(repository)
        {
            this._repository = repository;
        }


        public PagedResults<relatives_staff> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }

        public List<dropdown> GetAllDropDown()
        {
            return this._repository.GetAllDropDown();
        }
    }
}