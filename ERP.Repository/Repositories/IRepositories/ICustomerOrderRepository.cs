using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface ICustomerOrderRepository : IGenericRepository<customer_order>
    {
        PagedResults<customerorderviewmodel> CreatePagedResults(int pageNumber, int pageSize);
        customerordermodelview GetAllOrderById(int id);
        PagedResults<customerorderviewmodel> GetAllSearch(int pageNumber, int pageSize, int? payment_type_id, string name);
    }
}