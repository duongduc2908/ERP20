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
    public class GroupRoleRepository : GenericRepository<group_role>, IGroupRoleRepository
    {
        public GroupRoleRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public PagedResults<group_role> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.group_role.OrderBy(t => t.gr_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.group_role.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<group_role>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public List<dropdown> GetDropdown(int company_id)
        {
            List<dropdown> res = new List<dropdown>();
            List<group_role> lts_gr =  _dbContext.group_role.Where(x => x.company_id == company_id).ToList();
            foreach(group_role gr in lts_gr)
            {
                dropdown dr = new dropdown();
                dr.id = gr.gr_id;
                dr.name = gr.gr_name;
                res.Add(dr);
            }
            return res;
        }
    }
}