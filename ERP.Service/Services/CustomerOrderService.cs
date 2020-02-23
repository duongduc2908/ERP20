using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;

namespace ERP.Service.Services
{
    public class CustomerOrderService : GenericService<customer_order>, ICustomerOrderService
    {
        private readonly ICustomerOrderRepository _repository;
        public CustomerOrderService(ICustomerOrderRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<customerorderviewmodel> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        public PagedResults<customer_order> GetAllOrderById(int id)
        {
            return this._repository.GetAllOrderById(id);
        }
        public PagedResults<customerorderviewmodel> GetAllSearch(int pageNumber, int pageSize, int? payment_type_id, string name)
        {
            return this._repository.GetAllSearch(pageNumber, pageSize,  payment_type_id, name);
        }
    }
}