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
    public class SmsTemplateService : GenericService<sms_template>, ISmsTemplateService
    {
        private readonly ISmsTemplateRepository _repository;
        public SmsTemplateService(ISmsTemplateRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<sms_template> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
    }
}