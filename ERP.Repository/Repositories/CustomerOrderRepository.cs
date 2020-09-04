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
using ERP.Data.ModelsERP.ModelView.Staff;
using ERP.Data.ModelsERP.ModelView.Excutor;
using ERP.Data.ModelsERP.ModelView.CustomerOrder;

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
        public PagedResults<customerorderviewmodel> ResultStatisticsCustomerOrder(int pageNumber, int pageSize, int staff_id, bool month, bool week, bool day,int company_id)
        {
            List<customerorderviewmodel> res = new List<customerorderviewmodel>();
            var skipAmount = pageSize * pageNumber;
            DateTime datetimesearch = new DateTime();
            if (month) datetimesearch = Utilis.GetFirstDayOfMonth(DateTime.Now);
            if (week) datetimesearch = Utilis.GetFirstDayOfWeek(DateTime.Now);
            if (day) datetimesearch = DateTime.Now;

            var list = _dbContext.customer_order.Where(i => (i.cuo_date <= DateTime.Now && i.cuo_date >= datetimesearch && i.staff_id == staff_id) && i.company_id == company_id).OrderBy(t => t.cuo_id).Skip(skipAmount).Take(pageSize);

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
            var i = _dbContext.customer_order.Where(x => x.cuo_id == id).FirstOrDefault();
            if (i != null)
            {
                servicercustomerorderviewmodel add = new servicercustomerorderviewmodel();
                //service time
                //add = _mapper.Map<servicercustomerorderviewmodel>(_dbContext.service_time.Where(x => x.customer_order_id == i.cuo_id).FirstOrDefault());
                //for (int j = 1; j < EnumRepeatType.st_repeat_type.Length + 1; j++)
                //{
                //    if (j == add.st_repeat_type)
                //    {
                //        add.st_repeat_type_name = EnumRepeatType.st_repeat_type[j - 1];
                //    }
                //}
                //customer order 
                add.cuo_address = i.cuo_address;
                add.cuo_code = i.cuo_code;
                add.cuo_date = i.cuo_date;
                add.cuo_id = i.cuo_id;
                add.cuo_color_show = i.cuo_color_show;
                add.cuo_discount = i.cuo_discount;
                //Thong tin khach hang 

                //Lay ra thong tin khach hang 
                #region customer
                customer cu = _dbContext.customers.Where(x => x.cu_id == i.customer_id).FirstOrDefault();
                var customerview = _mapper.Map<customerviewmodel>(cu);
                var sources = _dbContext.sources.Find(cu.source_id);
                var customergroup = _dbContext.customer_group.Find(cu.customer_group_id);
                var curator = _dbContext.staffs.Find(cu.cu_curator_id);
                var staff_cu = _dbContext.staffs.Find(cu.staff_id);
                if (curator != null) customerview.cu_curator_name = curator.sta_fullname;
                if (staff_cu != null) customerview.staff_name = staff_cu.sta_fullname;
                if (sources != null) customerview.source_name = sources.src_name;
                if (customergroup != null) customerview.customer_group_name = customergroup.cg_name;

                for (int j = 1; j < EnumCustomer.cu_type.Length + 1; j++)
                {
                    if (j == cu.cu_type)
                    {
                        customerview.cu_type_name = EnumCustomer.cu_type[j - 1];
                    }
                }
                for (int j = 1; j < EnumCustomer.cu_flag_order.Length + 1; j++)
                {
                    if (j == cu.cu_flag_order)
                    {
                        customerview.cu_flag_order_name = EnumCustomer.cu_flag_order[j - 1];
                    }
                }
                for (int j = 1; j < EnumCustomer.cu_flag_used.Length + 1; j++)
                {
                    if (j == cu.cu_flag_used)
                    {
                        customerview.cu_flag_used_name = EnumCustomer.cu_flag_used[j - 1];
                    }
                }
                for (int j = 1; j < EnumCustomer.cu_status.Length + 1; j++)
                {
                    if (j == cu.cu_status)
                    {
                        customerview.cu_status_name = EnumCustomer.cu_status[j - 1];
                    }
                }
                ship_address exists_address = _dbContext.ship_address.Where(x => x.customer_id == cu.cu_id && x.sha_flag_center == 1).FirstOrDefault();
                if(exists_address != null)
                {
                    customerview.sha_ward_now = exists_address.sha_ward;
                    customerview.sha_province_now = exists_address.sha_province;
                    customerview.sha_district_now = exists_address.sha_district;
                    customerview.sha_geocoding_now = exists_address.sha_geocoding;
                    customerview.sha_detail_now = exists_address.sha_detail;
                    customerview.sha_note_now = exists_address.sha_note;
                }
               
                // lay ra dia chi khach hang 
                var list_address = _dbContext.ship_address.Where(s => s.customer_id == cu.cu_id && s.sha_flag_center == 0).ToList();
                List<shipaddressviewmodel> lst_add = new List<shipaddressviewmodel>();
                foreach (ship_address s in list_address)
                {
                    shipaddressviewmodel add_sp = _mapper.Map<shipaddressviewmodel>(s);
                    add_sp.ward_id = _dbContext.ward.Where(t => t.Name.Contains(s.sha_ward)).FirstOrDefault().Id;
                    add_sp.district_id = _dbContext.district.Where(t => t.Name.Contains(s.sha_district)).FirstOrDefault().Id;
                    add_sp.province_id = _dbContext.province.Where(t => t.Name.Contains(s.sha_province)).FirstOrDefault().Id;
                    lst_add.Add(add_sp);
                }
                customerview.list_ship_address = lst_add;
                // lay ra dia chi khach hang 
                var list_phone = _dbContext.customer_phones.Where(s => s.customer_id == cu.cu_id).ToList();
                List<customer_phoneviewmodel> lst_cp_add = new List<customer_phoneviewmodel>();
                foreach (customer_phone s in list_phone)
                {
                    customer_phoneviewmodel add_cp = _mapper.Map<customer_phoneviewmodel>(s);
                    for (int j = 1; j < EnumCustomerPhone.cp_type.Length + 1; j++)
                    {
                        if (j == s.cp_type)
                        {
                            add_cp.cp_type_name = EnumCustomerPhone.cp_type[j - 1];
                        }
                    }

                    lst_cp_add.Add(add_cp);
                }
                customerview.list_customer_phone = lst_cp_add;
                //lay ra lich su mua cua khach hang
                var list_cuo_service_history = _dbContext.customer_order.Where(s => s.customer_id == cu.cu_id && s.cuo_code.Contains("ORS")).ToList();
                List<customerorderservicehistoryviewmodel> lst_cuo_service_his = new List<customerorderservicehistoryviewmodel>();
                foreach (customer_order s in list_cuo_service_history)
                {
                    customerorderservicehistoryviewmodel add_c = _mapper.Map<customerorderservicehistoryviewmodel>(s);
                    add_c.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                    lst_cuo_service_his.Add(add_c);
                }


                customerview.list_customer_order_service = lst_cuo_service_his;
                //lay ra lich su mua don hang cua khach hang
                var list_cuo_product_history = _dbContext.customer_order.Where(s => s.customer_id == cu.cu_id && !s.cuo_code.Contains("ORS")).ToList();
                List<customerorderproducthistoryviewmodel> lst_cuo_product_his = new List<customerorderproducthistoryviewmodel>();
                foreach (customer_order s in list_cuo_product_history)
                {
                    customerorderproducthistoryviewmodel add_c = _mapper.Map<customerorderproducthistoryviewmodel>(s);
                    add_c.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                    lst_cuo_product_his.Add(add_c);
                }


                customerview.list_customer_order_product = lst_cuo_product_his;
                //lay ra lich su cham soc cua khach hang
                var list_tran_history = _dbContext.transactions.Where(s => s.customer_id == cu.cu_id).ToList();
                List<customertransactionviewmodel> lst_tra_his = new List<customertransactionviewmodel>();
                foreach (transaction s in list_tran_history)
                {
                    customertransactionviewmodel add_t = _mapper.Map<customertransactionviewmodel>(s);
                    add_t.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                    lst_tra_his.Add(add_t);
                }

                customerview.list_transaction = lst_tra_his;
                add.customer = customerview;
                #endregion
                add.cu_fullname = customerview.cu_fullname;
                var mobile = _dbContext.customer_phones.Where(x => x.customer_id == i.customer_id && x.cp_type == 1).FirstOrDefault();
                if (mobile != null) add.cu_mobile = mobile.cp_phone_number;
                //thong tin danh sách service
                #region list_service
                //var lts_service = (from ex in _dbContext.order_service
                //                   join od in _dbContext.services on ex.service_id equals od.se_id
                //                   select new
                //                   {
                //                       od.se_id
                //                   }).ToList();
                //List<serviceviewmodel> lts_add_se = new List<serviceviewmodel>();
                //foreach (var se in lts_service)
                //{
                //    service s = _dbContext.services.Find(se.se_id);
                //    serviceviewmodel serviceview = _mapper.Map<serviceviewmodel>(s);


                //    for (int j = 1; j < EnumService.se_type.Length + 1; j++)
                //    {
                //        if (j == s.se_type)
                //        {
                //            serviceview.se_type_name = EnumService.se_type[j - 1];
                //        }
                //    }
                //    var x = _dbContext.service_category.Find(s.service_category_id);
                //    if (x != null) serviceview.service_category_name = x.sc_name;


                //    lts_add_se.Add(serviceview);
                //}
                //add.list_service = lts_add_se;
                #endregion
                //Thong tin list executor
                #region lts _ executor

                //List<executorviewmodel> lts_add_exe = new List<executorviewmodel>();
                //List<executor> lts_exe = _dbContext.executors.Where(x => x.customer_order_id == i.cuo_id).ToList();
                //foreach (executor e in lts_exe)
                //{
                //    executorviewmodel add_ex = new executorviewmodel();
                //    add_ex = _mapper.Map<executorviewmodel>(e);
                //    var staff_name = _dbContext.staffs.Find(e.staff_id);
                //    if (staff_name != null) add_ex.staff_name = staff_name.sta_fullname;
                //    for (int j = 1; j < EnumExecutor.exe_status.Length + 1; j++)
                //    {
                //        if (j == e.exe_status)
                //        {
                //            add_ex.exe_status_name = EnumExecutor.exe_status[j - 1];
                //        }
                //    }
                //    if (add_ex.exe_flag_overtime == true) add_ex.exe_flag_overtime_name = "Có";
                //    if (add_ex.exe_flag_overtime == false) add_ex.exe_flag_overtime_name = "Không";
                //    lts_add_exe.Add(add_ex);
                //}
                //add.list_executor = lts_add_exe;
                #endregion
                res.customer = customerview;
                //Lay ra danh sach san phan dat hang 
                var lst_order_product = _dbContext.order_product.Where(x => x.customer_order_id == i.cuo_id).ToList();
                res.list_product = new List<productorderviewmodel>();
                foreach (order_product op in lst_order_product)
                {
                    var x = _mapper.Map<productorderviewmodel>(op);
                    var product = _dbContext.products.Where(t => t.pu_id == op.product_id).FirstOrDefault();
                    x.pu_unit = product.pu_unit;
                    if(x.pu_unit != null) x.pu_unit_name = EnumProduct.pu_unit[(int)x.pu_unit - 1];
                    x.pu_id = product.pu_id;
                    x.pu_name = product.pu_name;
                    x.pu_sale_price = product.pu_sale_price;
                    x.max_quantity = product.pu_quantity;
                    res.list_product.Add(x);
                }
                res.cuo_ship_tax = i.cuo_ship_tax;
                res.cuo_status = i.cuo_status;
                if(i.cuo_status != null) res.cuo_status_name = EnumCustomerOrder.status[Convert.ToInt32(i.cuo_status) - 1];

                res.cuo_total_price = i.cuo_total_price;
                res.cuo_payment_status = i.cuo_payment_status;
                res.cuo_payment_type = i.cuo_payment_type;
                res.cuo_discount = i.cuo_discount;
                if(i.cuo_address != null) res.cuo_address = i.cuo_address;



            }
            return res;
        }
        public servicercustomerorderviewmodel GetAllOrderServiceById(int id)
        {
            customer_order i = _dbContext.customer_order.Find(id);
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
            //customer order 
            add.cuo_address = i.cuo_address;
            add.cuo_code = i.cuo_code;
            add.cuo_date = i.cuo_date;
            add.cuo_id = i.cuo_id;
            add.cuo_color_show = i.cuo_color_show;
            add.cuo_discount = i.cuo_discount;
            //Thong tin khach hang 

            //Lay ra thong tin khach hang 
            #region customer
            customer cu = _dbContext.customers.Where(x => x.cu_id == i.customer_id).FirstOrDefault();
            var customerview = _mapper.Map<customerviewmodel>(cu);
            var sources = _dbContext.sources.Find(cu.source_id);
            var customergroup = _dbContext.customer_group.Find(cu.customer_group_id);
            var curator = _dbContext.staffs.Find(cu.cu_curator_id);
            var staff_cu = _dbContext.staffs.Find(cu.staff_id);
            if (curator != null) customerview.cu_curator_name = curator.sta_fullname;
            if (staff_cu != null) customerview.staff_name = staff_cu.sta_fullname;
            if (sources != null) customerview.source_name = sources.src_name;
            if (customergroup != null) customerview.customer_group_name = customergroup.cg_name;

            for (int j = 1; j < EnumCustomer.cu_type.Length + 1; j++)
            {
                if (j == cu.cu_type)
                {
                    customerview.cu_type_name = EnumCustomer.cu_type[j - 1];
                }
            }
            for (int j = 1; j < EnumCustomer.cu_flag_order.Length + 1; j++)
            {
                if (j == cu.cu_flag_order)
                {
                    customerview.cu_flag_order_name = EnumCustomer.cu_flag_order[j - 1];
                }
            }
            for (int j = 1; j < EnumCustomer.cu_flag_used.Length + 1; j++)
            {
                if (j == cu.cu_flag_used)
                {
                    customerview.cu_flag_used_name = EnumCustomer.cu_flag_used[j - 1];
                }
            }
            for (int j = 1; j < EnumCustomer.cu_status.Length + 1; j++)
            {
                if (j == cu.cu_status)
                {
                    customerview.cu_status_name = EnumCustomer.cu_status[j - 1];
                }
            }
            ship_address exists_address = _dbContext.ship_address.Where(x => x.customer_id == cu.cu_id && x.sha_flag_center == 1).FirstOrDefault();

            customerview.sha_ward_now = exists_address.sha_ward;
            customerview.sha_province_now = exists_address.sha_province;
            customerview.sha_district_now = exists_address.sha_district;
            customerview.sha_geocoding_now = exists_address.sha_geocoding;
            customerview.sha_detail_now = exists_address.sha_detail;
            customerview.sha_note_now = exists_address.sha_note;
            // lay ra dia chi khach hang 
            var list_address = _dbContext.ship_address.Where(s => s.customer_id == cu.cu_id && s.sha_flag_center == 0).ToList();
            List<shipaddressviewmodel> lst_add = new List<shipaddressviewmodel>();
            foreach (ship_address s in list_address)
            {
                shipaddressviewmodel add_sp = _mapper.Map<shipaddressviewmodel>(s);
                add_sp.ward_id = _dbContext.ward.Where(t => t.Name.Contains(s.sha_ward)).FirstOrDefault().Id;
                add_sp.district_id = _dbContext.district.Where(t => t.Name.Contains(s.sha_district)).FirstOrDefault().Id;
                add_sp.province_id = _dbContext.province.Where(t => t.Name.Contains(s.sha_province)).FirstOrDefault().Id;
                lst_add.Add(add_sp);
            }
            customerview.list_ship_address = lst_add;
            // lay ra dia chi khach hang 
            var list_phone = _dbContext.customer_phones.Where(s => s.customer_id == cu.cu_id).ToList();
            List<customer_phoneviewmodel> lst_cp_add = new List<customer_phoneviewmodel>();
            foreach (customer_phone s in list_phone)
            {
                customer_phoneviewmodel add_cp = _mapper.Map<customer_phoneviewmodel>(s);
                for (int j = 1; j < EnumCustomerPhone.cp_type.Length + 1; j++)
                {
                    if (j == s.cp_type)
                    {
                        add_cp.cp_type_name = EnumCustomerPhone.cp_type[j - 1];
                    }
                }

                lst_cp_add.Add(add_cp);
            }
            customerview.list_customer_phone = lst_cp_add;
            //lay ra lich su mua cua khach hang
            var list_cuo_service_history = _dbContext.customer_order.Where(s => s.customer_id == cu.cu_id && s.cuo_code.Contains("ORS")).ToList();
            List<customerorderservicehistoryviewmodel> lst_cuo_service_his = new List<customerorderservicehistoryviewmodel>();
            foreach (customer_order s in list_cuo_service_history)
            {
                customerorderservicehistoryviewmodel add_c = _mapper.Map<customerorderservicehistoryviewmodel>(s);
                add_c.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                lst_cuo_service_his.Add(add_c);
            }


            customerview.list_customer_order_service = lst_cuo_service_his;
            //lay ra lich su mua don hang cua khach hang
            var list_cuo_product_history = _dbContext.customer_order.Where(s => s.customer_id == cu.cu_id &&!s.cuo_code.Contains("ORS")).ToList();
            List<customerorderproducthistoryviewmodel> lst_cuo_product_his = new List<customerorderproducthistoryviewmodel>();
            foreach (customer_order s in list_cuo_product_history)
            {
                customerorderproducthistoryviewmodel add_c = _mapper.Map<customerorderproducthistoryviewmodel>(s);
                add_c.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                lst_cuo_product_his.Add(add_c);
            }


            customerview.list_customer_order_product = lst_cuo_product_his;
            //lay ra lich su cham soc cua khach hang
            var list_tran_history = _dbContext.transactions.Where(s => s.customer_id == cu.cu_id).ToList();
            List<customertransactionviewmodel> lst_tra_his = new List<customertransactionviewmodel>();
            foreach (transaction s in list_tran_history)
            {
                customertransactionviewmodel add_t = _mapper.Map<customertransactionviewmodel>(s);
                add_t.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                lst_tra_his.Add(add_t);
            }

            customerview.list_transaction = lst_tra_his;
            add.customer = customerview;
            #endregion
            add.cu_fullname = customerview.cu_fullname;
            var mobile = _dbContext.customer_phones.Where(x => x.customer_id == i.customer_id && x.cp_type == 1).FirstOrDefault();
            if (mobile != null) add.cu_mobile = mobile.cp_phone_number;
            //thong tin danh sách service
            #region list_service
            var lts_service = (from ex in _dbContext.order_service
                               join od in _dbContext.services on ex.service_id equals od.se_id
                               where ex.customer_order_id == id
                               group od by od.se_id into g
                               select new
                               {
                                    g.Key
                               }).ToList();
            List<serviceviewmodel> lts_add_se = new List<serviceviewmodel>();
            var list_service_type = _dbContext.service_type.ToList();
            foreach (var se in lts_service)
            {
                service s = _dbContext.services.Find(se.Key);
                serviceviewmodel serviceview = _mapper.Map<serviceviewmodel>(s);


                foreach(service_type st in list_service_type)
                {
                    if (st.styp_id == s.se_type)
                    {
                        serviceview.se_type_name = st.styp_name;
                    }
                }
                var x = _dbContext.service_category.Find(s.service_category_id);
                if (x != null) serviceview.service_category_name = x.sc_name;
                var serviceunit = _dbContext.service_unit.Find(s.se_unit);
                if (serviceunit != null) serviceview.se_unit_name = serviceunit.suni_name;
                var order_service = _dbContext.order_service.Where(t => t.service_id == se.Key).FirstOrDefault();
                if (order_service != null) serviceview.os_quantity = order_service.os_quantity;
                lts_add_se.Add(serviceview);
            }
            add.list_service = lts_add_se;
            #endregion
            //Thong tin list executor
            #region lts_executor

            List<executorviewmodel> lts_add_exe = new List<executorviewmodel>();
            List<executor> lts_exe = _dbContext.executors.Where(x => x.customer_order_id == i.cuo_id).ToList();
            foreach (executor e in lts_exe)
            {
                executorviewmodel add_ex = new executorviewmodel();
                add_ex = _mapper.Map<executorviewmodel>(e);
                var staff_name = _dbContext.staffs.Find(e.staff_id);
                if (staff_name != null) add_ex.staff_name = staff_name.sta_fullname;
                for (int j = 1; j < EnumExecutor.exe_status.Length + 1; j++)
                {
                    if (j == e.exe_status)
                    {
                        add_ex.exe_status_name = EnumExecutor.exe_status[j - 1];
                    }
                }
                if (add_ex.exe_flag_overtime == true) add_ex.exe_flag_overtime_name = "Có";
                if (add_ex.exe_flag_overtime == false) add_ex.exe_flag_overtime_name = "Không";
                lts_add_exe.Add(add_ex);
            }
            add.list_executor = lts_add_exe;
            #endregion
            return add;
        }
        public PagedResults<customerorderviewmodel> GetAllSearch(int pageNumber, int pageSize, int? payment_type_id,int? cuo_status, DateTime? start_date, DateTime? end_date, string code, int company_id)
        {
            if (code != null) code = code.Trim().ToLower();
            List<customerorderviewmodel> res = new List<customerorderviewmodel>();

            var skipAmount = pageSize * pageNumber;

            List<customer_order> list = new List<customer_order>();
            if(code == null)
            {
                list = _dbContext.customer_order.Where(x => !x.cuo_code.Contains("ORS") && x.company_id == company_id).ToList();
            }
            else
            {
                list = _dbContext.customer_order.Where(x => (x.cuo_code.ToLower().Contains(code) && !x.cuo_code.Contains("ORS")) && x.company_id == company_id).ToList();
            }
            if(payment_type_id !=null)
            {
                list = list.Where(x => x.cuo_payment_type == payment_type_id).ToList();
            }
            if (cuo_status != null)
            {
                list = list.Where(x => x.cuo_status == cuo_status).ToList();
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
        public PagedResults<servicercustomerorderviewmodel> GetAllSearchCustomerOrderService(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name, int company_id)
        {
            if (search_name != null) search_name = search_name.Trim().ToLower();
            List<servicercustomerorderviewmodel> res = new List<servicercustomerorderviewmodel>();

            var skipAmount = pageSize * pageNumber;
            var lts_cuo_cu = (from cuo in _dbContext.customer_order
                              join cu in _dbContext.customers on cuo.customer_id equals cu.cu_id
                              where cuo.cuo_code.Contains("ORS") && cuo.company_id == company_id
                              select new
                              {
                                  cuo,cu
                              }).ToList();

            if (search_name != null)
            {
                if (CheckNumber.IsNumber(search_name))
                {
                    lts_cuo_cu = (from cuo in _dbContext.customer_order
                                      join cu in _dbContext.customers on cuo.customer_id equals cu.cu_id
                                      join cu_p in _dbContext.customer_phones on cuo.customer_id equals cu_p.customer_id
                                      where cuo.cuo_code.Contains("ORS") && cuo.company_id == company_id && cu_p.cp_phone_number.Contains(search_name)
                                      select new
                                      {
                                          cuo,cu
                                      }).ToList();
                }
                else lts_cuo_cu = lts_cuo_cu.Where(x => (x.cu.cu_fullname.ToLower().Contains(search_name) || x.cuo.cuo_code.ToLower().Contains(search_name))).ToList();
            }
            if (start_date != null)
            {
                lts_cuo_cu = lts_cuo_cu.Where(x => x.cuo.cuo_date >= start_date).ToList();
            }
            if (end_date != null)
            {
                end_date = end_date.Value.AddDays(1);
                lts_cuo_cu = lts_cuo_cu.Where(x => x.cuo.cuo_date <= end_date).ToList();
            }
            var total = lts_cuo_cu.Count();
            var results = lts_cuo_cu.OrderByDescending(t => t.cuo.cuo_id).Skip(skipAmount).Take(pageSize);
            foreach (var i in results)
            {
                servicercustomerorderviewmodel add = new servicercustomerorderviewmodel();
                add.cuo_address = i.cuo.cuo_address;
                add.cuo_code = i.cuo.cuo_code;
                add.cuo_date = i.cuo.cuo_date;
                add.cuo_id = i.cuo.cuo_id;
                add.cuo_color_show = i.cuo.cuo_color_show;
                add.cuo_discount = i.cuo.cuo_discount;
                
                add.cu_fullname =  i.cu.cu_fullname;
                var mobile = _dbContext.customer_phones.Where(x => x.customer_id == i.cuo.customer_id && x.cp_type == 1).FirstOrDefault();
                if (mobile != null) add.cu_mobile = mobile.cp_phone_number;
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
        public List<dropdown_salary> Get_staff_free(work_time_view c, string fullName,int company_id)
        {
            string thu = EnumDateWork.get_str(c.work_time.DayOfWeek.ToString());
            List<staff_time_detail> lst_time = new List<staff_time_detail>();

            #region["Lay danh sach lich lam cua nhan su"]
            var lst = _dbContext.staff_work_times.OrderBy(x => x.staff_id).Where(x=>x.sw_day_flag.Contains(thu)&&x.sw_time_start<=c.start_time&&x.sw_time_end>=c.end_time).ToList();
            for (int i = 0; i < lst.Count; i++)
            {
                staff_time_detail time_dt = new staff_time_detail();
                time_dt.staff_id = lst[i].staff_id;
                staff_time st = new staff_time();
                st.sw_day_flag = lst[i].sw_day_flag;
                st.sw_time_end = lst[i].sw_time_end;
                st.sw_time_start = lst[i].sw_time_start;
                time_dt.list_time.Add(st);
                for (int j = i + 1; j < lst.Count; j++)
                {
                    if (lst[i].staff_id == lst[j].staff_id)
                    {
                        staff_time st1 = new staff_time();
                        st1.sw_day_flag = lst[j].sw_day_flag;
                        st1.sw_time_end = lst[j].sw_time_end;
                        st1.sw_time_start = lst[j].sw_time_start;
                        time_dt.list_time.Add(st1);
                        i = j;
                        break;
                    }
                }
                lst_time.Add(time_dt);
            }
            #endregion

            
            #region["Lay danh sach nhan su ban"]
            var list_busy = _dbContext.executors.Where(x => x.work_time == c.work_time && x.start_time <= c.start_time && x.end_time >= c.end_time).ToList();
            #endregion

            if(list_busy.Count!=0)
            {
                foreach(executor st in list_busy)
                {
                    var test = lst.Where(x => x.staff_id == st.staff_id).FirstOrDefault();
                    if (test != null)
                    {
                        lst.Remove(test);
                    }
                }
            }
            
            

            

            List<dropdown_salary> res = new List<dropdown_salary>();
            List<staff> list_staff_free = new List<staff>();
            foreach(staff_work_time st in lst)
            {
                var free = _dbContext.staffs.Where(x => x.sta_id == st.staff_id && x.sta_working_status == 1 && x.company_id == company_id).FirstOrDefault();
                if(free != null)
                    list_staff_free.Add(free);
            }
            if(fullName !=null)
            {
                list_staff_free = list_staff_free.Where(x => x.sta_fullname.Contains(fullName)).ToList();
            }
            foreach (staff st in list_staff_free)
            {
                dropdown_salary dr = new dropdown_salary();
                dr.id = st.sta_id;
                dr.name = st.sta_fullname;
                dr.salary = st.sta_salary;
                res.Add(dr);
            }
            #region["Lấy danh sách nhân sự đang được phân công làm việc ở giờ hiện tại"]
            if (c.customer_order_id != 0)
            {
                var lst_staff = (from ex in _dbContext.executors
                                 where ex.customer_order_id == c.customer_order_id && ex.work_time == c.work_time && ex.start_time == c.start_time && ex.end_time == c.end_time
                                 select new
                                 {
                                     ex.staff_id
                                 }).ToList();
                foreach (var i in lst_staff)
                {
                    dropdown_salary dr = new dropdown_salary();
                    var t = _dbContext.staffs.Find(i.staff_id);
                    dr.id = t.sta_id;
                    dr.name = t.sta_fullname;
                    dr.salary = t.sta_salary;
                    res.Add(dr);

                }
            }
            if (c.list_staff_id != null)
            {
                foreach (int _id in c.list_staff_id)
                {
                    var test = res.Where(x => x.id == _id).FirstOrDefault();
                    if (test != null)
                    {
                        res.Remove(test);
                    }
                }
            }

            #endregion
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
            for (int i = 1; i < EnumCustomerOrder.status.Length+1; i++)
            {
                dropdown dr = new dropdown();
                dr.id = i;
                dr.name = EnumCustomerOrder.status[i - 1];
                res.Add(dr);
            }
            return res;
        }
        public PagedResults<customerorderproductview> ExportCustomerOrderProduct(int pageNumber, int pageSize, int? payment_type_id, DateTime? start_date, DateTime? end_date, string name,int company_id)
        {
            List<customerorderproductview> res = new List<customerorderproductview>();
            var skipAmount = pageSize * pageNumber;

            List<customer_order> list = new List<customer_order>();
            if (name == null)
            {
                list = _dbContext.customer_order.Where(x => !x.cuo_code.Contains("ORS") && x.company_id == company_id).ToList();
            }
            else
            {
                list = _dbContext.customer_order.Where(x => x.cuo_code.ToLower().Contains(name) && !x.cuo_code.Contains("ORS") && x.company_id == company_id).ToList();
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
            var total = list.Count();
            var results = list.OrderByDescending(t => t.cuo_id).Skip(skipAmount).Take(pageSize);
            foreach (customer_order i in results)
            {
                var orderview = _mapper.Map<customerorderproductview>(i);
                var customer = _dbContext.customers.Find(i.customer_id);
                if (customer != null)
                {
                    orderview.customer_name = customer.cu_fullname;
                    orderview.cu_code = customer.cu_code;
                }
                var lts_product = (from ex in _dbContext.order_product
                                   join od in _dbContext.products on ex.product_id equals od.pu_id
                                   where ex.customer_order_id == i.cuo_id
                                   select new
                                   {
                                       od.pu_id, od.pu_code, od.pu_name, ex.op_quantity, ex.op_discount,
                                       ex.op_total_value,od.pu_sale_price
                                   }).ToList();
                foreach(var pu in lts_product)
                {
                    orderview.pu_code = pu.pu_code;
                    orderview.pu_name = pu.pu_name;
                    orderview.pu_sale_price = pu.pu_sale_price;
                    orderview.op_quantity = pu.op_quantity;
                    orderview.op_discount = pu.op_discount;
                    orderview.op_total_value = pu.op_total_value;
                    
                    res.Add(orderview);
                }

               
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<customerorderproductview>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };
        }

        public List<order_service_view> GetServiceByDay(string role ,int id, DateTime start_date, DateTime to_date)
        {
            List<order_service_view> res = new List<order_service_view>();
            if (role.Contains("Admin"))
            {
                var lts_cg = (from ex in _dbContext.executors
                              join od in _dbContext.order_service on ex.customer_order_id equals od.customer_order_id
                              join sv in _dbContext.services on od.service_id equals sv.se_id
                              join st in _dbContext.staffs on ex.staff_id equals st.sta_id
                              join cuo in _dbContext.customer_order on od.customer_order_id equals cuo.cuo_id
                              where ex.work_time >= start_date.Date && ex.work_time <= to_date.Date
                              orderby ex.work_time
                              select new
                              {
                                  ex.work_time,
                                  ex.start_time,
                                  ex.end_time,
                                  sv.se_name,
                                  st.sta_fullname,
                                  cuo.customer_id
                              }).ToList();
                for (int i = 0; i < lts_cg.Count; i++)
                {
                    orderservice_day oday = new orderservice_day();
                    order_service_view ov = new order_service_view();
                    oday.start_time = lts_cg[i].start_time;
                    oday.end_time = lts_cg[i].end_time;
                    oday.service_name = lts_cg[i].se_name;
                    ov.list_service.Add(oday);
                    for (int j = i + 1; j < lts_cg.Count; j++)
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
                    ov.staff_name = lts_cg[i].sta_fullname;
                    int cu_id = lts_cg[i].customer_id.Value;
                    var cu = _dbContext.customers.FirstOrDefault(x => x.cu_id == cu_id);
                    var phone = _dbContext.customer_phones.FirstOrDefault(x => x.customer_id == cu_id && x.cp_type == 1);
                    ov.customer_name = cu.cu_fullname;
                    ov.cu_phone_number = phone.cp_phone_number;
                    res.Add(ov);
                }
            }
            else {
                var lts_cg = (from ex in _dbContext.executors
                              join od in _dbContext.order_service on ex.customer_order_id equals od.customer_order_id
                              join sv in _dbContext.services on od.service_id equals sv.se_id
                              join st in _dbContext.staffs on ex.staff_id equals st.sta_id
                              join cuo in _dbContext.customer_order on od.customer_order_id equals cuo.cuo_id
                              where ex.staff_id == id && ex.work_time >= start_date.Date && ex.work_time <= to_date.Date
                              orderby ex.work_time
                              select new
                              {
                                  ex.work_time,
                                  ex.start_time,
                                  ex.end_time,
                                  sv.se_name,
                                  st.sta_fullname,
                                  cuo.customer_id
                              }).ToList();
                for (int i = 0; i < lts_cg.Count; i++)
                {
                    orderservice_day oday = new orderservice_day();
                    order_service_view ov = new order_service_view();
                    oday.start_time = lts_cg[i].start_time;
                    oday.end_time = lts_cg[i].end_time;
                    oday.service_name = lts_cg[i].se_name;
                    ov.list_service.Add(oday);
                    for (int j = i + 1; j < lts_cg.Count; j++)
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
                    ov.staff_name = lts_cg[i].sta_fullname;
                    int cu_id = lts_cg[i].customer_id.Value;
                    var cu = _dbContext.customers.FirstOrDefault(x => x.cu_id == cu_id);
                    var phone = _dbContext.customer_phones.FirstOrDefault(x => x.customer_id == cu_id && x.cp_type == 1);
                    ov.customer_name = cu.cu_fullname;
                    ov.cu_phone_number = phone.cp_phone_number;
                    res.Add(ov);
                }
            }
           
            return res;
        }
    }
}