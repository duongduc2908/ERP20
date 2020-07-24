using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;

namespace ERP.Service.Services
{
    public class DeviceStaffService : GenericService<device_staff>, IDeviceStaffService
    {
        private readonly IDeviceStaffRepository _repository;
        public DeviceStaffService(IDeviceStaffRepository repository) : base(repository)
        {
            this._repository = repository;
        }
        public PagedResults<device_staff> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
    }
}