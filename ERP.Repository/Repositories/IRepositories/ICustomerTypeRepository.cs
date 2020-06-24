using ERP.Common.GenericRepository;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface ICustomerTypeRepository : IGenericRepository<customer_type>
    {
        List<dropdown> GetAllDropdown(int company_id);
    }
}
