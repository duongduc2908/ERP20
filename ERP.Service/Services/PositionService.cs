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
    public class PositionService : GenericService<position>, IPositionService
    {
        private readonly IPositionRepository _repository;
        public PositionService(IPositionRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<position> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        public List<dropdown> GetDropdowns(int company_id)
        {
            return this._repository.GetDropdowns(company_id);
        }
    }
}