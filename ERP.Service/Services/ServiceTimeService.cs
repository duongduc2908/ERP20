using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class ServiceTimeService : GenericService<service_time>, IServiceTimeService
    {
        private readonly IServiceTimeRepository _repository;
        public ServiceTimeService(IServiceTimeRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<service_time> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        public List<dropdown> GetRepeatType()
        {
            return this._repository.GetRepeatType();
        }
    }
}