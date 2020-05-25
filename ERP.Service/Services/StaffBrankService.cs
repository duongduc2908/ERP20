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
    public class StaffBrankService : GenericService<staff_brank>, IStaffBrankService
    {
        private readonly IStaffBrankRepository _repository;
        public StaffBrankService(IStaffBrankRepository repository) : base(repository)
        {
            this._repository = repository;
        }


        public PagedResults<staff_brank> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }

        
    }
}