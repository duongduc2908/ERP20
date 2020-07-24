using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Repository.Repositories.IRepositories;

namespace ERP.Repository.Repositories
{
    public class DeviceStaffRepository : GenericRepository<device_staff>, IDeviceStaffRepository
    {
        public DeviceStaffRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public PagedResults<device_staff> CreatePagedResults(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}