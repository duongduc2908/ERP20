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
        public customerordermodelview GetAllOrderById(int id)
        {
            return this._repository.GetAllOrderById(id);
        }
        public PagedResults<customerorderviewmodel> GetAllSearch(int pageNumber, int pageSize, int? payment_type_id, string name)
        {
            return this._repository.GetAllSearch(pageNumber, pageSize,  payment_type_id, name);
        }
        
        public PagedResults<customerorderviewmodel> ResultStatisticsCustomerOrder(int pageNumber, int pageSize, int staff_id, bool month, bool week, bool day)
        {
            return this._repository.ResultStatisticsCustomerOrder(pageNumber, pageSize, staff_id,month,  week, day);
        }
        public int ResultStatisticsByMonth(int staff_id)
        {
            return this._repository.ResultStatisticsByMonth(staff_id);
        }
        public int ResultStatisticsByWeek(int staff_id)
        {
            return this._repository.ResultStatisticsByWeek(staff_id);
        }
        public int ResultStatisticsByDay(int staff_id)
        {
            return this._repository.ResultStatisticsByDay(staff_id);
        }
    }
}