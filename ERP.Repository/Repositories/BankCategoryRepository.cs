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
    public class BankCategoryRepository : GenericRepository<bank_category>, IBankCategoryRepository
    {
        private readonly IMapper _mapper;
        public BankCategoryRepository(ERPDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._mapper = mapper;
        }
        public BankCategoryRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public PagedResults<bank_category> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.bank_category.OrderBy(t => t.bac_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.companies.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<bank_category>
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
            List<bank_category> list_res = new List<bank_category>();
            
            list_res = _dbContext.bank_category.ToList();
            foreach (var co in list_res)
            {
                dropdown dr = new dropdown();
                //Do something
                dr.id = co.bac_id;
                dr.name = co.bac_name;
                res.Add(dr);
            }
            return res;
        }
    }
}