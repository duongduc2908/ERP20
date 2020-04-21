using AutoMapper;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Statistics;
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
        public List<statisticrevenueviewmodel> GetRevenueSource(int staff_id)
        {
            List<statisticrevenueviewmodel> res = new List<statisticrevenueviewmodel>();
            //Kiểm tra admin , user 
            var user = _dbContext.staffs.Find(staff_id);
            if (user == null) { return null; }
            else
            {
                if (user.group_role_id == 1)
                {
                    var lts_cg = (from s in _dbContext.sources
                                  join c in _dbContext.customers on s.src_id equals c.source_id
                                  join co in _dbContext.customer_order on c.cu_id equals co.customer_id
                                  group co by s.src_name into t
                                  select new
                                  {
                                      name = t.Key,
                                      total = t.Sum(i => i.cuo_total_price),
                                  }).ToList();

                    foreach (var i in lts_cg)
                    {
                        statisticrevenueviewmodel add = new statisticrevenueviewmodel();
                        add.cg_name = i.name;
                        add.total_revenue = i.total;
                        res.Add(add);
                    }
                }
                else
                {
                    var lts_cg = (from s in _dbContext.sources
                                  join c in _dbContext.customers on s.src_id equals c.source_id
                                  join co in _dbContext.customer_order on c.cu_id equals co.customer_id
                                  where co.staff_id == user.sta_id
                                  group co by s.src_name into t
                                  select new
                                  {
                                      name = t.Key,
                                      total = t.Sum(i => i.cuo_total_price),
                                  }).ToList();

                    foreach (var i in lts_cg)
                    {
                        statisticrevenueviewmodel add = new statisticrevenueviewmodel();
                        add.cg_name = i.name;
                        add.total_revenue = i.total;
                        res.Add(add);
                    }
                }
            }

            return res;
        }
        public List<dropdown> GetAllDropdown()
        {
            List<dropdown> res = new List<dropdown>();
            List<source> lts_s = _dbContext.sources.ToList();
            foreach(source so in lts_s)
            {
                dropdown dr = new dropdown();
                dr.id = so.src_id;
                dr.name = so.src_name;
                res.Add(dr);
            }
            return res;
        }
    }
}