using AutoMapper;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class SupplierRepository : GenericRepository<supplier>, ISupplierRepository
    {
        private readonly IMapper _mapper;
        public SupplierRepository(ERPDbContext dbContext, IMapper _mapper) : base(dbContext)
        {
            this._mapper = _mapper;
        }

        public PagedResults<supplier> GetAllPageById(int pageNumber, int pageSize, int id)
        {
            List<supplier> res = new List<supplier>();
            var list = _dbContext.suppliers.OrderBy(t => t.su_id).ToList();
            var totalNumberOfRecords = list.Count();
            var results = list.ToList();

            return new PagedResults<supplier>
            {
                Results = res,
                PageNumber = 0,
                PageSize = 0,
                TotalNumberOfPages = 0,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<supplier> GetAllPage(int pageNumber, int pageSize)
        {
            List<supplier> res = new List<supplier>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.suppliers.OrderBy(t => t.su_id).Skip(skipAmount).Take(pageSize);
            var totalNumberOfRecords = list.Count();

            var results = list.ToList();
            foreach (supplier i in results)
            {
                var productview = _mapper.Map<supplier>(i);
                //var deparment = _dbContext.departments.FirstOrDefault(x => x.de_id == i.department_id);
                //var position = _dbContext.positions.FirstOrDefault(x => x.pos_id == i.position_id);
                //var group_role = _dbContext.group_role.FirstOrDefault(x => x.gr_id == i.group_role_id);
                //staffview.department_name = deparment.de_name;
                //staffview.position_name = position.pos_name;
                //staffview.group_name = group_role.gr_name;
                res.Add(productview);
            }
            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<supplier>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<supplier> GetSupliers(string search_name)
        {

            var list = _dbContext.suppliers.Where(p => p.su_name.Contains(search_name)).ToList();
            var totalNumberOfRecords = list.Count();
            var results = list.ToList();

            return new PagedResults<supplier>
            {
                Results = results,
                PageNumber = 0,
                PageSize = 0,
                TotalNumberOfPages = 0,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
    }
}