using ERP.Common.GenericService;
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
    public class StaffTypeService : GenericService<staff_type>, IStaffTypeService
    {
        private readonly IStaffTypeRepository _repository;
        public StaffTypeService(IStaffTypeRepository repository) : base(repository)
        {
            this._repository = repository;
        }
        public List<dropdown> GetAllDropdown(int company_id)
        {
            return this._repository.GetAllDropdown(company_id);
        }

    }
}