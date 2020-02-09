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
    public class OrderProductRepository : GenericRepository<order_product>, IOrderProductRepository
    {
        private readonly IMapper _mapper;
        public OrderProductRepository(ERPDbContext dbContext, IMapper _mapper) : base(dbContext)
        {
            this._mapper = _mapper;
        }
        public PagedResults<order_product> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.order_product.OrderBy(t => t.op_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.order_product.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<order_product>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<orderproductviewmodel> GetAllOrderProduct(int customer_order_id)
        {
            List<orderproductviewmodel> res = new List<orderproductviewmodel>();
            var list = _dbContext.order_product.Where(t => t.customer_order_id == customer_order_id).ToList();
            var totalNumberOfRecords = list.Count();

            
            foreach (order_product i in list)
            {
                var orderproductview = _mapper.Map<orderproductviewmodel>(i);
                var product = _dbContext.products.FirstOrDefault(x => x.pu_id == i.product_id);

                orderproductview.pu_buy_price = product.pu_buy_price;
                orderproductview.pu_unit= product.pu_unit;
                orderproductview.pu_name = product.pu_name;
                orderproductview.pu_code = product.pu_code;
                
                res.Add(orderproductview);
            }

            return new PagedResults<orderproductviewmodel>
            {
                Results = res,
                PageNumber = 0,
                PageSize = 0,
                TotalNumberOfPages = 0,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
    }
}