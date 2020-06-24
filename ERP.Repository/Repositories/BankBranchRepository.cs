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
    public class BankBranchRepository : GenericRepository<bank_branch>, IBankBranchRepository
    {
        private readonly IMapper _mapper;
        public BankBranchRepository(ERPDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._mapper = mapper;
        }
        public BankBranchRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public PagedResults<bank_branch> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.bank_branch.OrderBy(t => t.bbr_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.companies.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<bank_branch>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }

        public List<dropdown> GetAllDropDown(int? id, string search)
        {
            List<dropdown> res = new List<dropdown>();
            List<bank_branch> list_res = new List<bank_branch>();
            if (id != null)
            {
                list_res = _dbContext.bank_branch.Where(x => x.bank_id == id).ToList();
            }
            else list_res = _dbContext.bank_branch.ToList();
            if(search != null)
            {
                list_res = _dbContext.bank_branch.Where(x => x.bbr_name.Trim().ToLower().Contains(search.Trim().ToLower())).ToList();
            }
            
            foreach (var co in list_res)
            {
                dropdown dr = new dropdown();
                //Do something
                dr.id = co.bbr_id;
                dr.name = co.bbr_name;
                res.Add(dr);
            }
            return res;
        }
    }
}