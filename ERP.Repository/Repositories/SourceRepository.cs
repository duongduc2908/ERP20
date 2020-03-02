using AutoMapper;
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
    public class SourceRepository : GenericRepository<source>, ISourceRepository
    {
        private readonly IMapper _mapper;
        public SourceRepository(ERPDbContext dbContext, IMapper _mapper) : base(dbContext)
        {
            this._mapper = _mapper;
        }

        public PagedResults<source> GetAllPageById(int pageNumber, int pageSize, int id)
        {
            List<source> res = new List<source>();
            var list = _dbContext.sources.OrderBy(t => t.src_id).ToList();
            var totalNumberOfRecords = list.Count();
            var results = list.ToList();

            return new PagedResults<source>
            {
                Results = res,
                PageNumber = 0,
                PageSize = 0,
                TotalNumberOfPages = 0,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<source> GetAllPage(int pageNumber, int pageSize)
        {
            List<source> res = new List<source>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.sources.OrderBy(t => t.src_id).Skip(skipAmount).Take(pageSize);
            var totalNumberOfRecords = list.Count();

            var results = list.ToList();
            foreach (source i in results)
            {
                var productview = _mapper.Map<source>(i);
                res.Add(productview);
            }
            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<source>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<source> GetSources(string search_name)
        {

            var list = _dbContext.sources.Where(p => p.src_name.Contains(search_name)).ToList();
            var totalNumberOfRecords = list.Count();
            var results = list.ToList();

            return new PagedResults<source>
            {
                Results = results,
                PageNumber = 0,
                PageSize = 0,
                TotalNumberOfPages = 0,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
    }
}