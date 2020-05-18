using AutoMapper;
using ERP.Common.Constants.Enums;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Customer;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.Product;
using ERP.Data.ModelsERP.ModelView.Service;
using ERP.Data.ModelsERP.ModelView.Sms;
using ERP.Data.ModelsERP.ModelView.Transaction;
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
            var total = _dbContext.customers.Count();

            var results = list.ToList();
            foreach(customer i in results)
            {
                var customerview = _mapper.Map<customerviewmodel>(i);

                for (int j = 1; j < EnumCustomer.cu_type.Length+1; j++)
                {
                    if (j == i.cu_type)
                    {
                        customerview.cu_type_name = EnumCustomer.cu_type[j-1];
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
                TotalNumberOfRecords = total
            };

        }
        public PagedResults<customerviewmodel> GetAllPageBySource(int pageNumber, int pageSize,int source_id)
        {
            List<customerviewmodel> res = new List<customerviewmodel>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.customers.OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);

           
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
                TotalNumberOfRecords = total
            };

        }
        public bool Check_location(ship_address sa)
        {
            if (sa.sha_province != null)
            {
                if (_dbContext.province.FirstOrDefault(x => x.Name.Equals(sa.sha_province)) == null)
                    return false;
            }
            if (sa.sha_district != null)
            {
                if (_dbContext.district.FirstOrDefault(x => x.Name.Equals(sa.sha_district)) == null)
                    return false;
            }
            if (sa.sha_ward != null)
            {
                if (_dbContext.ward.FirstOrDefault(x => x.Name.Equals(sa.sha_ward)) == null)
                    return false;
            }
            return true;
        }
        public PagedResults<customerviewmodel> GetAllPageByType(int pageNumber, int pageSize, int cu_type)
        {
            List<customerviewmodel> res = new List<customerviewmodel>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.customers.OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);

            
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
                TotalNumberOfRecords = total
            };

        }
        public PagedResults<customerviewmodel> GetAllPageByGroup(int pageNumber, int pageSize,int customer_group_id)
        {
            List<customerviewmodel> res = new List<customerviewmodel>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.customers.OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);

            
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
                TotalNumberOfRecords = total
            };

        }
        
        public PagedResults<customerviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, DateTime? start_date, DateTime? end_date, string name,int company_id)
        {
            if (name != null) name = name.Trim().ToLower();
            List<customer> list;
            List<customerviewmodel> res = new List<customerviewmodel>();
            var skipAmount = pageSize * pageNumber;
            if (name == null)
            {
                list = _dbContext.customers.Where(x => x.company_id == company_id).ToList();
            }
            else list = _dbContext.customers.Where(x => (x.cu_fullname.ToLower().Contains(name) || x.cu_code.ToLower().Contains(name))&& x.company_id == company_id).ToList();
            if (source_id != null)
            {
                list = list.Where(x => x.source_id == source_id).ToList();
            }
            if (cu_type != null)
            {
                list = list.Where(x => x.cu_type == cu_type).ToList();
            }
            if(customer_group_id != null)
            {
                list = list.Where(x => x.customer_group_id == customer_group_id).ToList();
            }
            if (start_date != null)
            {
                list = list.Where(x => x.cu_create_date >= start_date).ToList();
            }
            if (end_date != null)
            {
                end_date = end_date.Value.AddDays(1);
                list = list.Where(x => x.cu_create_date <= end_date).ToList();
            }
            var total = list.Count();

            var results = list.OrderByDescending(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            foreach(customer i in results)
            {
                var customerview = _mapper.Map<customerviewmodel>(i);
                var sources = _dbContext.sources.Find(i.source_id);
                var customergroup = _dbContext.customer_group.Find(i.customer_group_id);
                var curator = _dbContext.staffs.Find(i.cu_curator_id);
                var staff_cu = _dbContext.staffs.Find(i.staff_id);
                if(curator != null) customerview.cu_curator_name = curator.sta_fullname;
                if (staff_cu != null) customerview.staff_name = staff_cu.sta_fullname;
                if (sources != null) customerview.source_name = sources.src_name;
                if (customergroup != null) customerview.customer_group_name = customergroup.cg_name;

                for (int j = 1; j < EnumCustomer.cu_type.Length+1; j++)
                {
                    if (j == i.cu_type)
                    {
                        customerview.cu_type_name = EnumCustomer.cu_type[j-1];
                    }
                }
                for (int j = 1; j < EnumCustomer.cu_flag_order.Length + 1; j++)
                {
                    if (j == i.cu_flag_order)
                    {
                        customerview.cu_flag_order_name = EnumCustomer.cu_flag_order[j - 1];
                    }
                }
                for (int j = 1; j < EnumCustomer.cu_flag_used.Length + 1; j++)
                {
                    if (j == i.cu_flag_used)
                    {
                        customerview.cu_flag_used_name = EnumCustomer.cu_flag_used[j - 1];
                    }
                }
                for (int j = 1; j < EnumCustomer.cu_status.Length + 1; j++)
                {
                    if (j == i.cu_status)
                    {
                        customerview.cu_status_name = EnumCustomer.cu_status[j - 1];
                    }
                }
                ship_address exists_address = _dbContext.ship_address.Where(x => x.customer_id == i.cu_id && x.sha_flag_center == 1).FirstOrDefault();
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
                var list_address = _dbContext.ship_address.Where(s => s.customer_id == i.cu_id && s.sha_flag_center == 0).ToList();
                List<shipaddressviewmodel> lst_add = new List<shipaddressviewmodel>();
                foreach (ship_address s in list_address)
                {
                    shipaddressviewmodel add = _mapper.Map<shipaddressviewmodel>(s);
                    add.ward_id = _dbContext.ward.Where(t => t.Name.Contains(s.sha_ward)).FirstOrDefault().Id;
                    add.district_id = _dbContext.district.Where(t => t.Name.Contains(s.sha_district)).FirstOrDefault().Id;
                    add.province_id = _dbContext.province.Where(t => t.Name.Contains(s.sha_province)).FirstOrDefault().Id;
                    lst_add.Add(add);
                }
                customerview.list_ship_address = lst_add;
                // lay ra dia chi khach hang 
                var list_phone = _dbContext.customer_phones.Where(s => s.customer_id == i.cu_id ).ToList();
                List<customer_phoneviewmodel> lst_cp_add = new List<customer_phoneviewmodel>();
                foreach (customer_phone s in list_phone)
                {
                    customer_phoneviewmodel add = _mapper.Map<customer_phoneviewmodel>(s);
                    for (int j = 1; j < EnumCustomerPhone.cp_type.Length + 1; j++)
                    {
                        if (j == s.cp_type)
                        {
                            add.cp_type_name = EnumCustomerPhone.cp_type[j - 1];
                        }
                    }

                    lst_cp_add.Add(add);
                }
                customerview.list_customer_phone = lst_cp_add;

                //lay ra lich su mua cua khach hang
                #region cusotmer order service
                var list_cuo_service_history = _dbContext.customer_order.Where(s => s.customer_id == i.cu_id && s.cuo_code.Contains("ORS")).ToList();
                List<customerorderservicehistoryviewmodel> lst_cuo_service_his = new List<customerorderservicehistoryviewmodel>();
                foreach (customer_order s in list_cuo_service_history)
                {
                    customerorderservicehistoryviewmodel add = _mapper.Map<customerorderservicehistoryviewmodel>(s);
                    add.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                    #region list_service
                    var lts_service = (from ex in _dbContext.order_service
                                       join od in _dbContext.services on ex.service_id equals od.se_id
                                       join cuo in _dbContext.customer_order on ex.customer_order_id equals cuo.cuo_id
                                       where ex.customer_order_id == s.cuo_id && cuo.customer_id == i.cu_id
                                       group od by od.se_id into t
                                       select new
                                       {
                                           t.Key
                                       }).ToList();
                    List<serviceviewmodel> lts_add_se = new List<serviceviewmodel>();
                    foreach (var se in lts_service)
                    {
                        service ss = _dbContext.services.Find(se.Key);
                        serviceviewmodel serviceview = _mapper.Map<serviceviewmodel>(ss);


                        for (int j = 1; j < EnumService.se_type.Length + 1; j++)
                        {
                            if (j == ss.se_type)
                            {
                                serviceview.se_type_name = EnumService.se_type[j - 1];
                            }
                        }
                        var x = _dbContext.service_category.Find(ss.service_category_id);
                        if (x != null) serviceview.service_category_name = x.sc_name;


                        lts_add_se.Add(serviceview);
                    }
                    add.list_service_history = lts_add_se;
                    #endregion
                    lst_cuo_service_his.Add(add);
                }
                customerview.list_customer_order_service = lst_cuo_service_his;
                #endregion
                //lay ra lich su mua cua khach hang
                #region customer order product
                var list_cuo_product_history = _dbContext.customer_order.Where(s => s.customer_id == i.cu_id && !s.cuo_code.Contains("ORS")).ToList();
                List<customerorderproducthistoryviewmodel> lst_cuo_product_his = new List<customerorderproducthistoryviewmodel>();
                foreach (customer_order s in list_cuo_product_history)
                {
                    customerorderproducthistoryviewmodel add = _mapper.Map<customerorderproducthistoryviewmodel>(s);
                    add.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                    #region list_product
                    var lts_product = (from ex in _dbContext.order_product
                                       join od in _dbContext.products on ex.product_id equals od.pu_id
                                       join cuo in _dbContext.customer_order on ex.customer_order_id equals cuo.cuo_id
                                       where ex.customer_order_id == s.cuo_id && cuo.customer_id == i.cu_id
                                       group od by od.pu_id into t
                                       select new
                                       {
                                           t.Key
                                       }).ToList();
                    List<productviewmodelpluss> lts_add_se = new List<productviewmodelpluss>();
                    foreach (var se in lts_product)
                    {
                        product ss = _dbContext.products.Find(se.Key);
                        productviewmodelpluss productview = _mapper.Map<productviewmodelpluss>(ss);
                        var product_category = _dbContext.product_category.Find(ss.product_category_id);
                        if (product_category != null) productview.product_category_name = product_category.pc_name;
                        for (int j = 1; j < EnumProduct.pu_unit.Length + 1; j++)
                        {
                            if (j == ss.pu_unit)
                            {
                                productview.pu_unit_name = EnumProduct.pu_unit[j - 1];
                            }
                        }

                        lts_add_se.Add(productview);
                    }
                    add.list_product_history = lts_add_se;
                    #endregion
                    lst_cuo_product_his.Add(add);
                }
                customerview.list_customer_order_product = lst_cuo_product_his;
                #endregion
                //lay ra lich su cham soc cua khach hang
                var list_tran_history = _dbContext.transactions.Where(s => s.customer_id == i.cu_id).ToList();
                List<customertransactionviewmodel> lst_tra_his = new List<customertransactionviewmodel>();
                foreach (transaction s in list_tran_history)
                {
                    customertransactionviewmodel add = _mapper.Map<customertransactionviewmodel>(s);
                    add.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                    lst_tra_his.Add(add);
                }

                customerview.list_transaction= lst_tra_his;

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
                TotalNumberOfRecords = total
            };

        }
        public PagedResults<customeraddressviewmodel> GetCustomerByCurator(int pageSize, int pageNumber, int? cu_curator_id, string search_name,int company_id)
        {
            if (search_name != null) search_name = search_name.Trim();
            List<customer> list;
            List<customeraddressviewmodel> res = new List<customeraddressviewmodel>();
            var skipAmount = pageSize * pageNumber;
            if (search_name == null)
            {
                list = _dbContext.customers.Where(x => x.company_id == company_id).ToList();
            }
            else list = _dbContext.customers.Where(x => x.cu_fullname.Contains(search_name) && x.company_id == company_id).ToList();
            if (cu_curator_id != null)
            {
                list = list.Where(x => x.cu_curator_id == cu_curator_id).ToList();
            }
            
            var total = list.Count();

            List<customer> results = list.OrderByDescending(t => t.cu_id).Skip(skipAmount).Take(pageSize).ToList();
            foreach(customer i in results)
            {
                var customerview = _mapper.Map<customeraddressviewmodel>(i);
                var sources = _dbContext.sources.FirstOrDefault(x => x.src_id == i.source_id);
                var customergroup = _dbContext.customer_group.FirstOrDefault(x => x.cg_id == i.customer_group_id);
                if(sources != null )customerview.source_name = sources.src_name;
                if (customergroup != null)  customerview.customer_group_name = customergroup.cg_name;
                var curator = _dbContext.staffs.Find(i.cu_curator_id);
                var staff_cu = _dbContext.staffs.Find(i.staff_id);
                if(curator != null) customerview.cu_curator_name = curator.sta_fullname;
                if (staff_cu != null) customerview.staff_name = staff_cu.sta_fullname;

                for (int j = 1; j < EnumCustomer.cu_type.Length+1; j++)
                {
                    if (j == i.cu_type)
                    {
                        customerview.cu_type_name = EnumCustomer.cu_type[j-1];
                    }
                }
                // lay ra dia chi khach hang 
                //var list_address = _dbContext.ship_address.Where(s => s.customer_id == i.cu_id).ToList();
                //List<shipaddressviewmodel> lst_add = new List<shipaddressviewmodel>();
                //foreach (ship_address s in list_address)
                //{
                //    shipaddressviewmodel add = _mapper.Map<shipaddressviewmodel>(s);
                //    add.ward_id = _dbContext.ward.Where(t => t.Name.Contains(s.sha_ward)).FirstOrDefault().Id;
                //    add.district_id = _dbContext.district.Where(t => t.Name.Contains(s.sha_district)).FirstOrDefault().Id;
                //    add.province_id = _dbContext.province.Where(t => t.Name.Contains(s.sha_province)).FirstOrDefault().Id;
                //    lst_add.Add(add);
                //}
                //customerview.list_address = lst_add;
                

                res.Add(customerview);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<customeraddressviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };

        }
        
        public PagedResults<servicesearchcustomerviewmodel> GetAllPageSearchService(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name, int company_id)
        {
            if (name != null) name = name.Trim();
            List<customer> list;
            List<servicesearchcustomerviewmodel> res = new List<servicesearchcustomerviewmodel>();
            var skipAmount = pageSize * pageNumber;
            if (name == null)
            {
                list = _dbContext.customers.Where(x=> x.company_id == company_id).ToList();
            }
            else list = _dbContext.customers.Where(x => x.cu_fullname.Contains(name)&& x.company_id == company_id).ToList();
            if (source_id != null)
            {
                list = list.Where(x => x.source_id == source_id).ToList();
            }
            if (cu_type != null)
            {
                list = list.Where(x => x.cu_type == cu_type).ToList();
            }
            if (customer_group_id != null)
            {
                list = list.Where(x => x.customer_group_id == customer_group_id).ToList();
            }
            var total = list.Count();

            var results = list.OrderByDescending(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            foreach (customer i in results)
            {
                var customerview = _mapper.Map<servicesearchcustomerviewmodel>(i);
                var sources = _dbContext.sources.FirstOrDefault(x => x.src_id == i.source_id);
                var customergroup = _dbContext.customer_group.FirstOrDefault(x => x.cg_id == i.customer_group_id);
                customerview.source_name = sources.src_name;
                customerview.customer_group_name = customergroup.cg_name;
                var curator = _dbContext.staffs.Find(i.cu_curator_id);
                var staff_cu = _dbContext.staffs.Find(i.staff_id);
                if (curator != null) customerview.cu_curator_name = curator.sta_fullname;
                if (staff_cu != null) customerview.staff_name = staff_cu.sta_fullname;

                for (int j = 1; j < EnumCustomer.cu_type.Length + 1; j++)
                {
                    if (j == i.cu_type)
                    {
                        customerview.cu_type_name = EnumCustomer.cu_type[j - 1];
                    }
                }
                // lay ra dia chi khach hang 
                var list_address = _dbContext.ship_address.Where(s => s.customer_id == i.cu_id).ToList();
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
               

                res.Add(customerview);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<servicesearchcustomerviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };

        }
        public PagedResults<smscustomerviewmodel> GetAllPageSearchSms(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name,int company_id)
        {
            if (name != null) name = name.Trim().ToLower();
            List<customer> list;
            List<smscustomerviewmodel> res = new List<smscustomerviewmodel>();
            var skipAmount = pageSize * pageNumber;
            if (name == null)
            {
                list = _dbContext.customers.Where(x => x.company_id == company_id).ToList();
            }
            else list = _dbContext.customers.Where(x => (x.cu_fullname.ToLower().Contains(name) || x.cu_code.ToLower().Contains(name)) && x.company_id == company_id).ToList();
            if (source_id != null)
            {
                list = list.Where(x => x.source_id == source_id).ToList();
            }
            if (cu_type != null)
            {
                list = list.Where(x => x.cu_type == cu_type).ToList();
            }
            if (customer_group_id != null)
            {
                list = list.Where(x => x.customer_group_id == customer_group_id).ToList();
            }
            var total = list.Count();

            var results = list.OrderByDescending(t => t.cu_id).Skip(skipAmount).Take(pageSize);

            foreach (customer i in results)
            {
                var customerview = _mapper.Map<smscustomerviewmodel>(i);
                var sources = _dbContext.sources.FirstOrDefault(x => x.src_id == i.source_id);
                var customergroup = _dbContext.customer_group.FirstOrDefault(x => x.cg_id == i.customer_group_id);
                customerview.source_name = sources.src_name;
                customerview.customer_group_name = customergroup.cg_name;
                var curator = _dbContext.staffs.Find(i.cu_curator_id);
                for (int j = 1; j < EnumCustomer.cu_type.Length+1; j++)
                {
                    if (j == i.cu_type)
                    {
                        customerview.cu_type_name = EnumCustomer.cu_type[j - 1];
                    }
                }
                res.Add(customerview);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<smscustomerviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };

        }
     
        public customerviewmodel GetInfor(int cu_id)
        {
            customer i = _dbContext.customers.Find(cu_id);
            var customerview = _mapper.Map<customerviewmodel>(i);
            var sources = _dbContext.sources.Find(i.source_id);
            var customergroup = _dbContext.customer_group.Find(i.customer_group_id);
            var curator = _dbContext.staffs.Find(i.cu_curator_id);
            var staff_cu = _dbContext.staffs.Find(i.staff_id);
            if (curator != null) customerview.cu_curator_name = curator.sta_fullname;
            if (staff_cu != null) customerview.staff_name = staff_cu.sta_fullname;
            if (sources != null) customerview.source_name = sources.src_name;
            if (customergroup != null) customerview.customer_group_name = customergroup.cg_name;

            for (int j = 1; j < EnumCustomer.cu_type.Length + 1; j++)
            {
                if (j == i.cu_type)
                {
                    customerview.cu_type_name = EnumCustomer.cu_type[j - 1];
                }
            }
            for (int j = 1; j < EnumCustomer.cu_flag_order.Length + 1; j++)
            {
                if (j == i.cu_flag_order)
                {
                    customerview.cu_flag_order_name = EnumCustomer.cu_flag_order[j - 1];
                }
            }
            for (int j = 1; j < EnumCustomer.cu_flag_used.Length + 1; j++)
            {
                if (j == i.cu_flag_used)
                {
                    customerview.cu_flag_used_name = EnumCustomer.cu_flag_used[j - 1];
                }
            }
            for (int j = 1; j < EnumCustomer.cu_status.Length + 1; j++)
            {
                if (j == i.cu_status)
                {
                    customerview.cu_status_name = EnumCustomer.cu_status[j - 1];
                }
            }
            ship_address exists_address = _dbContext.ship_address.Where(x => x.customer_id == i.cu_id && x.sha_flag_center == 1).FirstOrDefault();
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
            var list_address = _dbContext.ship_address.Where(s => s.customer_id == i.cu_id && s.sha_flag_center == 0).ToList();
            List<shipaddressviewmodel> lst_add = new List<shipaddressviewmodel>();
            foreach (ship_address s in list_address)
            {
                shipaddressviewmodel add = _mapper.Map<shipaddressviewmodel>(s);
                add.ward_id = _dbContext.ward.Where(t => t.Name.Contains(s.sha_ward)).FirstOrDefault().Id;
                add.district_id = _dbContext.district.Where(t => t.Name.Contains(s.sha_district)).FirstOrDefault().Id;
                add.province_id = _dbContext.province.Where(t => t.Name.Contains(s.sha_province)).FirstOrDefault().Id;
                lst_add.Add(add);
            }
            customerview.list_ship_address = lst_add;
            // lay ra dia chi khach hang 
            var list_phone = _dbContext.customer_phones.Where(s => s.customer_id == i.cu_id).ToList();
            List<customer_phoneviewmodel> lst_cp_add = new List<customer_phoneviewmodel>();
            foreach (customer_phone s in list_phone)
            {
                customer_phoneviewmodel add = _mapper.Map<customer_phoneviewmodel>(s);
                for (int j = 1; j < EnumCustomerPhone.cp_type.Length + 1; j++)
                {
                    if (j == s.cp_type)
                    {
                        add.cp_type_name = EnumCustomerPhone.cp_type[j - 1];
                    }
                }

                lst_cp_add.Add(add);
            }
            customerview.list_customer_phone = lst_cp_add;
            //lay ra lich su mua cua khach hang
            #region cusotmer order service
            var list_cuo_service_history = _dbContext.customer_order.Where(s => s.customer_id == i.cu_id && s.cuo_code.Contains("ORS")).ToList();
            List<customerorderservicehistoryviewmodel> lst_cuo_service_his = new List<customerorderservicehistoryviewmodel>();
            foreach (customer_order s in list_cuo_service_history)
            {
                customerorderservicehistoryviewmodel add = _mapper.Map<customerorderservicehistoryviewmodel>(s);
                add.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                #region list_service
                var lts_service = (from ex in _dbContext.order_service
                                   join od in _dbContext.services on ex.service_id equals od.se_id
                                   join cuo in _dbContext.customer_order on ex.customer_order_id equals cuo.cuo_id
                                   where ex.customer_order_id == s.cuo_id && cuo.customer_id == cu_id
                                   group od by od.se_id into t
                                   select new
                                   {
                                       t.Key
                                   }).ToList();
                List<serviceviewmodel> lts_add_se = new List<serviceviewmodel>();
                foreach (var se in lts_service)
                {
                    service ss = _dbContext.services.Find(se.Key);
                    serviceviewmodel serviceview = _mapper.Map<serviceviewmodel>(ss);


                    for (int j = 1; j < EnumService.se_type.Length + 1; j++)
                    {
                        if (j == ss.se_type)
                        {
                            serviceview.se_type_name = EnumService.se_type[j - 1];
                        }
                    }
                    var x = _dbContext.service_category.Find(ss.service_category_id);
                    if (x != null) serviceview.service_category_name = x.sc_name;


                    lts_add_se.Add(serviceview);
                }
                add.list_service_history = lts_add_se;
                #endregion
                lst_cuo_service_his.Add(add);
            }
            customerview.list_customer_order_service = lst_cuo_service_his;
            #endregion
            //lay ra lich su mua cua khach hang
            #region customer order product
            var list_cuo_product_history = _dbContext.customer_order.Where(s => s.customer_id == i.cu_id && s.cuo_code.Contains("ORS")).ToList();
            List<customerorderproducthistoryviewmodel> lst_cuo_product_his = new List<customerorderproducthistoryviewmodel>();
            foreach (customer_order s in list_cuo_product_history)
            {
                customerorderproducthistoryviewmodel add = _mapper.Map<customerorderproducthistoryviewmodel>(s);
                add.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                #region list_product
                var lts_product = (from ex in _dbContext.order_product
                                   join od in _dbContext.products on ex.product_id equals od.pu_id
                                   join cuo in _dbContext.customer_order on ex.customer_order_id equals cuo.cuo_id
                                   where ex.customer_order_id == s.cuo_id && cuo.customer_id == i.cu_id
                                   group od by od.pu_id into t
                                   select new
                                   {
                                       t.Key
                                   }).ToList();
                List<productviewmodelpluss> lts_add_se = new List<productviewmodelpluss>();
                foreach (var se in lts_product)
                {
                    product ss = _dbContext.products.Find(se.Key);
                    productviewmodelpluss productview = _mapper.Map<productviewmodelpluss>(ss);
                    var product_category = _dbContext.product_category.Find(ss.product_category_id);
                    if (product_category != null) productview.product_category_name = product_category.pc_name;
                    for (int j = 1; j < EnumProduct.pu_unit.Length + 1; j++)
                    {
                        if (j == ss.pu_unit)
                        {
                            productview.pu_unit_name = EnumProduct.pu_unit[j - 1];
                        }
                    }

                    lts_add_se.Add(productview);
                }
                add.list_product_history = lts_add_se;
                #endregion
                lst_cuo_product_his.Add(add);
            }
            customerview.list_customer_order_product = lst_cuo_product_his;
            #endregion
            //lay ra lich su cham soc cua khach hang
            var list_tran_history = _dbContext.transactions.Where(s => s.customer_id == i.cu_id).ToList();
            List<customertransactionviewmodel> lst_tra_his = new List<customertransactionviewmodel>();
            foreach (transaction s in list_tran_history)
            {
                customertransactionviewmodel add = _mapper.Map<customertransactionviewmodel>(s);
                add.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                lst_tra_his.Add(add);
            }

            customerview.list_transaction = lst_tra_his;
            return customerview;
        }
        public servicesearchcustomerviewmodel GetServiceInforCustomer(int cu_id)
        {

            servicesearchcustomerviewmodel res = new servicesearchcustomerviewmodel();
            var i = _dbContext.customers.Find(cu_id);
            var customerview = _mapper.Map<servicesearchcustomerviewmodel>(i);
            res = customerview;
            
            var sources = _dbContext.sources.FirstOrDefault(x => x.src_id == i.source_id);
            var customergroup = _dbContext.customer_group.FirstOrDefault(x => x.cg_id == i.customer_group_id);
            customerview.source_name = sources.src_name;
            customerview.customer_group_name = customergroup.cg_name;
            var curator = _dbContext.staffs.Find(i.cu_curator_id);
            var staff_cu = _dbContext.staffs.Find(i.staff_id);
            if (curator != null) customerview.cu_curator_name = curator.sta_fullname;
            if (staff_cu != null) customerview.staff_name = staff_cu.sta_fullname;

            for (int j = 1; j < EnumCustomer.cu_type.Length + 1; j++)
            {
                if (j == i.cu_type)
                {
                    customerview.cu_type_name = EnumCustomer.cu_type[j - 1];
                }
            }
            // lay ra dia chi khach hang 
            var list_address = _dbContext.ship_address.Where(s => s.customer_id == i.cu_id).ToList();
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


            return res;
        }
        public List<dropdown> GetAllType(int company_id)
        {

            List<dropdown> res = new List<dropdown>();
            List<customer_type> lts_s = _dbContext.customer_type.Where(x => x.company_id == company_id).ToList();
            foreach(customer_type i in  lts_s)
            {
                dropdown pu = new dropdown();
                pu.id = i.cut_id;
                pu.name = i.cut_name;

                res.Add(pu);
            }
            return res;

        }
        public PagedResults<customerviewexport> ExportCustomer(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, DateTime? start_date, DateTime? end_date, string name,int company_id)
        {
            if (name != null) name = name.Trim();
            List<customer> list;
            List<customerviewexport> res = new List<customerviewexport>();
            var skipAmount = pageSize * pageNumber;
            if (name == null)
            {
                list = _dbContext.customers.Where(x => x.company_id == company_id).ToList();
            }
            else list = _dbContext.customers.Where(x => (x.cu_fullname.Contains(name) || x.cu_code.Contains(name) || x.cu_email.Contains(name) || x.cu_note.Contains(name)) && x.company_id == company_id).ToList();
            if (source_id != null)
            {
                list = list.Where(x => x.source_id == source_id).ToList();
            }
            if (cu_type != null)
            {
                list = list.Where(x => x.cu_type == cu_type).ToList();
            }
            if (customer_group_id != null)
            {
                list = list.Where(x => x.customer_group_id == customer_group_id).ToList();
            }
            if (start_date != null)
            {
                list = list.Where(x => x.cu_create_date >= start_date).ToList();
            }
            if (end_date != null)
            {
                end_date = end_date.Value.AddDays(1);
                list = list.Where(x => x.cu_create_date <= end_date).ToList();
            }
            var total = list.Count();

            var results = list.OrderByDescending(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            foreach (customer i in results)
            {
                var customerview = _mapper.Map<customerviewexport>(i);
                var sources = _dbContext.sources.Find(i.source_id);
                var customergroup = _dbContext.customer_group.Find(i.customer_group_id);
                var curator = _dbContext.staffs.Find(i.cu_curator_id);
                var staff_cu = _dbContext.staffs.Find(i.staff_id);
                if (sources != null) customerview.source_name = sources.src_name;
                if (customergroup != null) customerview.customer_group_name = customergroup.cg_name;

                for (int j = 1; j < EnumCustomer.cu_type.Length + 1; j++)
                {
                    if (j == i.cu_type)
                    {
                        customerview.cu_type_name = EnumCustomer.cu_type[j - 1];
                    }
                }
                for (int j = 1; j < EnumCustomer.cu_flag_order.Length + 1; j++)
                {
                    if (j == i.cu_flag_order)
                    {
                        customerview.cu_flag_order_name = EnumCustomer.cu_flag_order[j - 1];
                    }
                }
                for (int j = 1; j < EnumCustomer.cu_flag_used.Length + 1; j++)
                {
                    if (j == i.cu_flag_used)
                    {
                        customerview.cu_flag_used_name = EnumCustomer.cu_flag_used[j - 1];
                    }
                }
                for (int j = 1; j < EnumCustomer.cu_status.Length + 1; j++)
                {
                    if (j == i.cu_status)
                    {
                        customerview.cu_status_name = EnumCustomer.cu_status[j - 1];
                    }
                }
                var address = _dbContext.ship_address.Where(x => x.customer_id == i.cu_id && x.sha_flag_center == 1).FirstOrDefault();
                if (address != null) customerview.cu_address = String.Concat(address.sha_province, "-", address.sha_district, "-", address.sha_ward, "-", address.sha_detail);

                //so dien thoai 
                var phone = _dbContext.customer_phones.Where(x => x.cp_type == 1).FirstOrDefault();
                if (phone != null) customerview.cu_mobile = phone.cp_phone_number;

                res.Add(customerview);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<customerviewexport>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };

        }
        public transactioncustomerviewmodel GetInforCustomerTransaction(int cu_id)
        {
            transactioncustomerviewmodel res = new transactioncustomerviewmodel();
            
            //lay ra thong tin khach hang 
            var cus = _dbContext.customers.Where(c => c.cu_id == cu_id).FirstOrDefault();
            var customerview = _mapper.Map<transactioncustomerviewmodel>(cus);
            var sources = _dbContext.sources.FirstOrDefault(x => x.src_id == cus.source_id);
            var customergroup = _dbContext.customer_group.FirstOrDefault(x => x.cg_id == cus.customer_group_id);
            var curator = _dbContext.staffs.Find(customerview.cu_curator_id);
            if(curator != null) customerview.cu_curator_name = curator.sta_fullname;
            if (sources != null)  customerview.source_name = sources.src_name;
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
                                      select new
                                      {
                                          od.cuo_id
                                      }).ToList();
            foreach (var cuo in list_customer_order_service)
            {
                var list_order_service = _dbContext.order_service.Where(o => o.customer_order_id == cuo.cuo_id).ToList();
                foreach (order_service ord in list_order_service)
                {
                    var se = _dbContext.services.Find(ord.service_id);
                    if(se != null)
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
                                           join cu in _dbContext.customers on cu_id equals cu.cu_id
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
            var list_customer_order_product = _dbContext.customer_order.Where(cuo => cuo.customer_id == cu_id).ToList();
            foreach (customer_order cuo in list_customer_order_product)
            {
                var list_order_product = _dbContext.order_product.Where(o => o.customer_order_id == cuo.cuo_id).ToList();
                foreach (order_product ord in list_order_product)
                {
                    transactionorderproductviewmodel add = _mapper.Map<transactionorderproductviewmodel>(ord);
                    var prodcut_cur = _dbContext.products.Where(pu => pu.pu_id == add.product_id).FirstOrDefault();
                    add.pu_name = prodcut_cur.pu_name;
                    for (int j = 1; j < EnumProduct.pu_unit.Length + 1; j++)
                    {
                        if (j == prodcut_cur.pu_unit)
                        {
                            add.pu_unit_name = EnumProduct.pu_unit[j - 1];
                        }
                    }
                    add.cu_fullname = _dbContext.customers.Where(c => c.cu_id == cus.cu_id).FirstOrDefault().cu_fullname;
                    for (int j = 1; j < EnumCustomerOrder.status.Length + 1; j++)
                    {
                        if (j == cuo.cuo_status)
                        {
                            add.cuo_status_name = EnumCustomerOrder.status[j - 1];
                        }
                    }

                    add.cuo_address = cuo.cuo_address;

                    add.op_total_value = ord.op_total_value;
                    list_add_order_product.Add(add);
                }

            }
            customerview.list_order_product = list_add_order_product;

            res = customerview;
            
            return res;
        }
        public List<dropdown> GetAllDropdown(int company_id)
        {
            List<dropdown> res = new List<dropdown>();
            List<customer> lts_s = _dbContext.customers.Where(x => x.company_id == company_id).ToList();
            foreach (customer cg in lts_s)
            {
                dropdown dr = new dropdown();
                dr.id = cg.cu_id;
                dr.name = cg.cu_fullname;
                res.Add(dr);
            }
            return res;
        }
    }
}