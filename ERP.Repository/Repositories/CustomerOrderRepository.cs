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
                if(i.cuo_status == 0)
                {
                    orderview.cuo_status = EnumCustomerOrder.status_0;
                }
                if (i.cuo_status == 1)
                {
                    orderview.cuo_status = EnumCustomerOrder.status_1;
                }
                if (i.cuo_status == 2)
                {
                    orderview.cuo_status = EnumCustomerOrder.status_2;
                }

                
                if (i.cuo_payment_type == 1)
                {
                    orderview.cuo_payment_type = EnumCustomerOrder.cuo_payment_type_1;
                }
                if (i.cuo_payment_type == 2)
                {
                    orderview.cuo_payment_type = EnumCustomerOrder.cuo_payment_type_2;
                }
                if (i.cuo_payment_type == 3)
                {
                    orderview.cuo_payment_type = EnumCustomerOrder.cuo_payment_type_3;
                }

                if (i.cuo_payment_status == 1)
                {
                    orderview.cuo_payment_status = EnumCustomerOrder.cuo_payment_status_1;
                }
                if (i.cuo_payment_status == 2)
                {
                    orderview.cuo_payment_status = EnumCustomerOrder.cuo_payment_status_2;
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
    }
}