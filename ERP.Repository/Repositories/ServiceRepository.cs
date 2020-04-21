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
                lts_se = _dbContext.services.Where(t => t.se_name.Contains(search_name)).ToList();
            }
            else
            {
                lts_se = _dbContext.services.ToList();
            }
            var total = lts_se.Count();

            var results = lts_se.OrderByDescending(t => t.se_id).Skip(skipAmount).Take(pageSize);
            foreach (service i in results)
            {
                serviceviewmodel serviceview = _mapper.Map<serviceviewmodel>(i);
                for (int j = 1; j < EnumService.se_type.Length + 1; j++)
                {
                    if (j == i.se_type)
                    {
                        serviceview.se_type_name = EnumService.se_type[j - 1];
                    }
                }
                var x = _dbContext.service_category.Find(i.service_category_id);
                if (x != null) serviceview.service_category_name = x.sc_name;


                res.Add(serviceview);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<serviceviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };
        }
        public PagedResults<serviceinforviewmodel> GetAllPageInforService(int pageNumber, int pageSize, string search_name)
        {
            if (search_name != null)
            {
                search_name = search_name.Trim();
            }
            List<serviceinforviewmodel> res = new List<serviceinforviewmodel>();
            var skipAmount = pageSize * pageNumber;
            List<service> list = new List<service>();
            if (search_name == null)
            {
                list = _dbContext.services.ToList();
            }
            else
            {
                list = _dbContext.services.Where(x => x.se_code.Contains(search_name) || x.se_name.Contains(search_name)).ToList();
            }
           
            var totalNumberOfRecords = list.Count();
            var results = list.OrderByDescending(t => t.se_id).Skip(skipAmount).Take(pageSize);

            //Do sonething 
            foreach(service s in results)
            {
                var add = _mapper.Map<serviceinforviewmodel>(s);
                var category = _dbContext.service_category.Find(s.service_category_id);
                add.service_category_name = category.sc_name;
                res.Add(add);
            }

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<serviceinforviewmodel>
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
        public serviceviewmodel GetById(int se_id)
        {
            service i = _dbContext.services.Find(se_id);
            serviceviewmodel serviceview = _mapper.Map<serviceviewmodel>(i);
            for (int j = 1; j < EnumService.se_type.Length + 1; j++)
            {
                if (j == i.se_type)
                {
                    serviceview.se_type_name = EnumService.se_type[j - 1];
                }
            }
            var x = _dbContext.service_category.Find(i.service_category_id);
            if (x != null) serviceview.service_category_name = x.sc_name;
            return serviceview;
        }
        public List<dropdown> GetAllDropdown()
        {

            List<dropdown> res = new List<dropdown>();
            List<service> lts_se = _dbContext.services.ToList();
            foreach(service se in lts_se)
            {
                dropdown pu = new dropdown();
                pu.id = se.se_id;
                pu.name = se.se_name;

                res.Add(pu);
            }
            return res;
        }
    }

}