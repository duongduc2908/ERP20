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
    public class RelativesStaffRepository : GenericRepository<relatives_staff>, IRelativesStaffRepository
    {
        private readonly IMapper _mapper;
        public RelativesStaffRepository(ERPDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._mapper = mapper;
        }
        public RelativesStaffRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public PagedResults<relatives_staff> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.relatives_staff.OrderBy(t => t.rels_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.companies.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<relatives_staff>
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