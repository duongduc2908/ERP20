using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView.Sms;
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

        public PagedResults<smstemplatemodelview> CreatePagedResults(int pageNumber, int pageSize, string search_name)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize, search_name);
        }
    }
}