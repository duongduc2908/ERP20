using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service.Services.IServices
{
    public interface ICustomerOrderService : IGenericService<customer_order>
    {
        PagedResults<customerorderviewmodel> CreatePagedResults(int pageNumber, int pageSize);
        PagedResults<customer_order> GetAllOrderById(int id);
    }
}
