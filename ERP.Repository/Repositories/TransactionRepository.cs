using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView.Transaction;
using AutoMapper;
using ERP.Common.Constants.Enums;
using ERP.Data.ModelsERP.ModelView.Customer;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.ExportDB;

namespace ERP.Repository.Repositories
{
    public class TransactionRepository : GenericRepository<transaction>, ITransactionRepository
    {
        private readonly IMapper _mapper;
        public TransactionRepository(ERPDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._mapper = mapper;
        }
        public PagedResults<transaction> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.transactions.OrderBy(t => t.tra_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.transactions.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<transaction>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public transactionviewmodel GetById(int tra_id)
        {
            transactionviewmodel transactionview = new transactionviewmodel();
            var tran = _dbContext.transactions.Where(x => x.tra_id == tra_id).FirstOrDefault();
            if (tran != null)
            {
                transactionview = _mapper.Map<transactionviewmodel>(tran);
                //Bat theo Enums
                for (int j = 1; j < EnumTransaction.tra_type.Length + 1; j++)
                {
                    if (j == tran.tra_type)
                    {
                        transactionview.tra_type_name = EnumTransaction.tra_type[j - 1];
                    }
                }
                for (int j = 1; j < EnumTransaction.tra_priority.Length + 1; j++)
                {
                    if (j == tran.tra_priority)
                    {
                        transactionview.tra_priority_name = EnumTransaction.tra_priority[j - 1];
                    }
                }
                for (int j = 1; j < EnumTransaction.tra_status.Length + 1; j++)
                {
                    if (j == tran.tra_status)
                    {
                        transactionview.tra_status_name = EnumTransaction.tra_status[j - 1];
                    }
                }
                for (int j = 1; j < EnumTransaction.tra_rate.Length + 1; j++)
                {
                    if (j == tran.tra_rate)
                    {
                        transactionview.tra_rate_name = EnumTransaction.tra_rate[j - 1];
                    }
                }
                //Bat cac truong tra ve id 
                var staff_name = _dbContext.staffs.Where(s => s.sta_id == tran.staff_id).FirstOrDefault();
                if(staff_name != null) transactionview.staff_name = staff_name.sta_fullname;
                var cus_name = _dbContext.customers.Where(s => s.cu_id == tran.customer_id).FirstOrDefault();
                if(cus_name != null) transactionview.customer_name = cus_name.cu_fullname;


                //Lay ra thong tin khach hang 
                //#region customer
                //customer cu = _dbContext.customers.Where(x => x.cu_id == tran.customer_id).FirstOrDefault();
                //var customerview = _mapper.Map<customerviewmodel>(cu);
                //var sources = _dbContext.sources.Find(cu.source_id);
                //var customergroup = _dbContext.customer_group.Find(cu.customer_group_id);
                //var curator = _dbContext.staffs.Find(cu.cu_curator_id);
                //var staff_cu = _dbContext.staffs.Find(cu.staff_id);
                //if (curator != null) customerview.cu_curator_name = curator.sta_fullname;
                //if (staff_cu != null) customerview.staff_name = staff_cu.sta_fullname;
                //if (sources != null) customerview.source_name = sources.src_name;
                //if (customergroup != null) customerview.customer_group_name = customergroup.cg_name;

                //for (int j = 1; j < EnumCustomer.cu_type.Length + 1; j++)
                //{
                //    if (j == cu.cu_type)
                //    {
                //        customerview.cu_type_name = EnumCustomer.cu_type[j - 1];
                //    }
                //}
                //for (int j = 1; j < EnumCustomer.cu_flag_order.Length + 1; j++)
                //{
                //    if (j == cu.cu_flag_order)
                //    {
                //        customerview.cu_flag_order_name = EnumCustomer.cu_flag_order[j - 1];
                //    }
                //}
                //for (int j = 1; j < EnumCustomer.cu_flag_used.Length + 1; j++)
                //{
                //    if (j == cu.cu_flag_used)
                //    {
                //        customerview.cu_flag_used_name = EnumCustomer.cu_flag_used[j - 1];
                //    }
                //}
                //for (int j = 1; j < EnumCustomer.cu_status.Length + 1; j++)
                //{
                //    if (j == cu.cu_status)
                //    {
                //        customerview.cu_status_name = EnumCustomer.cu_status[j - 1];
                //    }
                //}
                //ship_address exists_address = _dbContext.ship_address.Where(x => x.customer_id == cu.cu_id && x.sha_flag_center == 1).FirstOrDefault();

                //customerview.sha_ward_now = exists_address.sha_ward;
                //customerview.sha_province_now = exists_address.sha_province;
                //customerview.sha_district_now = exists_address.sha_district;
                //customerview.sha_geocoding_now = exists_address.sha_geocoding;
                //customerview.sha_detail_now = exists_address.sha_detail;
                //customerview.sha_note_now = exists_address.sha_note;
                //// lay ra dia chi khach hang 
                //var list_address = _dbContext.ship_address.Where(s => s.customer_id == cu.cu_id && s.sha_flag_center == 0).ToList();
                //List<shipaddressviewmodel> lst_add = new List<shipaddressviewmodel>();
                //foreach (ship_address s in list_address)
                //{
                //    shipaddressviewmodel add_sp = _mapper.Map<shipaddressviewmodel>(s);
                //    add_sp.ward_id = _dbContext.ward.Where(t => t.Name.Contains(s.sha_ward)).FirstOrDefault().Id;
                //    add_sp.district_id = _dbContext.district.Where(t => t.Name.Contains(s.sha_district)).FirstOrDefault().Id;
                //    add_sp.province_id = _dbContext.province.Where(t => t.Name.Contains(s.sha_province)).FirstOrDefault().Id;
                //    lst_add.Add(add_sp);
                //}
                //customerview.list_ship_address = lst_add;
                //// lay ra dia chi khach hang 
                //var list_phone = _dbContext.customer_phones.Where(s => s.customer_id == cu.cu_id).ToList();
                //List<customer_phoneviewmodel> lst_cp_add = new List<customer_phoneviewmodel>();
                //foreach (customer_phone s in list_phone)
                //{
                //    customer_phoneviewmodel add_cp = _mapper.Map<customer_phoneviewmodel>(s);
                //    for (int j = 1; j < EnumCustomerPhone.cp_type.Length + 1; j++)
                //    {
                //        if (j == s.cp_type)
                //        {
                //            add_cp.cp_type_name = EnumCustomerPhone.cp_type[j - 1];
                //        }
                //    }

                //    lst_cp_add.Add(add_cp);
                //}
                //customerview.list_customer_phone = lst_cp_add;


                ////lay ra lich su cham soc cua khach hang
                //var list_tran_history = _dbContext.transactions.Where(s => s.customer_id == cu.cu_id).ToList();
                //List<customertransactionviewmodel> lst_tra_his = new List<customertransactionviewmodel>();
                //foreach (transaction s in list_tran_history)
                //{
                //    customertransactionviewmodel add_t = _mapper.Map<customertransactionviewmodel>(s);
                //    add_t.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                //    lst_tra_his.Add(add_t);
                //}

                //customerview.list_transaction = lst_tra_his;
                //transactionview.customer = customerview;
                //#endregion
            }
            //Lay ra  khang hang 
            transactioncustomerviewmodel res = new transactioncustomerviewmodel();

            //lay ra thong tin khach hang 
            var cus = _dbContext.customers.Where(c => c.cu_id == tran.customer_id).FirstOrDefault();
            var customerview = _mapper.Map<transactioncustomerviewmodel>(cus);
            //Lay ra so dien thoai
            var cu_mobile = _dbContext.customer_phones.Where(x => x.customer_id == cus.cu_id && x.cp_type == 1).FirstOrDefault();
            if (cu_mobile != null) customerview.cu_mobile =cu_mobile.cp_phone_number;

            //Lay ra dia chi 
            var cu_address = _dbContext.ship_address.Where(x => x.customer_id == cus.cu_id && x.sha_flag_center == 1).FirstOrDefault();
            if (cu_address != null) customerview.cu_address = String.Concat(cu_address.sha_detail, "-", cu_address.sha_ward, "-", cu_address.sha_district, "-", cu_address.sha_province);
            var sources = _dbContext.sources.FirstOrDefault(x => x.src_id == cus.source_id);
            var customergroup = _dbContext.customer_group.FirstOrDefault(x => x.cg_id == cus.customer_group_id);
            var curator = _dbContext.staffs.Find(customerview.cu_curator_id);
            if (curator != null) customerview.cu_curator_name = curator.sta_fullname;
            if (sources != null) customerview.source_name = sources.src_name;
            if (customergroup != null) customerview.customer_group_name = customergroup.cg_name;
            for (int j = 1; j < EnumCustomer.cu_type.Length + 1; j++)
            {
                if (j == cus.cu_type)
                {
                    customerview.cu_type_name = EnumCustomer.cu_type[j - 1];
                }
            }
            //lay ra lich su dich vu cua khach hang
            #region cusotmer order service
            List<transactionorderserviceviewmodel> list_add_order_service = new List<transactionorderserviceviewmodel>();
            var list_customer_order_service = (from od in _dbContext.customer_order
                                               join ex in _dbContext.executors on od.cuo_id equals ex.customer_order_id
                                               where ex.exe_status == 1 && od.customer_id == cus.cu_id
                                               group od by od.cuo_id into t
                                               select new
                                               {
                                                   t.Key
                                               }).ToList();
            foreach (var cuo in list_customer_order_service)
            {
                var list_order_service = _dbContext.order_service.Where(o => o.customer_order_id == cuo.Key).ToList();
                foreach (order_service ord in list_order_service)
                {
                    var se = _dbContext.services.Find(ord.service_id);
                    if (se != null)
                    {
                        transactionorderserviceviewmodel add = _mapper.Map<transactionorderserviceviewmodel>(se);
                        for (int j = 1; j < EnumService.se_type.Length + 1; j++)
                        {
                            if (j == add.se_type)
                            {
                                add.se_type_name = EnumService.se_type[j - 1];
                            }
                        }
                        var x = _dbContext.service_category.Find(add.service_category_id);
                        if (x != null) add.service_category_name = x.sc_name;
                        var date_use_end = (from od in _dbContext.order_service
                                            join cu in _dbContext.customers on tran.customer_id equals cu.cu_id
                                            join ex in _dbContext.executors on od.customer_order_id equals ex.customer_order_id
                                            where ex.exe_status == 1 && od.service_id == ord.service_id
                                            group ex by ex.work_time into t
                                            orderby t.Key descending
                                            select new
                                            {
                                                t.Key
                                            }).FirstOrDefault();
                        if (date_use_end != null) add.cuo_date = Convert.ToDateTime(date_use_end.Key);
                        list_add_order_service.Add(add);
                    }

                }

            }
            customerview.list_customer_order_service = list_add_order_service;
            #endregion
            //Lay ra lich su mua 
            List<transactionorderproductviewmodel> list_add_order_product = new List<transactionorderproductviewmodel>();
            var list_customer_order_product = _dbContext.customer_order.Where(cuo => cuo.customer_id == tran.customer_id).ToList();
            foreach (customer_order cuo in list_customer_order_product)
            {
                var list_order_product = _dbContext.order_product.Where(o => o.customer_order_id == cuo.cuo_id).ToList();
                foreach (order_product ord in list_order_product)
                {
                    transactionorderproductviewmodel add = _mapper.Map<transactionorderproductviewmodel>(ord);
                    var prodcut_cur = _dbContext.products.Where(pu => pu.pu_id == add.product_id).FirstOrDefault();
                    if (prodcut_cur != null)
                    {
                        add.pu_name = prodcut_cur.pu_name;
                        for (int j = 1; j < EnumProduct.pu_unit.Length + 1; j++)
                        {
                            if (j == prodcut_cur.pu_unit)
                            {
                                add.pu_unit_name = EnumProduct.pu_unit[j - 1];
                            }
                        }
                    }

                    
                    var cu_name = _dbContext.customers.Where(c => c.cu_id == cus.cu_id).FirstOrDefault();
                    if(cu_name != null) add.cu_fullname = cu_name.cu_fullname;

                    for (int j = 1; j < EnumCustomerOrder.status.Length + 1; j++)
                    {
                        if (j == cuo.cuo_status)
                        {
                            add.cuo_status_name = EnumCustomerOrder.status[j - 1];
                        }
                    }

                    add.cuo_address = cuo.cuo_address;
                    add.cuo_date = cuo.cuo_date;
                    add.op_total_value = ord.op_total_value;
                    list_add_order_product.Add(add);
                }

            }
            customerview.list_order_product = list_add_order_product;

            res = customerview;
            transactionview.customer = res;
            return transactionview;
        }
        public List<dropdown> GetTransactionType(int company_id)
        {
            List<dropdown> res = new List<dropdown>();
            var list_type = _dbContext.transaction_deal_type.Where(x => x.company_id == company_id).ToList();
            foreach (var co in list_type)
            {
                dropdown dr = new dropdown();
                dr.id = co.trand_id;
                dr.name = co.trand_name;
                res.Add(dr);
            }
            return res;
        }
        public List<dropdown> GetTransactionPriority(int company_id)
        {
            List<dropdown> res = new List<dropdown>();
            var list_priority = _dbContext.transaction_priority.Where(x => x.company_id == company_id).ToList();
            foreach (var co in list_priority)
            {
                dropdown dr = new dropdown();
                dr.id = co.tpro_id;
                dr.name = co.tpro_name;
                res.Add(dr);
            }
            return res;
        }
        public List<dropdown> GetTransactionStatus(int company_id)
        {
            List<dropdown> res = new List<dropdown>();
            for (int i = 1; i < EnumTransaction.tra_status.Length + 1; i++)
            {
                dropdown dr = new dropdown();
                dr.id = i;
                dr.name = EnumTransaction.tra_status[i - 1];
                res.Add(dr);
            }
            return res;
        }
        public List<dropdown> GetTransactionRate(int company_id)
        {
            List<dropdown> res = new List<dropdown>();
            var list_transaction_evaluate  = _dbContext.transaction_evaluate.Where(x => x.company_id == company_id).ToList();
            foreach (var co in list_transaction_evaluate)
            {
                dropdown dr = new dropdown();
                dr.id = co.teval_id;
                dr.name = co.teval_name;
                res.Add(dr);
            }
            return res;
        }
        public PagedResults<transactionviewmodel> GetAllPageSearch(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name, int company_id, int curr_id)
        {
            if (search_name != null) search_name = search_name.Trim().ToLower();
            List<transactionviewmodel> res = new List<transactionviewmodel>();
            List<transaction> list = new List<transaction>();

            var skipAmount = pageSize * pageNumber;
            //Lấy ra thằng hiện tại đang làm việc 
            var user_curr = _dbContext.staffs.Find(curr_id);
            if (user_curr != null)
            { 
                var role = _dbContext.group_role.Where(x => x.gr_id == user_curr.group_role_id).FirstOrDefault();
                if(role.gr_name.Contains("admin"))
                {
                 
                    if (search_name == null) list = _dbContext.transactions.Where(x => x.company_id == company_id).ToList();
                    else list = _dbContext.transactions.Where(i => (i.tra_title.ToLower().Contains(search_name) || i.tra_result.ToLower().Contains(search_name)) && i.company_id == company_id).ToList();
                    if (start_date != null)
                    {
                        list = list.Where(x => x.tra_datetime >= start_date).ToList();
                    }
                    if (end_date != null)
                    {
                        end_date = end_date.Value.AddDays(1);
                        list = list.Where(x => x.tra_datetime <= end_date).ToList();
                    }
                }
                else
                {

                    if (search_name == null) list = _dbContext.transactions.Where(x => x.company_id == company_id && x.staff_id == curr_id).ToList();
                    else list = _dbContext.transactions.Where(i => (i.tra_title.ToLower().Contains(search_name) || i.tra_result.ToLower().Contains(search_name)) && i.company_id == company_id && i.staff_id==curr_id).ToList();
                    if (start_date != null)
                    {
                        list = list.Where(x => x.tra_datetime >= start_date).ToList();
                    }
                    if (end_date != null)
                    {
                        end_date = end_date.Value.AddDays(1);
                        list = list.Where(x => x.tra_datetime <= end_date).ToList();
                    }
                }
                
            }

            var totalNumberOfRecords = list.Count();
            var results = list.OrderBy(t => t.tra_id).Skip(skipAmount).Take(pageSize);


            foreach (transaction i in results)
            {

                var transactionview = _mapper.Map<transactionviewmodel>(i);
                //Bat theo Enums
                for (int j = 1; j < EnumTransaction.tra_type.Length + 1; j++)
                {
                    if (j == i.tra_type)
                    {
                        transactionview.tra_type_name = EnumTransaction.tra_type[j - 1];
                    }
                }
                for (int j = 1; j < EnumTransaction.tra_priority.Length + 1; j++)
                {
                    if (j == i.tra_priority)
                    {
                        transactionview.tra_priority_name = EnumTransaction.tra_priority[j - 1];
                    }
                }
                for (int j = 1; j < EnumTransaction.tra_status.Length + 1; j++)
                {
                    if (j == i.tra_status)
                    {
                        transactionview.tra_status_name = EnumTransaction.tra_status[j - 1];
                    }
                }
                for (int j = 1; j < EnumTransaction.tra_rate.Length + 1; j++)
                {
                    if (j == i.tra_rate)
                    {
                        transactionview.tra_rate_name = EnumTransaction.tra_rate[j - 1];
                    }
                }
                //Bat cac truong tra ve id 
                transactionview.staff_name = _dbContext.staffs.Where(s => s.sta_id == i.staff_id).FirstOrDefault().sta_fullname;
                transactionview.customer_name = _dbContext.customers.Where(s => s.cu_id == i.customer_id).FirstOrDefault().cu_fullname;

                //Lay ra thong tin khach hang 
                //#region customer
                //customer cu = _dbContext.customers.Where(x => x.cu_id == i.customer_id).FirstOrDefault();
                //var customerview = _mapper.Map<customerviewmodel>(cu);
                //var sources = _dbContext.sources.Find(cu.source_id);
                //var customergroup = _dbContext.customer_group.Find(cu.customer_group_id);
                //var curator = _dbContext.staffs.Find(cu.cu_curator_id);
                //var staff_cu = _dbContext.staffs.Find(cu.staff_id);
                //if (curator != null) customerview.cu_curator_name = curator.sta_fullname;
                //if (staff_cu != null) customerview.staff_name = staff_cu.sta_fullname;
                //if (sources != null) customerview.source_name = sources.src_name;
                //if (customergroup != null) customerview.customer_group_name = customergroup.cg_name;

                //for (int j = 1; j < EnumCustomer.cu_type.Length + 1; j++)
                //{
                //    if (j == cu.cu_type)
                //    {
                //        customerview.cu_type_name = EnumCustomer.cu_type[j - 1];
                //    }
                //}
                //for (int j = 1; j < EnumCustomer.cu_flag_order.Length + 1; j++)
                //{
                //    if (j == cu.cu_flag_order)
                //    {
                //        customerview.cu_flag_order_name = EnumCustomer.cu_flag_order[j - 1];
                //    }
                //}
                //for (int j = 1; j < EnumCustomer.cu_flag_used.Length + 1; j++)
                //{
                //    if (j == cu.cu_flag_used)
                //    {
                //        customerview.cu_flag_used_name = EnumCustomer.cu_flag_used[j - 1];
                //    }
                //}
                //for (int j = 1; j < EnumCustomer.cu_status.Length + 1; j++)
                //{
                //    if (j == cu.cu_status)
                //    {
                //        customerview.cu_status_name = EnumCustomer.cu_status[j - 1];
                //    }
                //}
                //ship_address exists_address = _dbContext.ship_address.Where(x => x.customer_id == cu.cu_id && x.sha_flag_center == 1).FirstOrDefault();

                //customerview.sha_ward_now = exists_address.sha_ward;
                //customerview.sha_province_now = exists_address.sha_province;
                //customerview.sha_district_now = exists_address.sha_district;
                //customerview.sha_geocoding_now = exists_address.sha_geocoding;
                //customerview.sha_detail_now = exists_address.sha_detail;
                //customerview.sha_note_now = exists_address.sha_note;
                //// lay ra dia chi khach hang 
                //var list_address = _dbContext.ship_address.Where(s => s.customer_id == cu.cu_id && s.sha_flag_center == 0).ToList();
                //List<shipaddressviewmodel> lst_add = new List<shipaddressviewmodel>();
                //foreach (ship_address s in list_address)
                //{
                //    shipaddressviewmodel add_sp = _mapper.Map<shipaddressviewmodel>(s);
                //    add_sp.ward_id = _dbContext.ward.Where(t => t.Name.Contains(s.sha_ward)).FirstOrDefault().Id;
                //    add_sp.district_id = _dbContext.district.Where(t => t.Name.Contains(s.sha_district)).FirstOrDefault().Id;
                //    add_sp.province_id = _dbContext.province.Where(t => t.Name.Contains(s.sha_province)).FirstOrDefault().Id;
                //    lst_add.Add(add_sp);
                //}
                //customerview.list_ship_address = lst_add;
                //// lay ra dia chi khach hang 
                //var list_phone = _dbContext.customer_phones.Where(s => s.customer_id == cu.cu_id).ToList();
                //List<customer_phoneviewmodel> lst_cp_add = new List<customer_phoneviewmodel>();
                //foreach (customer_phone s in list_phone)
                //{
                //    customer_phoneviewmodel add_cp = _mapper.Map<customer_phoneviewmodel>(s);
                //    for (int j = 1; j < EnumCustomerPhone.cp_type.Length + 1; j++)
                //    {
                //        if (j == s.cp_type)
                //        {
                //            add_cp.cp_type_name = EnumCustomerPhone.cp_type[j - 1];
                //        }
                //    }

                //    lst_cp_add.Add(add_cp);
                //}
                //customerview.list_customer_phone = lst_cp_add;


                ////lay ra lich su cham soc cua khach hang
                //var list_tran_history = _dbContext.transactions.Where(s => s.customer_id == cu.cu_id).ToList();
                //List<customertransactionviewmodel> lst_tra_his = new List<customertransactionviewmodel>();
                //foreach (transaction s in list_tran_history)
                //{
                //    customertransactionviewmodel add_t = _mapper.Map<customertransactionviewmodel>(s);
                //    add_t.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                //    lst_tra_his.Add(add_t);
                //}

                //customerview.list_transaction = lst_tra_his;
                //transactionview.customer = customerview;
                //#endregion
                res.Add(transactionview);
            }


            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<transactionviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<transactionview> ExportTransaction(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name, int company_id)
        {
            List<transactionview> res = new List<transactionview>();
            List<transaction> list = new List<transaction>();

            var skipAmount = pageSize * pageNumber;

            if (search_name == null) list = _dbContext.transactions.Where(i => i.company_id == company_id).ToList();
            else list = _dbContext.transactions.Where(i => (i.tra_title.Contains(search_name) || i.tra_result.Contains(search_name)) && i.company_id == company_id).ToList();
            if (start_date != null)
            {
                list = list.Where(x => x.tra_datetime >= start_date).ToList();
            }
            if (end_date != null)
            {
                end_date = end_date.Value.AddDays(1);
                list = list.Where(x => x.tra_datetime <= end_date).ToList();
            }
            var totalNumberOfRecords = _dbContext.transactions.Count();
            var results = list.OrderBy(t => t.tra_id).Skip(skipAmount).Take(pageSize);
            foreach (transaction i in results)
            {
                var transactionview = _mapper.Map<transactionview>(i);
                //Bat theo Enums
                for (int j = 1; j < EnumTransaction.tra_type.Length + 1; j++)
                {
                    if (j == i.tra_type)
                    {
                        transactionview.tra_type_name = EnumTransaction.tra_type[j - 1];
                    }
                }
                for (int j = 1; j < EnumTransaction.tra_priority.Length + 1; j++)
                {
                    if (j == i.tra_priority)
                    {
                        transactionview.tra_priority_name = EnumTransaction.tra_priority[j - 1];
                    }
                }
                for (int j = 1; j < EnumTransaction.tra_status.Length + 1; j++)
                {
                    if (j == i.tra_status)
                    {
                        transactionview.tra_status_name = EnumTransaction.tra_status[j - 1];
                    }
                }
                for (int j = 1; j < EnumTransaction.tra_rate.Length + 1; j++)
                {
                    if (j == i.tra_rate)
                    {
                        transactionview.tra_rate_name = EnumTransaction.tra_rate[j - 1];
                    }
                }
                //Bat cac truong tra ve id 
                transactionview.staff_name = _dbContext.staffs.Where(s => s.sta_id == i.staff_id).FirstOrDefault().sta_fullname;
                transactionview.customer_name = _dbContext.customers.Where(s => s.cu_id == i.customer_id).FirstOrDefault().cu_fullname;

                res.Add(transactionview);
            }
            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);
            return new PagedResults<transactionview>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public List<transactionstatisticrateviewmodel> GetTransactionStatisticRate(int staff_id, int company_id)
        {
            List<transactionstatisticrateviewmodel> res = new List<transactionstatisticrateviewmodel>();
            //Kiểm tra admin , user 
            var user = _dbContext.staffs.Find(staff_id);
            if (user == null) { return null; }
            else
            {
                if (user.group_role_id == 1)
                {
                    var lts_rate = _dbContext.transaction_evaluate.Where(x => x.company_id == company_id).ToList();
                    foreach (var r in lts_rate)
                    {
                        transactionstatisticrateviewmodel add = new transactionstatisticrateviewmodel();
                        add.cg_name =r.teval_name;
                        add.number = _dbContext.transactions.Where(t => t.tra_rate == r.teval_id && t.company_id == company_id).Count();
                        res.Add(add);
                    }
                }
                else
                {
                    var lts_rate = _dbContext.transaction_evaluate.Where(x => x.company_id == company_id).ToList();
                    foreach (var r in lts_rate)
                    {
                        transactionstatisticrateviewmodel add = new transactionstatisticrateviewmodel();
                        add.cg_name = r.teval_name;
                        add.number = _dbContext.transactions.Where(t => t.tra_rate == r.teval_id && t.staff_id == user.sta_id && t.company_id == company_id).Count();
                        res.Add(add);
                    }
                }
            }

            return res;
        }
    }
}