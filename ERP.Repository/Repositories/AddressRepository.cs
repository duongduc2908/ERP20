using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        public ERPDbContext _dbContext;
        public AddressRepository(ERPDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public List<dropdown> GetAllProvince()
        {
            List<dropdown> lst_res = new List<dropdown>();
            var list = _dbContext.province.ToList();
            foreach (Province p in list)
            {
                dropdown dr = new dropdown();
                dr.id = p.Id;
                dr.name = p.Name;
                lst_res.Add(dr);
            }
            return lst_res;
        }
        public List<dropdown> GetAllDistrictByIdPro(int? province_id)
        {
            List<dropdown> lst_res = new List<dropdown>();
            var list = _dbContext.district.Where(i => i.ProvinceId == province_id).ToList();
            foreach (District d in list)
            {
                dropdown dr = new dropdown();
                dr.id = d.Id;
                dr.name = d.Name;
                lst_res.Add(dr);
            }
            return lst_res;
        }
        public List<dropdown> GetAllWardByIdDis(int? district_id)
        {
            List<dropdown> lst_res = new List<dropdown>();
            var list = _dbContext.ward.Where(i => i.DistrictID == district_id).ToList();
            foreach (Ward p in list)
            {
                dropdown dr = new dropdown();
                dr.id = p.Id;
                dr.name = p.Name;
                lst_res.Add(dr);
            }
            return lst_res;
        }
    }
}