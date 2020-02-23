using AutoMapper;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Common.Constants.Enums;
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
    public class CustomerOrderRepository : GenericRepository<customer_order>, ICustomerOrderRepository
    {
        private readonly IMapper _mapper;
        public CustomerOrderRepository(ERPDbContext dbContext, IMapper _mapper) : base(dbContext)
        {
            this._mapper = _mapper;
        }
        public PagedResults<customerorderviewmodel> CreatePagedResults(int pageNumber, int pageSize)
        {
            List<customerorderviewmodel> res = new List<customerorderviewmodel>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.customer_order.OrderBy(t => t.cuo_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.customer_order.Count();

            var results = list.ToList();
            foreach (customer_order i in results)
            {
                var orderview = _mapper.Map<customerorderviewmodel>(i);
                for (int j = 0; j < 3; j++)
                {
                    if (j == i.cuo_status)
                    {
                        orderview.cuo_status = EnumCustomerOrder.status[j];
                    }
                }
                
                for (int j = 0; j < 3; j++)
                {
                    if (j == i.cuo_payment_type)
                    {
                        orderview.cuo_payment_type = EnumCustomerOrder.cuo_payment_type[j];
                    }
                }
                for (int j = 0; j < 2; j++)
                {
                    if (j == i.cuo_payment_status)
                    {
                        orderview.cuo_payment_status = EnumCustomerOrder.cuo_payment_status[j];
                    }
                }
                res.Add(orderview);
            }

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<customerorderviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<customer_order> GetAllOrderById(int id)
        {
            

            var order = _dbContext.customer_order.Where(cuo => cuo.cuo_id == id).ToList();

            var totalNumberOfRecords = _dbContext.customer_order.Count();


            
            var totalPageCount = 0;

            return new PagedResults<customer_order>
            {
                Results = order,
                PageNumber = 0,
                PageSize = 0,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<customerorderviewmodel> GetAllSearch(int pageNumber, int pageSize, int? payment_type_id, string code)
        {
            List<customerorderviewmodel> res = new List<customerorderviewmodel>();

            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.customer_order.Where(t => t.cuo_payment_type == payment_type_id && t.cuo_code.Contains(code)).OrderBy(t => t.cuo_id).Skip(skipAmount).Take(pageSize);
            if (payment_type_id == null)
            {
                if (code != null)
                {
                    list = _dbContext.customer_order.Where(t => t.cuo_code.Contains(code)).OrderBy(t => t.cuo_id).Skip(skipAmount).Take(pageSize);
                }
                else
                {
                    list = _dbContext.customer_order.OrderBy(t => t.cuo_id).Skip(skipAmount).Take(pageSize);
                }

            }
            if (code == null)
            {
                if (payment_type_id != null)
                {
                    list = _dbContext.customer_order.Where(t => t.cuo_payment_type == payment_type_id).OrderBy(t => t.cuo_id).Skip(skipAmount).Take(pageSize);
                }
                else
                {
                    list = _dbContext.customer_order.OrderBy(t => t.cuo_id).Skip(skipAmount).Take(pageSize);
                }
            }

            var total = _dbContext.customer_order.Count();
            var totalNumberOfRecords = list.Count();

            var results = list.ToList();
            foreach (customer_order i in results)
            {
                var orderview = _mapper.Map<customerorderviewmodel>(i);
                for (int j = 0; j < 3; j++)
                {
                    if (j == i.cuo_status)
                    {
                        orderview.cuo_status = EnumCustomerOrder.status[j];
                    }
                }

                for (int j = 0; j < 3; j++)
                {
                    if (j == i.cuo_payment_type)
                    {
                        orderview.cuo_payment_type = EnumCustomerOrder.cuo_payment_type[j];
                    }
                }
                for (int j = 0; j < 2; j++)
                {
                    if (j == i.cuo_payment_status)
                    {
                        orderview.cuo_payment_status = EnumCustomerOrder.cuo_payment_status[j];
                    }
                }
                res.Add(orderview);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<customerorderviewmodel>
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