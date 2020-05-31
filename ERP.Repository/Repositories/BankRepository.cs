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
    public class BankRepository : GenericRepository<bank>, IBankRepository
    {
        private readonly IMapper _mapper;
        public BankRepository(ERPDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._mapper = mapper;
        }
        public BankRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public PagedResults<bank> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.banks.OrderBy(t => t.ba_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.companies.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<bank>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }

        public List<dropdown> GetAllDropDown(int? id)
        {
            List<dropdown> res = new List<dropdown>();
            List<bank> list_res = new List<bank>();
            if (id != null)
            {
                list_res = _dbContext.banks.Where(x => x.bank_category_id == id ).ToList();
            }
            else list_res = _dbContext.banks.ToList();
           
            foreach (var co in list_res)
            {
                dropdown dr = new dropdown();
                //Do something
                dr.id = co.ba_id;
                dr.name= co.ba_name;
                res.Add(dr);
            }
            return res;
        }
    }
}