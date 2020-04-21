using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;

namespace ERP.Service.Services
{
    public class CustomerPhoneService : GenericService<customer_phone>, ICustomerPhoneService
    {
        private readonly ICustomerPhoneRepository _repository;
        public CustomerPhoneService(ICustomerPhoneRepository repository) : base(repository)
        {
            this._repository = repository;
        }

    }
}