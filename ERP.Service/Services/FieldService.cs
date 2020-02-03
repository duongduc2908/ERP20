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
    public class FieldService : GenericService<field>, IFieldService
    {
        private readonly IFieldRepository _repository;
        public FieldService(IFieldRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<field> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
    }
}