using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class SmsStrategyRepository : GenericRepository<sms_strategy>, ISmsStrategyRepository
    {
        public SmsStrategyRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public PagedResults<sms_strategy> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.sms_strategy.OrderBy(t => t.sms_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.sms_strategy.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<sms_strategy>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
    }
}