using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class ExecutorService : GenericService<executor>, IExecutorService
    {
        private readonly IExecutorRepository _repository;
        public ExecutorService(IExecutorRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<executor> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
    }
}