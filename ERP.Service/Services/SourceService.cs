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
    public class SourceService : GenericService<source>, ISourceService
    {
        private readonly ISourceRepository _repository;
        public SourceService(ISourceRepository repository) : base(repository)
        {
            this._repository = repository;
        }
        public PagedResults<source> GetAllPage(int pageNumber, int pageSize)
        {
            return this._repository.GetAllPage(pageNumber, pageSize);
        }
        public PagedResults<source> GetAllPageById(int pageNumber, int pageSize, int id)
        {
            return this._repository.GetAllPageById(pageNumber, pageSize, id);
        }

        public PagedResults<source> GetSources(string search_name)
        {
            return this._repository.GetSources(search_name);
        }
    }
}