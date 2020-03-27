using AutoMapper;
using ERP.Common.Constants.Enums;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.Product;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class ProductRepository : GenericRepository<product>, IProductRepository
    {
        private readonly IMapper _mapper;
        public ProductRepository(ERPDbContext dbContext, IMapper _mapper) : base(dbContext)
        {
            this._mapper = _mapper;
        }
        
        public PagedResults<productviewmodel> GetAllPageById( int id)
        {
            List<productviewmodel> res = new List<productviewmodel>();
            var list = _dbContext.products.Where(i => i.pu_id == id).ToList();
            var totalNumberOfRecords = _dbContext.products.Count();

            var results = list.ToList();
            foreach (product i in results)
            {
                var productview = _mapper.Map<productviewmodel>(i);
                var product_category = _dbContext.product_category.FirstOrDefault(x => x.pc_id == i.product_category_id);
                var supplier = _dbContext.suppliers.FirstOrDefault(x => x.su_id == i.provider_id);

                productview.product_category_name = product_category.pc_name;
                productview.provider_name = supplier.su_name;
                for(int j = 1; j< 3; j++)
                {
                    if(j == i.pu_unit)
                    {
                        productview.pu_unit_name = EnumProduct.pu_unit[j-1];
                    }
                }
               
                res.Add(productview);
            }
            
            return new PagedResults<productviewmodel>
            {
                Results = res,
                PageNumber = 0,
                PageSize = 0,
                TotalNumberOfPages = 0,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<productviewmodel> GetAllPage(int pageNumber, int pageSize)
        {
            List<productviewmodel> res = new List<productviewmodel>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.products.OrderBy(t => t.pu_id).Skip(skipAmount).Take(pageSize);
            var totalNumberOfRecords = list.Count();

            var results = list.ToList();
            foreach (product i in results)
            {
                var productview = _mapper.Map<productviewmodel>(i);
                var product_category = _dbContext.product_category.FirstOrDefault(x => x.pc_id == i.product_category_id);
                var supplier = _dbContext.suppliers.FirstOrDefault(x => x.su_id == i.provider_id);
               
                productview.product_category_name = product_category.pc_name;
                productview.provider_name = supplier.su_name;

                for (int j = 1; j < EnumProduct.pu_unit.Length+1; j++)
                {
                    if (j == i.pu_unit)
                    {
                        productview.pu_unit_name = EnumProduct.pu_unit[j-1];
                    }
                }
                res.Add(productview);
            }
            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<productviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
       
        public PagedResults<productviewmodel> GetProducts(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name, int? category_id)
        {

            List<productviewmodel> res = new List<productviewmodel>();
            List<product> list = new List<product>();
            var skipAmount = pageSize * pageNumber;
            if(search_name == null)
            {
                list = _dbContext.products.ToList();
            }
            else list = _dbContext.products.Where(t => t.pu_name.Contains(search_name)).ToList();
            if (category_id != null)
            {
                list = list.Where(x => x.product_category_id == category_id).ToList();
            }
            if (start_date != null)
            {
                list = list.Where(x => x.pu_create_date >= start_date).ToList();
            }
            if (end_date != null)
            {
                end_date = end_date.Value.AddDays(1);
                list = list.Where(x => x.pu_create_date <= end_date).ToList();
            }
            var total = list.Count();
            
            var results = list.OrderByDescending(t => t.pu_id).Skip(skipAmount).Take(pageSize);
            foreach (product i in results)
            {
                var productview = _mapper.Map<productviewmodel>(i);
                var product_category = _dbContext.product_category.FirstOrDefault(x => x.pc_id == i.product_category_id);
                var supplier = _dbContext.suppliers.FirstOrDefault(x => x.su_id == i.provider_id);

                productview.product_category_name = product_category.pc_name;
                productview.provider_name = supplier.su_name;

                for (int j = 1; j < EnumProduct.pu_unit.Length+1; j++)
                {
                    if (j == i.pu_unit)
                    {
                        productview.pu_unit_name = EnumProduct.pu_unit[j-1];
                    }
                }

                //Lay ra thong tin san pham 
                var list_orp_history = _dbContext.order_product.Where(s => s.product_id == i.pu_id).ToList();
                List<orderproducthistoryviewmodel> lst_orp = new List<orderproducthistoryviewmodel>();
                foreach (order_product s in list_orp_history)
                {
                    orderproducthistoryviewmodel add = _mapper.Map<orderproducthistoryviewmodel>(s);
                    var customer_order = _dbContext.customer_order.Where(cuo => cuo.cuo_id == s.customer_order_id).FirstOrDefault();
                    add.cuo_code = customer_order.cuo_code;
                    add.cuo_date = customer_order.cuo_date;

                    //Lay ra khach hang 
                    var customer = _dbContext.customers.Where(c => c.cu_id == customer_order.customer_id).FirstOrDefault();
                    add.cu_fullname = customer.cu_fullname;

                    var staff = _dbContext.staffs.Where(sta => sta.sta_id == customer_order.staff_id).FirstOrDefault();
                    add.sta_fullname = staff.sta_fullname;
                    add.pu_unit_name = productview.pu_unit_name;
                    lst_orp.Add(add);
                }
                productview.list_orp_history = lst_orp;
                res.Add(productview);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<productviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };
        }
        public PagedResults<productview> ExportProduct(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name, int? category_id)
        {

            List<productview> res = new List<productview>();
            List<product> list = new List<product>();

            var skipAmount = pageSize * pageNumber;
            if (search_name == null)
            {
                list = _dbContext.products.ToList();
            }
            else list = _dbContext.products.Where(t => t.pu_name.Contains(search_name)).ToList();
            if (category_id != null)
            {
                list = list.Where(x => x.product_category_id == category_id).ToList();
            }
            if (start_date != null)
            {
                list = list.Where(x => x.pu_create_date >= start_date).ToList();
            }
            if (end_date != null)
            {
                end_date = end_date.Value.AddDays(1);
                list = list.Where(x => x.pu_create_date <= end_date).ToList();
            }
            var total = _dbContext.products.Count();

            var results = list.OrderBy(t => t.pu_id).Skip(skipAmount).Take(pageSize);
            foreach (product i in results)
            {
                var productview = _mapper.Map<productview>(i);
                var product_category = _dbContext.product_category.FirstOrDefault(x => x.pc_id == i.product_category_id);
                var supplier = _dbContext.suppliers.FirstOrDefault(x => x.su_id == i.provider_id);
                
                productview.product_category_name = product_category.pc_name;
                productview.provider_name = supplier.su_name;

                for (int j = 1; j < EnumProduct.pu_unit.Length + 1; j++)
                {
                    if (j == i.pu_unit)
                    {
                        productview.pu_unit_name = EnumProduct.pu_unit[j - 1];
                    }
                }
                res.Add(productview);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<productview>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };
        }
        public List<dropdown> GetUnit()
        {

            List<dropdown> res = new List<dropdown>();
            for(int i = 1; i < EnumProduct.pu_unit.Length+1; i++)
            {
                dropdown pu = new dropdown();
                pu.id = i;
                pu.name = EnumProduct.pu_unit[i-1];
                
                res.Add(pu);
            }
            return res;
        }
    }
}