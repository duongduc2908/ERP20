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
    public class DepartmentService : GenericService<department>, IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        public DepartmentService(IDepartmentRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<department> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        public List<dropdown> Get_Level_One()
        {
            return this._repository.Get_Level_One();
        }
        public List<dropdown> Get_Children(int id)
        {
            return this._repository.Get_Children(id);
        }

    }
}