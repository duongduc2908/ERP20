using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface IDeviceStaffRepository : IGenericRepository<device_staff>
    {
        PagedResults<device_staff> CreatePagedResults(int pageNumber, int pageSize);
    }
}
