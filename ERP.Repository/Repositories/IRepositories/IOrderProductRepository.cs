using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface IOrderProductRepository : IGenericRepository<order_product>
    {
        PagedResults<order_product> CreatePagedResults(int pageNumber, int pageSize);
        PagedResults<orderproductviewmodel> GetAllOrderProduct(int customer_order_id);
    }
}
