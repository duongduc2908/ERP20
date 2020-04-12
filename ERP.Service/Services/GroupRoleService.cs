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
    public class GroupRoleService : GenericService<group_role>, IGroupRoleService
    {
        private readonly IGroupRoleRepository _repository;
        public GroupRoleService(IGroupRoleRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<group_role> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        public List<dropdown> GetDropdown()
        {
            return this._repository.GetDropdown();
        }
    }
}