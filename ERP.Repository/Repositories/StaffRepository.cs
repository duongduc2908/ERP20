using ERP.Common.Constants;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class StaffRepository : GenericRepository<staff>, IStaffRepository
    {
        public StaffRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public PagedResults<staff> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.staffs.OrderBy(t => t.sta_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.staffs.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<staff>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public void ChangePassword(ChangePasswordBindingModel model, int id)
        {
            
            var current_user = _dbContext.staffs.FirstOrDefault(x => x.sta_id == id);
            var new_user = current_user;
            if(current_user != null)
            {
                if(current_user.sta_password == HashMd5.convertMD5(model.OldPassword))
                {
                    new_user.sta_password = HashMd5.convertMD5(model.NewPassword);
                    _dbContext.Entry(current_user).CurrentValues.SetValues(new_user);
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}