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
    public class DepartmentRepository : GenericRepository<department>, IDepartmentRepository
    {
        public DepartmentRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public PagedResults<department> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.departments.OrderBy(t => t.de_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.departments.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<department>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public List<dropdown> Get_Level_One()
        {
            List<dropdown> res = new List<dropdown>();
            var list_department = _dbContext.departments.Where(x => x.de_parent_id == 0).ToList();
            foreach (department dp in list_department)
            {
                dropdown dr = new dropdown();
                dr.id = dp.de_id;
                dr.name = dp.de_name;
                res.Add(dr);
            }
            return res;
        }
        public List<dropdown> Get_Children(int id)
        {
            List<dropdown> res = new List<dropdown>();
            var list_department = _dbContext.departments.Where(x => x.de_parent_id == id).ToList();
            foreach(department dp in list_department)
            {
                dropdown dr = new dropdown();
                dr.id = dp.de_id;
                dr.name = dp.de_name;
                res.Add(dr);
            }
            return res;
        }
    }
}