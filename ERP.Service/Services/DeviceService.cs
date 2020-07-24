using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Device;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;

namespace ERP.Service.Services
{
    public class DeviceService : GenericService<device>, IDeviceService
    {
        private readonly IDeviceRepository _repository;
        public DeviceService(IDeviceRepository repository) : base(repository)
        {
            this._repository = repository;
        }
        public PagedResults<device> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber,pageSize);
        }
        public List<dropdown> GetAllDropDown(int company_id)
        {
            return this._repository.GetAllDropDown(company_id);
        }

        public PagedResults<deviceviewmodel> GetAllSearch(int pageNumber, int pageSize, string search_name, int compnay_id)
        {
            return this._repository.GetAllSearch(pageNumber, pageSize,search_name,compnay_id);
        }

        public deviceviewmodel GetById(int dev_id)
        {
            throw new NotImplementedException();
        }
    }
}