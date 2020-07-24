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
    public interface IDeviceService : IGenericService<device>
    {
        PagedResults<device> CreatePagedResults(int pageNumber, int pageSize);
        PagedResults<deviceviewmodel> GetAllSearch(int pageNumber, int pageSize, string search_name, int compnay_id);
        List<dropdown> GetAllDropDown(int company_id);
        deviceviewmodel GetById(int dev_id);
    }
}
