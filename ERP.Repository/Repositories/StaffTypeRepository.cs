using ERP.Common.GenericRepository;
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
    public class StaffTypeRepository : GenericRepository<staff_type>, IStaffTypeRepository
    {
        public StaffTypeRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public List<dropdown> GetAllDropdown(int company_id)
        {
            List<dropdown> res = new List<dropdown>();
            List<staff_type> lts_s = _dbContext.staff_type.Where(x => x.company_id == company_id).ToList();
            foreach (staff_type stt in lts_s)
            {
                dropdown dr = new dropdown();
                dr.id = stt.sttp_id;
                dr.name = stt.sttp_name;
                res.Add(dr);
            }
            return res;
        }
    }
}