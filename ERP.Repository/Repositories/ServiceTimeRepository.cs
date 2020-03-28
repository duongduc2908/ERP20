using ERP.Common.Constants.Enums;
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
    public class ServiceTimeRepository : GenericRepository<service_time>, IServiceTimeRepository
    {
        public ServiceTimeRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public PagedResults<service_time> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.service_time.OrderBy(t => t.st_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.service_time.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<service_time>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public List<dropdown> GetRepeatType()
        {

            List<dropdown> res = new List<dropdown>();
            for (int i = 1; i < EnumRepeatType.st_repeat_type.Length + 1; i++)
            {
                dropdown pu = new dropdown();
                pu.id = i;
                pu.name = EnumRepeatType.st_repeat_type[i - 1];

                res.Add(pu);
            }
            return res;
        }
    }
}