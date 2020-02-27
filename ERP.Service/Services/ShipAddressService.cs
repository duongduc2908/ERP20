using ERP.Common.GenericService;
using ERP.Common.Models;
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
    public class ShipAddressService : GenericService<ship_address>, IShipAddressService
    {
        private readonly IShipAddressRepository _repository;
        public ShipAddressService(IShipAddressRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<ship_address> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        public List<dropdown> GetAllProvince()
        {
            return this._repository.GetAllProvince();
        }
        public List<dropdown> GetAllDistrictByIdPro(int? province_id)
        {
            return this._repository.GetAllDistrictByIdPro(province_id);
        }
        public List<dropdown> GetAllWardByIdDis(int? district_id)
        {
            return this._repository.GetAllWardByIdDis(district_id);
        }
    }
}