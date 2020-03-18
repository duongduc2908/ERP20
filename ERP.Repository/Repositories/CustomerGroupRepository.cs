﻿using AutoMapper;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.CustomerGroup;
using ERP.Data.ModelsERP.ModelView.Statistics;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class CustomerGroupRepository : GenericRepository<customer_group>, ICustomerGroupRepository
    {
        private readonly IMapper _mapper;
        public CustomerGroupRepository(ERPDbContext dbContext, IMapper _mapper) : base(dbContext)
        {
            this._mapper = _mapper;
        }
        public PagedResults<customergroupviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? cg_id, string name)
        {
            List<customergroupviewmodel> res = new List<customergroupviewmodel>();

            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.customer_group.Where(t => t.cg_id == cg_id && t.cg_name.Contains(name)).OrderBy(t => t.cg_id).Skip(skipAmount).Take(pageSize);
            if (cg_id == null)
            {
                if (name != null)
                {
                    list = _dbContext.customer_group.Where(t => t.cg_name.Contains(name)).OrderBy(t => t.cg_id).Skip(skipAmount).Take(pageSize);
                }
                else
                {
                    list = _dbContext.customer_group.OrderBy(t => t.cg_id).Skip(skipAmount).Take(pageSize);
                }

            }
            if (name == null)
            {
                if (cg_id != null)
                {
                    list = _dbContext.customer_group.Where(t => t.cg_id == cg_id).OrderBy(t => t.cg_id).Skip(skipAmount).Take(pageSize);
                }
                else
                {
                    list = _dbContext.customer_group.OrderBy(t => t.cg_id).Skip(skipAmount).Take(pageSize);
                }
            }
            var total = _dbContext.customer_group.Count();
            var results = list.ToList();
            foreach (customer_group i in results)
            {
                var customer_group_view = _mapper.Map<customergroupviewmodel>(i);
                var staffview = _dbContext.staffs.FirstOrDefault(x => x.sta_id == i.staff_id);

                customer_group_view.staff_name = staffview.sta_fullname;
                
                res.Add(customer_group_view);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<customergroupviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };
        } 
        public List<piechartview> GetPieChart()
        {
            List<piechartview> res = new List<piechartview>();
            var lts_cg = _dbContext.customer_group.OrderBy(i => i.cg_id).ToList();
            foreach(customer_group cg in lts_cg)
            {
                piechartview view = new piechartview();
                view.cg_name = cg.cg_name;
                view.number = _dbContext.customers.Where(i => i.customer_group_id == cg.cg_id).Count();
                res.Add(view);
            }
            return res;
        }
        public bool CheckUniqueName(string name,int id)
        {
            if(name != null)
            {
                name = name.Trim();
            }
            var lts_cg = _dbContext.customer_group.ToList();
            foreach(customer_group cg in lts_cg)
            {
                if(String.Compare(name, cg.cg_name, true) == 0 && cg.cg_id != id) return false;
            }
            return true;
        }

        public List<statisticrevenuecustomergroupviewmodel> GetRevenueCustomerGroup()
        {
            List<statisticrevenuecustomergroupviewmodel> res = new List<statisticrevenuecustomergroupviewmodel>();
            var lts_cg = (from cg in _dbContext.customer_group
                          join c in _dbContext.customers on cg.cg_id equals c.customer_group_id
                          join co in _dbContext.customer_order on c.cu_id equals co.customer_id
                          group co by cg.cg_name into t
                          select new
                          {
                              name = t.Key,
                              total = t.Sum(i => i.cuo_total_price),
                          }).ToList();
           
            foreach(var i in lts_cg)
            {
                statisticrevenuecustomergroupviewmodel add = new statisticrevenuecustomergroupviewmodel();
                add.cg_name = i.name;
                add.total_revenue = i.total;
                res.Add(add);
            }
            return res;
        }

    }
}