using AutoMapper;
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
    public class ServiceCategoryRepository : GenericRepository<service_category>, IServiceCategoryRepository
    {
        private readonly IMapper _mapper;
        public ServiceCategoryRepository(ERPDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._mapper = mapper;
        }
        public PagedResults<service_category> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.service_category.OrderBy(t => t.sc_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.service_category.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<service_category>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        
        public List<dropdown> GetAllName()
        {
            List<dropdown> lst_res = new List<dropdown>();
            var list = _dbContext.service_category.ToList();
            foreach (service_category s in list)
            {
                dropdown dr = new dropdown();
                dr.id = s.sc_id;
                dr.name = s.sc_name;
                lst_res.Add(dr);
            }
            return lst_res;
        }


    }
}