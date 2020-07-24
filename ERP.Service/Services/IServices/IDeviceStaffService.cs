using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Device;

namespace ERP.Service.Services.IServices
{
    public interface IDeviceStaffService : IGenericService<device_staff>
    {
        PagedResults<device_staff> CreatePagedResults(int pageNumber, int pageSize);
    }
}
