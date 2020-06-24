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
    public interface ICustomerTypeService : IGenericService<customer_type>
    {
        List<dropdown> GetAllDropdown(int company_id);
    }
}
