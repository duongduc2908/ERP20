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
    public class SocialService : GenericService<social>, ISocialService
    {
        private readonly ISocialRepository _repository;
        public SocialService(ISocialRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<social> GetAllPage(int pageNumber, int pageSize)
        {
            return this._repository.GetAllPage(pageNumber, pageSize);
        }
    }
}