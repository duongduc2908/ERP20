using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Statistics;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class OrderProducService : GenericService<order_product>, IOrderProductService
    {
        private readonly IOrderProductRepository _repository;
        public OrderProducService(IOrderProductRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<order_product> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        
        public PagedResults<orderproductviewmodel> GetAllOrderProduct(int customer_order_id)
        {
            return this._repository.GetAllOrderProduct(customer_order_id);
        }
        public PagedResults<statisticsorderviewmodel> ResultStatisticsOrder(int pageNumber, int pageSize, int staff_id, bool month, bool week, bool day, string search_name)
        {
            return this._repository.ResultStatisticsOrder(pageNumber,pageSize,staff_id,month,week,day,search_name);
        }
    }
}