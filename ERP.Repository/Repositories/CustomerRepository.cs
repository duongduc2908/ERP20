using AutoMapper;
using ERP.Common.Constants.Enums;
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
    public class CustomerRepository : GenericRepository<customer>, ICustomerRepository
    {
        private readonly IMapper _mapper;
        public CustomerRepository(ERPDbContext dbContext,IMapper _mapper) : base(dbContext)
        {
            this._mapper = _mapper;
        }
        public PagedResults<customerviewmodel> GetAllPage(int pageNumber, int pageSize)
        {
            List<customerviewmodel> res = new List<customerviewmodel>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.customers.OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = list.Count();

            var results = list.ToList();
            foreach(customer i in results)
            {
                var customerview = _mapper.Map<customerviewmodel>(i);

                if(i.cu_type == 0)
                {
                    customerview.cu_type_name = EnumCustomer.cu_type_0;
                }
                if (i.cu_type == 1)
                {
                    customerview.cu_type_name = EnumCustomer.cu_type_1;
                }
                var group_role = _dbContext.group_role.FirstOrDefault(x => x.gr_id == i.customer_group_id);
                var sources = _dbContext.sources.FirstOrDefault(x => x.src_id == i.source_id);
                customerview.customer_group_name = group_role.gr_name;
                customerview.source_name = sources.src_name;

                res.Add(customerview);
            }

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<customerviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };

        }
        public PagedResults<customerviewmodel> GetAllPageBySource(int pageNumber, int pageSize,int source_id)
        {
            List<customerviewmodel> res = new List<customerviewmodel>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.customers.OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = list.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<customerviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };

        }
        public PagedResults<customerviewmodel> GetAllPageByType(int pageNumber, int pageSize, int cu_type)
        {
            List<customerviewmodel> res = new List<customerviewmodel>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.customers.OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = list.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<customerviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };

        }
        public PagedResults<customerviewmodel> GetAllPageByGroup(int pageNumber, int pageSize,int customer_group_id)
        {
            List<customerviewmodel> res = new List<customerviewmodel>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.customers.OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = list.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<customerviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };

        }
        public PagedResults<customer> GetInfor(string search_name)
        {

            var list = _dbContext.customers.Where(c => c.cu_fullname.Contains(search_name) || c.cu_mobile.Contains(search_name)).ToList();
            var totalNumberOfRecords = list.Count();
            var results = list.ToList();

            return new PagedResults<customer>
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