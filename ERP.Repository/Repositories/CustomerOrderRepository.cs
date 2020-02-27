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
using ERP.Common.Constants;

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
        public PagedResults<customerorderviewmodel> ResultStatisticsCustomerOrder(int pageNumber, int pageSize, int staff_id, bool month, bool week, bool day)
        {
            List<customerorderviewmodel> res = new List<customerorderviewmodel>();
            var skipAmount = pageSize * pageNumber;
            DateTime datetimesearch = new DateTime();
            if (month) datetimesearch = Utilis.GetFirstDayOfMonth(DateTime.Now);
            if (week) datetimesearch = Utilis.GetFirstDayOfWeek(DateTime.Now);
            if (day) datetimesearch = DateTime.Now;

            var list = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= datetimesearch && i.staff_id == staff_id).OrderBy(t => t.cuo_id).Skip(skipAmount).Take(pageSize);

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
        public customerordermodelview GetAllOrderById(int id)
        {
            customerordermodelview res = new customerordermodelview();
            var cuo = _dbContext.customer_order.Where(i => i.cuo_id == id).FirstOrDefault();
            if(cuo != null)
            {
                res.customer = _dbContext.customers.Where(i => i.cu_id == cuo.customer_id).FirstOrDefault();
                var lst_order_product = _dbContext.order_product.Where(i => i.customer_order_id == cuo.cuo_id).ToList();
                res.list_product = new List<productorderviewmodel>();
                foreach (order_product i in lst_order_product)
                {
                    var x = _mapper.Map<productorderviewmodel>(i);
                    x.pu_unit = _dbContext.products.Where(t => t.pu_id == i.product_id).FirstOrDefault().pu_unit;
                    res.list_product.Add(x);
                }
                res.cuo_ship_tax = cuo.cuo_ship_tax;
                res.cuo_status = cuo.cuo_status;
                res.cuo_total_price = cuo.cuo_total_price;
                res.cuo_payment_status = cuo.cuo_payment_status;
                res.cuo_payment_type = cuo.cuo_payment_type;
                res.cuo_discount = cuo.cuo_discount;
            }
            return res;
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

        public int ResultStatisticsByMonth(int staff_id)
        {
            int res = 0;
            DateTime temp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 0, 0, 0);
            DateTime firstMonth = Utilis.GetFirstDayOfMonth(temp);
            
            var user = _dbContext.staffs.Find(staff_id);
            var x = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstMonth).Sum(i => i.cuo_total_price);
            if (user.group_role_id != 1)
            {
                x = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstMonth && i.staff_id == staff_id).Sum(i => i.cuo_total_price);
            }
            if(x != null){ res = (int)x;}
            return res;
        }
        public int ResultStatisticsByWeek(int staff_id)
        {
            int res = 0;
            DateTime temp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 0, 0, 0);
            
            DateTime firstWeek = Utilis.GetFirstDayOfWeek(temp);
            var user = _dbContext.staffs.Find(staff_id);
            var x = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstWeek).Sum(i => i.cuo_total_price);
            if (user.group_role_id != 1)
            {
                x = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstWeek && i.staff_id == staff_id).Sum(i => i.cuo_total_price);
            }
            if (x != null) { res = (int)x; }
            return res;
        }
        public int ResultStatisticsByDay(int staff_id)
        {

            int res = 0;
            DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 0, 0, 0);
            var user = _dbContext.staffs.Find(staff_id);
            var x = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstDay).Sum(i => i.cuo_total_price);
            if (user.group_role_id != 1)
            {
                x = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstDay && i.staff_id == staff_id).Sum(i => i.cuo_total_price);
            }
            if (x != null) { res = (int)x; }
            return res;
        }
    }
}