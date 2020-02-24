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
            var total = _dbContext.customers.Count();

            var results = list.ToList();
            foreach(customer i in results)
            {
                var customerview = _mapper.Map<customerviewmodel>(i);

                for (int j = 0; j < 2; j++)
                {
                    if (j == i.cu_type)
                    {
                        customerview.cu_type_name = EnumCustomer.cu_type[j];
                    }
                }
                var group_role = _dbContext.group_role.FirstOrDefault(x => x.gr_id == i.customer_group_id);
                var sources = _dbContext.sources.FirstOrDefault(x => x.src_id == i.source_id);
                customerview.customer_group_name = group_role.gr_name;
                customerview.source_name = sources.src_name;

                res.Add(customerview);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

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
            var total = _dbContext.customers.Count();

            var results = list.ToList();

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

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
            var total = _dbContext.customers.Count();

            var results = list.ToList();

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

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
            var total = _dbContext.customers.Count();

            var results = list.ToList();

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<customerviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };

        }
        
        public PagedResults<customerviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name)
        {
            List<customerviewmodel> res = new List<customerviewmodel>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.customers.OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            
            /**24TH**/
            int check_source_id = 0;
            int check_type = 0;
            int check_group_id = 0;
            int check_name = 0;
            if(source_id != null) { check_source_id = 1; }
            if(cu_type != null) { check_type = 1; }
            if(customer_group_id != null) { check_group_id = 1; }
            if(name != null) { check_name = 1; }

                    
            if(check_source_id == 0 && check_type == 0 && check_group_id == 0 && check_name ==1) 
            {
                list = _dbContext.customers.Where(t => t.cu_fullname.Contains(name)).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 0 && check_type == 0 && check_group_id == 1 && check_name ==0) 
            {
                list = _dbContext.customers.Where(t => t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 0 && check_type == 0 && check_group_id == 1 && check_name ==1) 
            {
                list = _dbContext.customers.Where(t => t.cu_fullname.Contains(name) && t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 0 && check_type == 1 && check_group_id == 0 && check_name ==0) 
            {
                list = _dbContext.customers.Where(t => t.cu_type == cu_type).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 0 && check_type == 1 && check_group_id == 0 && check_name ==1) 
            {
                list = _dbContext.customers.Where(t => t.cu_type == cu_type).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 0 && check_type == 1 && check_group_id == 1 && check_name ==0) 
            {
                list = _dbContext.customers.Where(t => t.cu_type == cu_type && t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 0 && check_type == 1 && check_group_id == 1 && check_name ==1) 
            {
                list = _dbContext.customers.Where(t => t.cu_type == cu_type && t.customer_group_id == customer_group_id && t.cu_fullname.Contains(name)).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 1 && check_type == 0 && check_group_id == 0 && check_name ==0) 
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 1 && check_type == 0 && check_group_id == 0 && check_name ==1) 
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_fullname.Contains(name)).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 1 && check_type == 0 && check_group_id == 1 && check_name ==0) 
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 1 && check_type == 0 && check_group_id == 1 && check_name ==1) 
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_fullname.Contains(name) && t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 1 && check_type == 1 && check_group_id == 0 && check_name ==0) 
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_type == cu_type).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 1 && check_type == 1 && check_group_id == 0 && check_name ==1) 
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_type == cu_type).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 1 && check_type == 1 && check_group_id == 1 && check_name ==0)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_type == cu_type && t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 1 && check_type == 1 && check_group_id == 1 && check_name ==1)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_type == cu_type && t.customer_group_id == customer_group_id && t.cu_fullname.Contains(name)).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
                     
            var totalNumberOfRecords = list.Count();
            var total = _dbContext.customers.Count();

            var results = list.ToList();
            foreach(customer i in results)
            {
                var customerview = _mapper.Map<customerviewmodel>(i);
                var sources = _dbContext.sources.FirstOrDefault(x => x.src_id == i.source_id);
                var customergroup = _dbContext.customer_group.FirstOrDefault(x => x.cg_id == i.customer_group_id);
                customerview.source_name = sources.src_name;
                customerview.customer_group_name = customergroup.cg_name;
                for (int j = 0; j < 2; j++)
                {
                    if (j == i.cu_type)
                    {
                        customerview.cu_type_name = EnumCustomer.cu_type[j];
                    }
                }
                

                res.Add(customerview);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

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
        public PagedResults<customerviewmodel> GetInfor(int cu_id)
        {

            List<customerviewmodel> res = new List<customerviewmodel>();
            

            var list = _dbContext.customers.Where(i => i.cu_id == cu_id).ToList();

            var totalNumberOfRecords = list.Count();
            var total = _dbContext.customers.Count();
            var results = list.ToList();
            foreach (customer i in results)
            {
                var customerview = _mapper.Map<customerviewmodel>(i);
                var sources = _dbContext.sources.FirstOrDefault(x => x.src_id == i.source_id);
                var customergroup = _dbContext.customer_group.FirstOrDefault(x => x.cg_id == i.customer_group_id);
                customerview.source_name = sources.src_name;
                customerview.customer_group_name = customergroup.cg_name;
                for (int j = 0; j < 2; j++)
                {
                    if (j == i.cu_type)
                    {
                        customerview.cu_type_name = EnumCustomer.cu_type[j];
                    }
                }


                res.Add(customerview);
            }

            return new PagedResults<customerviewmodel>
            {
                Results = res,
                PageNumber = 0,
                PageSize = 0,
                TotalNumberOfPages = 0,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public List<dropdown> GetAllType()
        {

            List<dropdown> res = new List<dropdown>();

            for (int i = 0; i < 2; i++)
            {
                dropdown pu = new dropdown();
                pu.id = i;
                pu.name = EnumCustomer.cu_type[i];

                res.Add(pu);
            }
            return res;

        }

    }
}