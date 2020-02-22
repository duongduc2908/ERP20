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
            var totalNumberOfRecords = list.Count();

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
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
    }
}