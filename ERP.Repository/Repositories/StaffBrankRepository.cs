using AutoMapper;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Repository.Repositories.IRepositories;
using System.Collections.Generic;
using System.Linq;
namespace ERP.Repository.Repositories
{
    public class StaffBrankRepository : GenericRepository<staff_brank>, IStaffBrankRepository
    {
        private readonly IMapper _mapper;
        public StaffBrankRepository(ERPDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._mapper = mapper;
        }
        public StaffBrankRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public PagedResults<staff_brank> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.staff_brank.OrderBy(t => t.stb_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.companies.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<staff_brank>
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