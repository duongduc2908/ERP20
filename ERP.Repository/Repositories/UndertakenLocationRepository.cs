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
    public class UndertakenLocationRepository : GenericRepository<undertaken_location>, IUndertakenLocationRepository
    {
        public UndertakenLocationRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public PagedResults<undertaken_location> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.undertaken_location.OrderBy(t => t.unl_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.undertaken_location.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<undertaken_location>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
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