using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ERP.Common.Constants.Enums;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Device;
using ERP.Repository.Repositories.IRepositories;

namespace ERP.Repository.Repositories
{
    public class DeviceRepository : GenericRepository<device>, IDeviceRepository
    {
        private readonly IMapper _mapper;
        public DeviceRepository(ERPDbContext dbContext, IMapper _mapper) : base(dbContext)
        {
            this._mapper = _mapper;
        }
        public PagedResults<device> CreatePagedResults(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
        public List<dropdown> GetAllDropDown(int company_id)
        {
            List<dropdown> res = new List<dropdown>();
            List<device> lts_s = _dbContext.devices.Where(x => x.company_id == company_id).ToList();
            foreach (device cg in lts_s)
            {
                dropdown dr = new dropdown();
                dr.id = cg.dev_id;
                dr.name = cg.dev_name;
                res.Add(dr);
            }
            return res;
        }
        public PagedResults<deviceviewmodel> GetAllSearch(int pageNumber, int pageSize, string search_name, int company_id)
        {
            if (search_name != null) search_name = search_name.Trim().ToLower();
            List<deviceviewmodel> res = new List<deviceviewmodel>();
            List<device> list = new List<device>();
            var skipAmount = pageSize * pageNumber;
            if (search_name == null)
            {
                list = _dbContext.devices.Where(t => t.company_id == company_id).ToList();
            }
            else list = _dbContext.devices.Where(t => (t.dev_name.ToLower().Contains(search_name) || t.dev_code.ToLower().Contains(search_name)) && t.company_id == company_id).ToList();
            var total = list.Count();

            var results = list.OrderByDescending(t => t.dev_id).Skip(skipAmount).Take(pageSize);
            foreach (device i in results)
            {
                var deviceview = _mapper.Map<deviceviewmodel>(i);

                for (int j = 1; j < EnumDevice.dev_unit.Length + 1; j++)
                {
                    if (j == i.dev_unit)
                    {
                        deviceview.dev_unit_name = EnumDevice.dev_unit[j - 1];
                    }
                }
                res.Add(deviceview);
            }
            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<deviceviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };

        }
        public deviceviewmodel GetById(int dev_id)
        {
            var results = _dbContext.devices.Where(i => i.dev_id == dev_id).FirstOrDefault();
            var deviceview = _mapper.Map<deviceviewmodel>(results);
            for (int j = 1; j < EnumDevice.dev_unit.Length + 1; j++)
            {
                if (j == deviceview.dev_unit)
                {
                    deviceview.dev_unit_name = EnumDevice.dev_unit[j - 1];
                }
            }

            return deviceview;
        }
        public List<dropdown> GetUnit(int company_id)
        {

            List<dropdown> res = new List<dropdown>();
            List<device_unit> lts_s = _dbContext.device_unit.Where(x => x.company_id == company_id).ToList();
            foreach (var so in lts_s)
            {
                dropdown dr = new dropdown();
                dr.id = so.deuni_id;
                dr.name = so.deuni_name;
                res.Add(dr);
            }
            return res;
        }
    }
}