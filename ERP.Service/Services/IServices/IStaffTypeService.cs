using ERP.Common.GenericService;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service.Services.IServices
{
    public interface IStaffTypeService : IGenericService<staff_type>
    {
        List<dropdown> GetAllDropdown(int company_id);
    }
}
