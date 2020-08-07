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
        
        public PagedResults<serviceviewmodel> GetAllPageSearch(int pageNumber, int pageSize, string search_name,int company_id )
        {

            if (search_name != null) search_name = search_name.Trim().ToLower();
            List<serviceviewmodel> res = new List<serviceviewmodel>();
            var skipAmount = pageSize * pageNumber;
            List<service> lts_se = new List<service>();
            if(search_name != null)
            {
                lts_se = _dbContext.services.Where(t => (t.se_name.ToLower().Contains(search_name) || t.se_code.ToLower().Contains(search_name)) && t.company_id == company_id).ToList();
            }
            else
            {
                lts_se = _dbContext.services.Where(t => t.company_id == company_id).ToList();
            }
            var total = lts_se.Count();

            var results = lts_se.OrderByDescending(t => t.se_id).Skip(skipAmount).Take(pageSize);
            var list_service_type = _dbContext.service_type.ToList();
            foreach (service i in results)
            {
                serviceviewmodel serviceview = _mapper.Map<serviceviewmodel>(i);

                var servicetype = _dbContext.service_type.Find(i.se_type);
                serviceview.se_type_name = servicetype.styp_name;
                var x = _dbContext.service_category.Find(i.service_category_id);
                if (x != null) serviceview.service_category_name = x.sc_name;
                var serviceunit = _dbContext.service_unit.Find(i.se_unit);
                serviceview.se_unit_name = serviceunit.suni_name;

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
        public PagedResults<serviceinforviewmodel> GetAllPageInforService(int pageNumber, int pageSize, string search_name, int company_id)
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
                list = _dbContext.services.Where(x => x.company_id == company_id).ToList();
            }
            else
            {
                list = _dbContext.services.Where(x => (x.se_code.Contains(search_name) || x.se_name.Contains(search_name)) && x.company_id == company_id).ToList();
            }
           
            var totalNumberOfRecords = list.Count();
            var results = list.OrderByDescending(t => t.se_id).Skip(skipAmount).Take(pageSize);

            //Do sonething 
            foreach(service s in results)
            {
                var add = _mapper.Map<serviceinforviewmodel>(s);
                var category = _dbContext.service_category.Find(s.service_category_id);
                add.service_category_name = category.sc_name;
                var servicetype = _dbContext.service_type.Find(s.se_type);
                add.se_type_name = servicetype.styp_name;
                var serviceunit = _dbContext.service_unit.Find(s.se_unit);
                add.se_unit_name = serviceunit.suni_name;
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

        public List<dropdown> GetType(int company_id)
        {

            List<dropdown> res = new List<dropdown>();
            var list_type = _dbContext.service_type.Where(x => x.company_id == company_id).ToList();
            foreach (var co in list_type)
            {
                dropdown dr = new dropdown();
                dr.id = co.styp_id;
                dr.name = co.styp_name;
                res.Add(dr);
            }
            return res;
        }
        public List<dropdown> GetUnit(int company_id)
        {

            List<dropdown> res = new List<dropdown>();
            var list_type = _dbContext.service_unit.Where(x => x.company_id == company_id).ToList();
            foreach (var co in list_type)
            {
                dropdown dr = new dropdown();
                dr.id = co.suni_id;
                dr.name = co.suni_name;
                res.Add(dr);
            }
            return res;
        }
        public serviceviewmodel GetById(int se_id)
        {
            service i = _dbContext.services.Find(se_id);
            serviceviewmodel serviceview = _mapper.Map<serviceviewmodel>(i);
            var category = _dbContext.service_category.Find(i.service_category_id);
            serviceview.service_category_name = category.sc_name;
            var servicetype = _dbContext.service_type.Find(i.se_type);
            serviceview.se_type_name = servicetype.styp_name;
            var serviceunit = _dbContext.service_unit.Find(i.se_unit);
            serviceview.se_unit_name = serviceunit.suni_name;
            return serviceview;
        }
        public List<dropdown> GetAllDropdown(int company_id)
        {

            List<dropdown> res = new List<dropdown>();
            List<service> lts_se = _dbContext.services.Where(x => x.company_id == company_id).ToList();
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