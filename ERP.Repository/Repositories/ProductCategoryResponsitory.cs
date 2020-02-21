using AutoMapper;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Repository.Repositories.IRepositories;
using Microsoft.AspNetCore.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class ProductCategoryResponsitory : GenericRepository<product_category>, IProductCategoryRepository
    {
        private readonly IMapper _mapper;
        public ProductCategoryResponsitory(ERPDbContext dbContext, IMapper _mapper) : base(dbContext)
        {
            this._mapper = _mapper;
        }

        public PagedResults<product_category> GetAllPageById(int pageNumber, int pageSize, int id)
        {
            List<product_category> res = new List<product_category>();
            var list = _dbContext.product_category.OrderBy(t => t.pc_id).ToList();
            var totalNumberOfRecords = list.Count();
            var results = list.ToList();

            return new PagedResults<product_category>
            {
                Results = res,
                PageNumber = 0,
                PageSize = 0,
                TotalNumberOfPages = 0,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<product_category> GetAllPage(int pageNumber, int pageSize)
        {
            List<product_category> res = new List<product_category>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.product_category.OrderBy(t => t.pc_id).Skip(skipAmount).Take(pageSize);
            var totalNumberOfRecords = list.Count();

            var results = list.ToList();
            foreach (product_category i in results)
            {
                var productview = _mapper.Map<product_category>(i);
                
                //staffview.group_name = group_role.gr_name;
                res.Add(productview);
            }
            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<product_category>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<product_category> GetProductCategorys(string search_name)
        {

            var list = _dbContext.product_category.Where(p => p.pc_name.Contains(search_name)).ToList();
            var totalNumberOfRecords = list.Count();
            var results = list.ToList();

            return new PagedResults<product_category>
            {
                Results = results,
                PageNumber = 0,
                PageSize = 0,
                TotalNumberOfPages = 0,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<string> GetAllName()
        {

            List<string> res = new List<string>();
            

            var list = _dbContext.product_category.OrderBy(t => t.pc_id).ToList();
            var totalNumberOfRecords = list.Count();

            var results = list.ToList();
            foreach (product_category i in results)
            {
                string name = "";
                name = i.pc_name;
                res.Add(name);
            }
            

            return new PagedResults<string>
            {
                Results = res,
                PageNumber = 0,
                PageSize = 0,
                TotalNumberOfPages = 0,
                TotalNumberOfRecords = 0
            };
        }
    }
}