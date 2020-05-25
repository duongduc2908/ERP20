using AutoMapper;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Company;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class BonusStaffRepository : GenericRepository<bonus_staff>, IBonusStaffRepository
    {
        private readonly IMapper _mapper;
        public BonusStaffRepository(ERPDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._mapper = mapper;
        }
        public BonusStaffRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public PagedResults<bonus_staff> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.bonus_staff.OrderBy(t => t.bos_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.companies.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<bonus_staff>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
       
        public List<dropdown> GetAllDropDown()
        {
            List<dropdown> res = new List<dropdown>();
            var list_company = _dbContext.bonus_staff.ToList();
            foreach (var co in list_company)
            {
                dropdown dr = new dropdown();
                //Do something
                res.Add(dr);
            }
            return res;
        }
    }
}