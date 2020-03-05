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
    public class SmsStrategyService : GenericService<sms_strategy>, ISmsStrategyService
    {
        private readonly ISmsStrategyRepository _repository;
        public SmsStrategyService(ISmsStrategyRepository repository) : base(repository)
        {
            this._repository = repository;
        }
        
        public PagedResults<smsstrategyviewmodel> GetAllPageSearch(int pageNumber, int pageSize, string search_name)
        {
            return this._repository.GetAllPageSearch(pageNumber, pageSize, search_name);
        }
    }
}