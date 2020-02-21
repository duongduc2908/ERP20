using AutoMapper;
using ERP.Common.Constants.Enums;
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
            var totalNumberOfRecords = list.Count();

            var results = list.ToList();
            foreach (product i in results)
            {
                var productview = _mapper.Map<productviewmodel>(i);
                var product_category = _dbContext.product_category.FirstOrDefault(x => x.pc_id == i.product_category_id);
                var supplier = _dbContext.suppliers.FirstOrDefault(x => x.su_id == i.provider_id);

                productview.product_category_name = product_category.pc_name;
                productview.provider_name = supplier.su_name;

                if (i.pu_unit == 0)
                {
                    productview.pu_unit_name = EnumProduct.pu_unit_0;
                }
                if (i.pu_unit == 1)
                {
                    productview.pu_unit_name = EnumProduct.pu_unit_1;
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

                if (i.pu_unit == 0)
                {
                    productview.pu_unit_name = EnumProduct.pu_unit_0;
                }
                if (i.pu_unit == 1)
                {
                    productview.pu_unit_name = EnumProduct.pu_unit_1;
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
        
        public PagedResults<productviewmodel> GetProducts(int pageNumber, int pageSize, string search_name, int? category_id)
        {

            List<productviewmodel> res = new List<productviewmodel>();

            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.products.Where(t => t.product_category_id == category_id && t.pu_name.Contains(search_name)).OrderBy(t => t.pu_id).Skip(skipAmount).Take(pageSize);
            if (category_id == null)
            {
                if (search_name != null)
                {
                    list = _dbContext.products.Where(t => t.pu_name.Contains(search_name)).OrderBy(t => t.pu_id).Skip(skipAmount).Take(pageSize);
                }
                else
                {
                    list = _dbContext.products.OrderBy(t => t.pu_id).Skip(skipAmount).Take(pageSize);
                }

            }
            if (search_name == null)
            {
                if (category_id != null)
                {
                    list = _dbContext.products.Where(t => t.product_category_id == category_id).OrderBy(t => t.pu_id).Skip(skipAmount).Take(pageSize);
                }
                else
                {
                    list = _dbContext.products.OrderBy(t => t.pu_id).Skip(skipAmount).Take(pageSize);
                }
            }

            var total = _dbContext.products.Count();
            var totalNumberOfRecords = list.Count();

            var results = list.ToList();
            foreach (product i in results)
            {
                var productview = _mapper.Map<productviewmodel>(i);
                var product_category = _dbContext.product_category.FirstOrDefault(x => x.pc_id == i.product_category_id);
                var supplier = _dbContext.suppliers.FirstOrDefault(x => x.su_id == i.provider_id);

                productview.product_category_name = product_category.pc_name;
                productview.provider_name = supplier.su_name;

                if (i.pu_unit == 0)
                {
                    productview.pu_unit_name = EnumProduct.pu_unit_0;
                }
                if (i.pu_unit == 1)
                {
                    productview.pu_unit_name = EnumProduct.pu_unit_1;
                }
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
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<string> GetUnit()
        {

            List<string> res = new List<string>();
            res.Add(EnumProduct.pu_unit_0);
            res.Add(EnumProduct.pu_unit_1);
            return new PagedResults<string>
            {
                Results = res,
                PageNumber = 0,
                PageSize = 0,
                TotalNumberOfPages = 0,
                TotalNumberOfRecords = 2
            };
        }
    }
}