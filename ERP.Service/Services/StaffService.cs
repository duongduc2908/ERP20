using ERP.Common.GenericRepository;
using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data;
using ERP.Data.ModelsERP;
using ERP.Repository.Repositories;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class StaffService : GenericService<staff>, IStaffService
    {
        private readonly IStaffRepository _repository;
        public StaffService(IStaffRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<staff> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        public void ChangePassword(ChangePasswordBindingModel model, int id)
        {
            this._repository.ChangePassword(model, id);
        }
    }
}