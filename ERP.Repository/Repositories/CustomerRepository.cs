using AutoMapper;
using ERP.Common.Constants.Enums;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Customer;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.Sms;
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
        
        public PagedResults<customerviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name)
        {
            if (name != null) name = name.Trim();
           
            List<customerviewmodel> res = new List<customerviewmodel>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.customers.OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);

            /**24TH**/
            #region [24 case]
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
                list = _dbContext.customers.Where(t => (t.cu_fullname.Contains(name) || t.cu_mobile.Contains(name) || t.cu_email.Contains(name) || t.cu_code.Contains(name))).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 0 && check_type == 0 && check_group_id == 1 && check_name ==0) 
            {
                list = _dbContext.customers.Where(t => t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }            
            if(check_source_id == 0 && check_type == 0 && check_group_id == 1 && check_name ==1) 
            {
                list = _dbContext.customers.Where(t => (t.cu_fullname.Contains(name) || t.cu_mobile.Contains(name) || t.cu_email.Contains(name) || t.cu_code.Contains(name))&& t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
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
                list = _dbContext.customers.Where(t => t.source_id == source_id && (t.cu_fullname.Contains(name) || t.cu_mobile.Contains(name) || t.cu_email.Contains(name) || t.cu_code.Contains(name))&& t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
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
            #endregion
            var total = _dbContext.customers.Count();

            var results = list.ToList();
            foreach(customer i in results)
            {
                var customerview = _mapper.Map<customerviewmodel>(i);
                var sources = _dbContext.sources.FirstOrDefault(x => x.src_id == i.source_id);
                var customergroup = _dbContext.customer_group.FirstOrDefault(x => x.cg_id == i.customer_group_id);
                customerview.source_name = sources.src_name;
                customerview.customer_group_name = customergroup.cg_name;
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
                //lay ra lich su mua cua khach hang
                var list_cuo_history = _dbContext.customer_order.Where(s => s.customer_id == i.cu_id).ToList();
                List<customerorderhistoryviewmodel> lst_cuo_his = new List<customerorderhistoryviewmodel>();
                foreach (customer_order s in list_cuo_history)
                {
                    customerorderhistoryviewmodel add = _mapper.Map<customerorderhistoryviewmodel>(s);
                    add.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                    lst_cuo_his.Add(add);
                }

                customerview.list_customer_order = lst_cuo_his;
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

        public PagedResults<smscustomerviewmodel> GetAllPageSearchSms(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name)
        {
            if (name != null) name = name.Trim();

            List<smscustomerviewmodel> res = new List<smscustomerviewmodel>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.customers.OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);

            /**24TH**/
            #region [24 case]
            int check_source_id = 0;
            int check_type = 0;
            int check_group_id = 0;
            int check_name = 0;
            if (source_id != null) { check_source_id = 1; }
            if (cu_type != null) { check_type = 1; }
            if (customer_group_id != null) { check_group_id = 1; }
            if (name != null) { check_name = 1; }


            if (check_source_id == 0 && check_type == 0 && check_group_id == 0 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => (t.cu_fullname.Contains(name) || t.cu_mobile.Contains(name) || t.cu_email.Contains(name) || t.cu_code.Contains(name))).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 0 && check_type == 0 && check_group_id == 1 && check_name == 0)
            {
                list = _dbContext.customers.Where(t => t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 0 && check_type == 0 && check_group_id == 1 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => (t.cu_fullname.Contains(name) || t.cu_mobile.Contains(name) || t.cu_email.Contains(name) || t.cu_code.Contains(name)) && t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 0 && check_type == 1 && check_group_id == 0 && check_name == 0)
            {
                list = _dbContext.customers.Where(t => t.cu_type == cu_type).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 0 && check_type == 1 && check_group_id == 0 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => t.cu_type == cu_type).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 0 && check_type == 1 && check_group_id == 1 && check_name == 0)
            {
                list = _dbContext.customers.Where(t => t.cu_type == cu_type && t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 0 && check_type == 1 && check_group_id == 1 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => t.cu_type == cu_type && t.customer_group_id == customer_group_id && t.cu_fullname.Contains(name)).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 0 && check_group_id == 0 && check_name == 0)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 0 && check_group_id == 0 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_fullname.Contains(name)).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 0 && check_group_id == 1 && check_name == 0)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 0 && check_group_id == 1 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && (t.cu_fullname.Contains(name) || t.cu_mobile.Contains(name) || t.cu_email.Contains(name) || t.cu_code.Contains(name)) && t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 1 && check_group_id == 0 && check_name == 0)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_type == cu_type).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 1 && check_group_id == 0 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_type == cu_type).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 1 && check_group_id == 1 && check_name == 0)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_type == cu_type && t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 1 && check_group_id == 1 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_type == cu_type && t.customer_group_id == customer_group_id && t.cu_fullname.Contains(name)).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            #endregion
            var total = _dbContext.customers.Count();

            var results = list.ToList();
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

            customerviewmodel res = new customerviewmodel();
            var customer_cur = _dbContext.customers.Where(i => i.cu_id == cu_id).FirstOrDefault();
            var customerview = _mapper.Map<customerviewmodel>(customer_cur);
            res = customerview;
            var sources = _dbContext.sources.FirstOrDefault(x => x.src_id == customer_cur.source_id);
            var customergroup = _dbContext.customer_group.FirstOrDefault(x => x.cg_id == customer_cur.customer_group_id);
            res.source_name = sources.src_name;
            res.customer_group_name = customergroup.cg_name;
            var curator = _dbContext.staffs.Find(customer_cur.cu_curator_id);
            if (curator != null) customerview.cu_curator_name = curator.sta_fullname;
            var staff_cu = _dbContext.staffs.Find(customer_cur.staff_id);
            if (staff_cu != null) customerview.staff_name = staff_cu.sta_fullname;
            for (int j = 1; j < EnumCustomer.cu_type.Length+1; j++)
            {
                if (j == customer_cur.cu_type)
                {
                    res.cu_type_name = EnumCustomer.cu_type[j-1];
                }
            }
            
            
            // lay ra dia chi khach hang 
            var list_address = _dbContext.ship_address.Where(s => s.customer_id == customer_cur.cu_id).ToList();
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
            //lay ra lich su mua cua khach hang
            var list_cuo_history = _dbContext.customer_order.Where(s => s.customer_id == customer_cur.cu_id).ToList();
            List<customerorderhistoryviewmodel> lst_cuo_his = new List<customerorderhistoryviewmodel>();
            foreach (customer_order s in list_cuo_history)
            {
                customerorderhistoryviewmodel add = _mapper.Map<customerorderhistoryviewmodel>(s);
                add.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                lst_cuo_his.Add(add);
            }

            customerview.list_customer_order = lst_cuo_his;
            //lay ra lich su cham soc cua khach hang
            var list_tran_history = _dbContext.transactions.Where(s => s.customer_id == customer_cur.cu_id).ToList();
            List<customertransactionviewmodel> lst_tra_his = new List<customertransactionviewmodel>();
            foreach (transaction s in list_tran_history)
            {
                customertransactionviewmodel add = _mapper.Map<customertransactionviewmodel>(s);
                add.staff_name = _dbContext.staffs.Where(t => t.sta_id == s.staff_id).FirstOrDefault().sta_fullname;
                lst_tra_his.Add(add);
            }

            customerview.list_transaction = lst_tra_his;
            
            return res;
        }
        public List<dropdown> GetAllType()
        {

            List<dropdown> res = new List<dropdown>();

            for (int i = 1; i < 3; i++)
            {
                dropdown pu = new dropdown();
                pu.id = i;
                pu.name = EnumCustomer.cu_type[i-1];

                res.Add(pu);
            }
            return res;

        }
        public PagedResults<customerview> ExportCustomer(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name)
        {
            if (name != null) name = name.Trim();

            List<customerview> res = new List<customerview>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.customers.OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);

            /**24TH**/
            #region [24 case]
            int check_source_id = 0;
            int check_type = 0;
            int check_group_id = 0;
            int check_name = 0;
            if (source_id != null) { check_source_id = 1; }
            if (cu_type != null) { check_type = 1; }
            if (customer_group_id != null) { check_group_id = 1; }
            if (name != null) { check_name = 1; }


            if (check_source_id == 0 && check_type == 0 && check_group_id == 0 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => (t.cu_fullname.Contains(name) || t.cu_mobile.Contains(name) || t.cu_email.Contains(name) || t.cu_code.Contains(name))).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 0 && check_type == 0 && check_group_id == 1 && check_name == 0)
            {
                list = _dbContext.customers.Where(t => t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 0 && check_type == 0 && check_group_id == 1 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => (t.cu_fullname.Contains(name) || t.cu_mobile.Contains(name) || t.cu_email.Contains(name) || t.cu_code.Contains(name)) && t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 0 && check_type == 1 && check_group_id == 0 && check_name == 0)
            {
                list = _dbContext.customers.Where(t => t.cu_type == cu_type).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 0 && check_type == 1 && check_group_id == 0 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => t.cu_type == cu_type).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 0 && check_type == 1 && check_group_id == 1 && check_name == 0)
            {
                list = _dbContext.customers.Where(t => t.cu_type == cu_type && t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 0 && check_type == 1 && check_group_id == 1 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => t.cu_type == cu_type && t.customer_group_id == customer_group_id && t.cu_fullname.Contains(name)).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 0 && check_group_id == 0 && check_name == 0)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 0 && check_group_id == 0 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_fullname.Contains(name)).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 0 && check_group_id == 1 && check_name == 0)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 0 && check_group_id == 1 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && (t.cu_fullname.Contains(name) || t.cu_mobile.Contains(name) || t.cu_email.Contains(name) || t.cu_code.Contains(name)) && t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 1 && check_group_id == 0 && check_name == 0)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_type == cu_type).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 1 && check_group_id == 0 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_type == cu_type).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 1 && check_group_id == 1 && check_name == 0)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_type == cu_type && t.customer_group_id == customer_group_id).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            if (check_source_id == 1 && check_type == 1 && check_group_id == 1 && check_name == 1)
            {
                list = _dbContext.customers.Where(t => t.source_id == source_id && t.cu_type == cu_type && t.customer_group_id == customer_group_id && t.cu_fullname.Contains(name)).OrderBy(t => t.cu_id).Skip(skipAmount).Take(pageSize);
            }
            #endregion
            var total = _dbContext.customers.Count();

            var results = list.ToList();
            foreach (customer i in results)
            {
                var customerex = _mapper.Map<customerview>(i);
                var sources = _dbContext.sources.FirstOrDefault(x => x.src_id == i.source_id);
                var customergroup = _dbContext.customer_group.FirstOrDefault(x => x.cg_id == i.customer_group_id);
                customerex.source_name = sources.src_name;
                customerex.customer_group_name = customergroup.cg_name;
                var curator = _dbContext.staffs.Find(i.cu_curator_id);
                var staff_cu = _dbContext.staffs.Find(i.staff_id);
                if (curator != null) customerex.cu_curator_name = curator.sta_fullname;
                if (staff_cu != null) customerex.staff_name = staff_cu.sta_fullname;

                for (int j = 1; j < EnumCustomer.cu_type.Length+1; j++)
                {
                    if (j == i.cu_type)
                    {
                        customerex.cu_type_name = EnumCustomer.cu_type[j - 1];
                    }
                }
                if (customerex.cu_status == 1 || customerex.cu_status == null) customerex.cu_status_name = "Kích hoạt";
                else customerex.cu_status_name = "Khóa";

                res.Add(customerex);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<customerview>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };

        }

    }
}