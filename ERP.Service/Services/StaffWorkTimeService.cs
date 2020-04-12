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
    public class StaffWorkTimeService : GenericService<staff_work_time>, IStaffWorkTimeService
    {
        private readonly IStaffWorkTimeRepository _repository;
        public StaffWorkTimeService(IStaffWorkTimeRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<staff_work_time> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
    }
}