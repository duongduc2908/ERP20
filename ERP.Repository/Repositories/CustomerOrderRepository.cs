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
using ERP.Data.ModelsERP.ModelView.Customer;
using ERP.Data.ModelsERP.ModelView.Statistics;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.Service;
using ERP.Data.ModelsERP.ModelView.OrderService;

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
                for (int j = 1; j < EnumCustomerOrder.status.Length+1; j++)
                {
                    if (j == i.cuo_status)
                    {
                        orderview.cuo_status_name = EnumCustomerOrder.status[j-1];
                    }
                }

                for (int j = 1; j < EnumCustomerOrder.cuo_payment_type.Length+1; j++)
                {
                    if (j == i.cuo_payment_type)
                    {
                        orderview.cuo_payment_type = EnumCustomerOrder.cuo_payment_type[j-1];
                    }
                }
                for (int j = 1; j < EnumCustomerOrder.cuo_payment_status.Length+1; j++)
                {
                    if (j == i.cuo_payment_status)
                    {
                        orderview.cuo_payment_status = EnumCustomerOrder.cuo_payment_status[j-1];
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
            var results = list.ToList();
            foreach (customer_order i in results)
            {
                var orderview = _mapper.Map<customerorderviewmodel>(i);
                for (int j = 1; j < EnumCustomerOrder.status.Length+1; j++)
                {
                    if (j == i.cuo_status)
                    {
                        orderview.cuo_status_name = EnumCustomerOrder.status[j-1];
                    }
                }

                for (int j = 1; j < EnumCustomerOrder.cuo_payment_type.Length+1; j++)
                {
                    if (j == i.cuo_payment_type)
                    {
                        orderview.cuo_payment_type = EnumCustomerOrder.cuo_payment_type[j-1];
                    }
                }
                for (int j = 1; j < EnumCustomerOrder.cuo_payment_status.Length+1; j++)
                {
                    if (j == i.cuo_payment_status)
                    {
                        orderview.cuo_payment_status = EnumCustomerOrder.cuo_payment_status[j-1];
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
                TotalNumberOfRecords = total
            };
        }
        public customerordermodelview GetAllOrderById(int id)
        {
            customerordermodelview res = new customerordermodelview();
            var cuo = _dbContext.customer_order.Where(i => i.cuo_id == id).FirstOrDefault();
            if (cuo != null)
            {
                // la ra thong tin nguoi dung 
                var cu_curr = _dbContext.customers.Where(i => i.cu_id == cuo.customer_id).FirstOrDefault();
               
                var customerview = _mapper.Map<customeraddressviewmodel>(cu_curr);
                var sources = _dbContext.sources.FirstOrDefault(x => x.src_id == cu_curr.source_id);
                var customergroup = _dbContext.customer_group.FirstOrDefault(x => x.cg_id == cu_curr.customer_group_id);
                customerview.source_name = sources.src_name;
                customerview.customer_group_name = customergroup.cg_name;
                var curator = _dbContext.staffs.Find(cu_curr.cu_curator_id);
                if (curator != null) customerview.cu_curator_name = curator.sta_fullname;
                var staff = _dbContext.staffs.Find(cu_curr.staff_id);
                if (staff != null) customerview.staff_name = staff.sta_fullname;
                for (int j = 1; j < EnumCustomer.cu_type.Length+1; j++)
                {
                    if (j == cu_curr.cu_type)
                    {
                        customerview.cu_type_name = EnumCustomer.cu_type[j-1];
                    }
                }
                // lay ra dia chi khach hang 
                var list_address = _dbContext.ship_address.Where(s => s.customer_id == cu_curr.cu_id).ToList();
                List<shipaddressviewmodel> lst_add = new List<shipaddressviewmodel>();
                foreach (ship_address s in list_address)
                {
                    shipaddressviewmodel add = _mapper.Map<shipaddressviewmodel>(s);
                    add.ward_id = _dbContext.ward.Where(t => t.Name.Contains(s.sha_ward)).FirstOrDefault().Id;
                    add.district_id = _dbContext.district.Where(t => t.Name.Contains(s.sha_district)).FirstOrDefault().Id;
                    add.province_id = _dbContext.province.Where(t => t.Name.Contains(s.sha_province)).FirstOrDefault().Id;
                    lst_add.Add(add);
                }
                customerview.list_address = lst_add;
                res.customer = customerview;
                
                //Lay ra danh sach san phan dat hang 
                var lst_order_product = _dbContext.order_product.Where(i => i.customer_order_id == cuo.cuo_id).ToList();
                res.list_product = new List<productorderviewmodel>();
                foreach (order_product i in lst_order_product)
                {
                    var x = _mapper.Map<productorderviewmodel>(i);
                    var product = _dbContext.products.Where(t => t.pu_id == i.product_id).FirstOrDefault();
                    x.pu_unit = product.pu_unit;
                    x.pu_unit_name = EnumProduct.pu_unit[(int)x.pu_unit - 1];
                    x.pu_name = product.pu_name;
                    x.pu_sale_price = product.pu_sale_price;
                    x.max_quantity = product.pu_quantity;
                    res.list_product.Add(x);
                }
                res.cuo_ship_tax = cuo.cuo_ship_tax;
                res.cuo_status = cuo.cuo_status;
                res.cuo_status_name = EnumCustomerOrder.status[Convert.ToInt32(cuo.cuo_status) - 1];
                res.cuo_total_price = cuo.cuo_total_price;
                res.cuo_payment_status = cuo.cuo_payment_status;
                res.cuo_payment_type = cuo.cuo_payment_type;
                res.cuo_discount = cuo.cuo_discount;
                res.cuo_address = cuo.cuo_address;

                
            }
            return res;
        }
        public PagedResults<customerorderviewmodel> GetAllSearch(int pageNumber, int pageSize, int? payment_type_id, DateTime? start_date, DateTime? end_date, string code)
        {
            List<customerorderviewmodel> res = new List<customerorderviewmodel>();

            var skipAmount = pageSize * pageNumber;

            List<customer_order> list = new List<customer_order>();
            if(code == null)
            {
                list = _dbContext.customer_order.Where(x => x.cuo_code.Contains("ORP")).ToList();
            }
            else
            {
                list = _dbContext.customer_order.Where(x => x.cuo_code.Contains(code) && x.cuo_code.Contains("ORP")).ToList();
            }
            if(payment_type_id !=null)
            {
                list = list.Where(x => x.cuo_payment_type == payment_type_id).ToList();
            }
            if (start_date != null)
            {
                list = list.Where(x => x.cuo_date >= start_date).ToList();
            }
            if (end_date != null)
            {
                end_date = end_date.Value.AddDays(1);
                list = list.Where(x => x.cuo_date <= end_date).ToList();
            }
            var total = list.Count();
            var results = list.OrderByDescending(t => t.cuo_id).Skip(skipAmount).Take(pageSize);
            foreach (customer_order i in results)
            {
                var orderview = _mapper.Map<customerorderviewmodel>(i);
                for (int j = 1; j < EnumCustomerOrder.status.Length + 1; j++)
                {
                    if (j == i.cuo_status)
                    {
                        orderview.cuo_status_name = EnumCustomerOrder.status[j-1];
                    }
                }

                for (int j = 1; j < EnumCustomerOrder.cuo_payment_type.Length+1; j++)
                {
                    if (j == i.cuo_payment_type)
                    {
                        orderview.cuo_payment_type = EnumCustomerOrder.cuo_payment_type[j-1];
                    }
                }
                for (int j = 1; j < EnumCustomerOrder.cuo_payment_status.Length+1; j++)
                {
                    if (j == i.cuo_payment_status)
                    {
                        orderview.cuo_payment_status = EnumCustomerOrder.cuo_payment_status[j-1];
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
                TotalNumberOfRecords = total
            };
        }
        public PagedResults<servicercustomerorderviewmodel> GetAllSearchCustomerOrderService(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name)
        {
            List<servicercustomerorderviewmodel> res = new List<servicercustomerorderviewmodel>();

            var skipAmount = pageSize * pageNumber;

            List<customer_order> list = new List<customer_order>();
            if(search_name == null)
            {
                list = _dbContext.customer_order.Where(x => x.cuo_code.Contains("ORS")).ToList();
            }
            else
            {
                list = _dbContext.customer_order.Where(x => x.cuo_code.Contains(search_name) && x.cuo_code.Contains("ORS")).ToList();
            }
            if (start_date != null)
            {
                list = list.Where(x => x.cuo_date >= start_date).ToList();
            }
            if (end_date != null)
            {
                end_date = end_date.Value.AddDays(1);
                list = list.Where(x => x.cuo_date <= end_date).ToList();
            }
            var total = list.Count();
            var results = list.OrderByDescending(t => t.cuo_id).Skip(skipAmount).Take(pageSize);
            foreach (customer_order i in results)
            {
                servicercustomerorderviewmodel add = new servicercustomerorderviewmodel();
                //service time
                add = _mapper.Map<servicercustomerorderviewmodel>(_dbContext.service_time.Where(x => x.customer_order_id == i.cuo_id).FirstOrDefault());
                for (int j = 1; j < EnumRepeatType.st_repeat_type.Length + 1; j++)
                {
                    if (j == add.st_repeat_type)
                    {
                        add.st_repeat_type_name = EnumRepeatType.st_repeat_type[j - 1];
                    }
                }
                //customer_order
                add.cuo_address = i.cuo_address;
                add.cuo_code = i.cuo_code;
                add.cuo_date = i.cuo_date;
                add.cuo_evaluation = i.cuo_evaluation;
                add.cuo_feedback = i.cuo_feedback;
                add.cuo_id = i.cuo_id;
                add.cuo_infor_time = i.cuo_infor_time;
                //customer
                customer cu = _dbContext.customers.Find(i.customer_id);
                add.cu_fullname = cu.cu_fullname;
                //list_service
                List<serviceinforviewmodel> list_service = new List<serviceinforviewmodel>();
                var lts_or_ser = _dbContext.order_service.Where(s => s.customer_order_id == i.cuo_id).ToList();
                foreach(order_service ors in lts_or_ser)
                {
                    var lts_ser = _dbContext.services.Where(s => s.se_id == ors.service_id).ToList(); ;
                    foreach (service s in lts_ser)
                    {
                        serviceinforviewmodel add_se = _mapper.Map<serviceinforviewmodel>(s);
                        var category = _dbContext.service_category.Find(s.service_category_id);
                        add_se.service_category_name = category.sc_name;
                        list_service.Add(add_se);
                    }
                }
                add.list_service = list_service;
                //list staff
                List<servicestaffviewmodel> list_staff = new List<servicestaffviewmodel>();
                var lts_exe = (from ex in _dbContext.executors where ex.customer_order_id == i.cuo_id
                               group ex by ex.staff_id into t
                              
                           select new
                           {
                               staff_id = t.Key,
                           }).ToList();
                
                foreach (var ex in lts_exe)
                {
                    var lts_sta = _dbContext.staffs.Where(s => s.sta_id == ex.staff_id).ToList(); ;
                    foreach (staff s in lts_sta)
                    {
                        servicestaffviewmodel add_sta = _mapper.Map<servicestaffviewmodel>(s);
                        var deparment = _dbContext.departments.FirstOrDefault(x => x.de_id == s.department_id);
                        var position = _dbContext.positions.FirstOrDefault(x => x.pos_id == s.position_id);
                        add_sta.department_name = deparment.de_name;
                        add_sta.position_name = position.pos_name;
                        list_staff.Add(add_sta);
                    }
                }
                add.list_staff = list_staff;
                //customer 
                customer cu_curr = _dbContext.customers.Find(i.customer_id);
                var customerview = _mapper.Map<servicesearchcustomerviewmodel>(cu_curr);
                var sources = _dbContext.sources.FirstOrDefault(x => x.src_id == cu_curr.source_id);
                var customergroup = _dbContext.customer_group.FirstOrDefault(x => x.cg_id == cu_curr.customer_group_id);
                customerview.source_name = sources.src_name;
                customerview.customer_group_name = customergroup.cg_name;
                var curator = _dbContext.staffs.Find(cu_curr.cu_curator_id);
                var staff_cu = _dbContext.staffs.Find(i.staff_id);
                if (curator != null) customerview.cu_curator_name = curator.sta_fullname;
                if (staff_cu != null) customerview.staff_name = staff_cu.sta_fullname;

                for (int j = 1; j < EnumCustomer.cu_type.Length + 1; j++)
                {
                    if (j == cu_curr.cu_type)
                    {
                        customerview.cu_type_name = EnumCustomer.cu_type[j - 1];
                    }
                }
                // lay ra dia chi khach hang 
                var list_address = _dbContext.ship_address.Where(s => s.customer_id == cu_curr.cu_id).ToList();
                List<shipaddressviewmodel> lst_add = new List<shipaddressviewmodel>();
                foreach (ship_address s in list_address)
                {
                    shipaddressviewmodel add_sha = _mapper.Map<shipaddressviewmodel>(s);
                    add_sha.ward_id = _dbContext.ward.Where(t => t.Name.Contains(s.sha_ward)).FirstOrDefault().Id;
                    add_sha.district_id = _dbContext.district.Where(t => t.Name.Contains(s.sha_district)).FirstOrDefault().Id;
                    add_sha.province_id = _dbContext.province.Where(t => t.Name.Contains(s.sha_province)).FirstOrDefault().Id;
                    lst_add.Add(add_sha);
                }
                customerview.list_address = lst_add;
                add.customer = customerview;
                res.Add(add);


            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<servicercustomerorderviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };
        }

        public statisticsbyrevenueviewmodel ResultStatisticsByRevenue(int staff_id)
        {
            //Lấy ra ngày hiện tại, ngày đầu tuần , ngày đầu năm 
            statisticsbyrevenueviewmodel res = new statisticsbyrevenueviewmodel();
            DateTime curr_day = DateTime.Now;
            DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 1);
            DateTime firstMonth = Utilis.GetFirstDayOfMonth(curr_day);
            DateTime firstWeek = Utilis.GetFirstDayOfWeek(curr_day);
            //Kiểm tra admin , user 
            var user = _dbContext.staffs.Find(staff_id);
            if (user == null) { return null; }
            else
            {
                if(user.group_role_id == 1)
                {
                    res.totalRevenueByMonth = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstMonth).Sum(i => i.cuo_total_price);
                    res.totalRevenueByWeek = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstWeek).Sum(i => i.cuo_total_price);
                    res.totalRevenueByDay = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstDay).Sum(i => i.cuo_total_price);
                    res.totalRevenue = _dbContext.customer_order.Sum(i => i.cuo_total_price);
                }
                else
                {
                    res.totalRevenueByMonth = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstMonth && i.staff_id == staff_id).Sum(i => i.cuo_total_price);
                    res.totalRevenueByWeek = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstWeek && i.staff_id == staff_id).Sum(i => i.cuo_total_price);
                    res.totalRevenueByDay = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstDay && i.staff_id == staff_id).Sum(i => i.cuo_total_price);
                    res.totalRevenue = _dbContext.customer_order.Where(i => i.staff_id == staff_id).Sum(i => i.cuo_total_price);
                }
            }
            return res;
        }
        public List<revenue> ResultStatisticByMonth(int staff_id)
        {
            //Lấy ra ngày hiện tại, ngày đầu tuần , ngày đầu năm 
            List<revenue> res = new List<revenue>();
            int month = DateTime.Now.Month;
            var user = _dbContext.staffs.Find(staff_id);
            if (user == null) { return null; }
            else
            {
                if(user.group_role_id == 1)
                {
                   for(int i = 1; i < month+1; i++)
                   {
                        revenue add = new revenue();
                        add.name = String.Concat("Tháng", Convert.ToString(i));
                        add.total = _dbContext.customer_order.Where(t => t.cuo_date.Value.Month == i).Sum(t => t.cuo_total_price);
                        res.Add(add);
                   }
                }
                else
                {
                    for (int i = 0; i < month + 1; i++)
                    {
                        revenue add = new revenue();
                        add.name = String.Concat("Tháng", Convert.ToString(i));
                        add.total = _dbContext.customer_order.Where(t => t.cuo_date.Value.Month == i && t.staff_id == user.sta_id).Sum(t => t.cuo_total_price);
                        res.Add(add);
                    }
                }
            }
            return res;
        }
        public List<dropdown> Get_staff_free(List<DateTime> results, string fullName)
        {
            List<int?> list_staff_bussy = new List<int?>();
            foreach (DateTime dt in results)
            {
                var add=(from ex in _dbContext.customer_group
                 group ex by ex.staff_id into t
                 select new
                 {
                     staff_id = t.Key,
                 }).ToList();
                foreach(var i in add)
                {
                    list_staff_bussy.Add(i.staff_id);
                }

            }
            list_staff_bussy = list_staff_bussy.Distinct().ToList();
            List<dropdown> res = new List<dropdown>();
            var list_staff = _dbContext.staffs.ToList();
            foreach(var i in list_staff_bussy)
            {
                list_staff.Remove(list_staff.Where(x => x.sta_id == i.GetValueOrDefault()).FirstOrDefault());
            }
            if(fullName!=null)
                list_staff = list_staff.Where(x => x.sta_fullname.Contains(fullName)|| x.sta_code.Contains(fullName)).ToList();
                foreach (staff st in list_staff)
                {
                    dropdown dr = new dropdown();
                    dr.id = st.sta_id;
                    dr.name = st.sta_fullname;
                    res.Add(dr);
                }
            return res;
        }


        public List<dropdown> GetAllPayment()
        {

            List<dropdown> res = new List<dropdown>();
            
            for(int i = 0; i< 3; i++ )
            {
                dropdown dr = new dropdown();
                dr.id = i+1;
                dr.name = EnumCustomerOrder.cuo_payment_type[i];
                res.Add(dr);
            }
            return res;
        }
        public List<dropdown> GetAllStatus()
        {
            List<dropdown> res = new List<dropdown>();
            for (int i = 1; i < 4; i++)
            {
                dropdown dr = new dropdown();
                dr.id = i;
                dr.name = EnumCustomerOrder.status[i - 1];
                res.Add(dr);
            }
            return res;
        }
        public PagedResults<customerorderview> ExportCustomerOrder(int pageNumber, int pageSize, int? payment_type_id, DateTime? start_date, DateTime? end_date, string name)
        {
            List<customerorderview> res = new List<customerorderview>();
            var skipAmount = pageSize * pageNumber;

            List<customer_order> list = new List<customer_order>();
            if (name == null)
            {
                list = _dbContext.customer_order.ToList();
            }
            else
            {
                list = _dbContext.customer_order.Where(x => x.cuo_code.Contains(name)).ToList();
            }
            if (payment_type_id != null)
            {
                list = list.Where(x => x.cuo_payment_type == payment_type_id).ToList();
            }
            if (start_date != null)
            {
                list = list.Where(x => x.cuo_date >= start_date).ToList();
            }
            if (end_date != null)
            {
                end_date = end_date.Value.AddDays(1);
                list = list.Where(x => x.cuo_date <= end_date).ToList();
            }
            var results = list.OrderBy(t => t.cuo_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.customer_order.Count();
            foreach (customer_order i in results)
            {
                var orderview = _mapper.Map<customerorderview>(i);
                var customer = _dbContext.customers.Find(i.customer_id);
                if (customer != null) orderview.customer_name = customer.cu_fullname;
                var staff = _dbContext.staffs.Find(i.staff_id);
                if (staff != null) orderview.staff_name = staff.sta_fullname;
                for (int j = 1; j < EnumCustomerOrder.status.Length + 1; j++)
                {
                    if (j == i.cuo_status)
                    {
                        orderview.cuo_status_name = EnumCustomerOrder.status[j - 1];
                    }
                }

                for (int j = 1; j < EnumCustomerOrder.cuo_payment_type.Length + 1; j++)
                {
                    if (j == i.cuo_payment_type)
                    {
                        orderview.cuo_payment_type_name = EnumCustomerOrder.cuo_payment_type[j - 1];
                    }
                }
                for (int j = 1; j < EnumCustomerOrder.cuo_payment_status.Length + 1; j++)
                {
                    if (j == i.cuo_payment_status)
                    {
                        orderview.cuo_payment_status_name = EnumCustomerOrder.cuo_payment_status[j - 1];
                    }
                }
                res.Add(orderview);
            }

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<customerorderview>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }

        public List<order_service_view> GetServiceByDay(int id, DateTime start_date, DateTime to_date)
        {
            List<order_service_view> res = new List<order_service_view>();
            var test = _dbContext.executors.Where(ex => ex.staff_id == id && ex.work_time >= start_date.Date && ex.work_time <= to_date.Date).ToList();
            var lts_cg = (from ex in _dbContext.executors
                          join od in _dbContext.order_service on ex.customer_order_id equals od.customer_order_id
                          join sv in _dbContext.services on od.service_id equals sv.se_id
                          where ex.staff_id == id && ex.work_time >= start_date.Date && ex.work_time <= to_date.Date
                          orderby ex.work_time 
                          select new
                          {
                              ex.work_time,ex.start_time, ex.end_time, sv.se_name
                          }).ToList();
            for(int i =0; i<lts_cg.Count;i++)
            {
                orderservice_day oday = new orderservice_day();
                order_service_view ov = new order_service_view();
                oday.start_time = lts_cg[i].start_time;
                oday.end_time = lts_cg[i].end_time;
                oday.service_name = lts_cg[i].se_name;
                ov.list_service.Add(oday);
                for (int j = i+1;j<lts_cg.Count; j++)
                {
                    
                    if (lts_cg[i].work_time == lts_cg[j].work_time)
                    {
                        orderservice_day oday2 = new orderservice_day();
                        oday2.start_time = lts_cg[j].start_time;
                        oday2.end_time = lts_cg[j].end_time;
                        oday2.service_name = lts_cg[j].se_name;
                        ov.list_service.Add(oday2);
                        i = j;
                        break;

                    }
                }
                ov.work_time = lts_cg[i].work_time;
                res.Add(ov);
            }
            return res;
        }
    }
}