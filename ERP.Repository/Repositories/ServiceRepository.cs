using AutoMapper;
using ERP.Common.Constants.Enums;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Service;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class ServiceRepository : GenericRepository<service>, IServiceRepository
    {
        private readonly IMapper _mapper;
        public ServiceRepository(ERPDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._mapper = mapper;
        }
        public PagedResults<service> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.services.OrderBy(t => t.se_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.services.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<service>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        
        public PagedResults<serviceviewmodel> GetAllPageSearch(int pageNumber, int pageSize, string search_name)
        {
            List<serviceviewmodel> res = new List<serviceviewmodel>();
            var skipAmount = pageSize * pageNumber;
            List<service> lts_se = new List<service>();
            if(search_name != null)
            {
                lts_se = _dbContext.services.Where(t => t.se_name == search_name).OrderByDescending(t => t.se_id).Skip(skipAmount).Take(pageSize).ToList();
            }
            else
            {
                lts_se = _dbContext.services.OrderByDescending(t => t.se_id).Skip(skipAmount).Take(pageSize).ToList();
            }
           

            var totalNumberOfRecords = _dbContext.services.Count();

            foreach(service i in lts_se)
            {
                var serviceview = _mapper.Map<serviceviewmodel>(i);
                var se_cat = _dbContext.service_category.Where(t => t.sc_id == serviceview.service_category_id).FirstOrDefault();
                if (se_cat != null) serviceview.service_category_name = se_cat.sc_name;

                for (int j = 1; j < EnumService.se_type.Length + 1; j++)
                {
                    if (j == i.se_type)
                    {
                        serviceview.se_type_name = EnumService.se_type[j - 1];
                    }
                }
                res.Add(serviceview);
            }

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<serviceviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public List<dropdown> GetType()
        {

            List<dropdown> res = new List<dropdown>();
            for (int i = 1; i < EnumService.se_type.Length+1; i++)
            {
                dropdown pu = new dropdown();
                pu.id = i;
                pu.name = EnumService.se_type[i - 1];

                res.Add(pu);
            }
            return res;
        }
    }
}