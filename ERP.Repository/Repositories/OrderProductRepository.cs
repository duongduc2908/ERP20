using AutoMapper;
using ERP.Common.Constants;
using ERP.Common.Constants.Enums;
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
        public PagedResults<statisticsorderviewmodel> ResultStatisticsOrder(int pageNumber, int pageSize, int staff_id, bool month, bool week, bool day, string search_name)
        {
            List<statisticsorderviewmodel> res = new List<statisticsorderviewmodel>(); // trả về res
            List<statisticsorderviewmodel> list = new List<statisticsorderviewmodel>(); //Đưa ra list để sử lý 
            List<customer_order> lts_or = new List<customer_order>();
            var skipAmount = pageSize * pageNumber;
            DateTime datetimesearch = new DateTime();
            if (month) datetimesearch = Utilis.GetFirstDayOfMonth(DateTime.Now);
            if (week) datetimesearch = Utilis.GetFirstDayOfWeek(DateTime.Now);
            if (day) datetimesearch = DateTime.Now;
            //Do something 
            var user_curr = _dbContext.staffs.Find(staff_id);
            if(user_curr == null) { return null; }
            else
            {
                if(user_curr.group_role_id == 1)
                {
                    
                    lts_or = _dbContext.customer_order.Where(t => t.staff_id == staff_id && t.cuo_date >= datetimesearch && t.cuo_date <= DateTime.Now).OrderByDescending(i => i.cuo_date).ToList();
                    foreach(customer_order cuo in lts_or )
                    {
                        //Lay ra danh sach san phan dat hang 
                        var lts_op = _dbContext.order_product.Where(i => i.customer_order_id == cuo.cuo_id).ToList();
                        foreach (order_product i in lts_op)
                        {
                            var add_res = _mapper.Map<statisticsorderviewmodel>(cuo);
                            add_res.op_total_value = i.op_total_value;
                            var pr = _dbContext.products.Find(i.product_id);
                            add_res.pu_id = pr.pu_id;
                            add_res.pu_name = pr.pu_name;
                            add_res.cuo_status_name = EnumCustomerOrder.status[Convert.ToInt32(add_res.cuo_status) - 1];
                            list.Add(add_res);
                        }
                    }
                    if(search_name != null)
                    {
                        res = list.Where(t => t.cuo_code.Contains(search_name) || t.pu_name.Contains(search_name)).OrderByDescending(t => t.pu_id).Skip(skipAmount).Take(pageSize).ToList();
                    }
                    else
                    {
                        res = list.OrderByDescending(t => t.pu_id).Skip(skipAmount).Take(pageSize).ToList();
                    }
                }
                else
                {
                    lts_or = _dbContext.customer_order.Where(t => t.cuo_date >= datetimesearch && t.cuo_date <= DateTime.Now).OrderByDescending(i => i.cuo_date).ToList();
                    foreach (customer_order cuo in lts_or)
                    {
                        //Lay ra danh sach san phan dat hang 
                        var lts_op = _dbContext.order_product.Where(i => i.customer_order_id == cuo.cuo_id).ToList();
                        foreach (order_product i in lts_op)
                        {
                            var add_res = _mapper.Map<statisticsorderviewmodel>(cuo);
                            add_res.op_total_value = i.op_total_value;
                            var pr = _dbContext.products.Find(i.product_id);
                            add_res.pu_id = pr.pu_id;
                            add_res.pu_name = pr.pu_name;
                            add_res.cuo_status_name = EnumCustomerOrder.status[Convert.ToInt32(add_res.cuo_status) - 1];
                            list.Add(add_res);
                        }
                    }
                    if (search_name != null)
                    {
                        res = list.Where(t => t.cuo_code.Contains(search_name) || t.pu_name.Contains(search_name)).OrderByDescending(t => t.pu_id).Skip(skipAmount).Take(pageSize).ToList();
                    }
                    else
                    {
                        res = list.OrderByDescending(t => t.pu_id).Skip(skipAmount).Take(pageSize).ToList();
                    }
                }
            }
            var totalNumberOfRecords = list.Count();
            var mod = totalNumberOfRecords % pageSize;
            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);
            return new PagedResults<statisticsorderviewmodel>
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