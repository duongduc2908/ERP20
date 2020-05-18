using AutoMapper;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Statistics;
using ERP.Data.ModelsERP.ModelView.Transaction;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;


namespace ERP.API.Controllers.Dashboard
{

    [EnableCors("*", "*", "*")]
    [Authorize]
    public class ManagerStatisticalController : BaseController
    {
        private readonly ICustomerOrderService _customerorderservice;
        private readonly IOrderProductService _orderproductservice;
        private readonly ICustomerGroupService _customergroupservice;
        private readonly ITransactionService _transactionservice;
        private readonly ISourceService _sourceservice;
        public ManagerStatisticalController() { }
        private readonly IMapper _mapper;
        public ManagerStatisticalController(ISourceService sourceservice,ITransactionService transactionservice,ICustomerOrderService customerorderservice, IOrderProductService orderproductservice, ICustomerGroupService customergroupservice, IMapper mapper)
        {
            this._customerorderservice = customerorderservice;
            this._orderproductservice = orderproductservice;
            this._customergroupservice = customergroupservice;
            this._transactionservice = transactionservice;
            this._sourceservice = sourceservice;
            this._mapper = mapper;
        }
        #region[Statistics by]
        [HttpGet]
        [Route("api/dashboards/statistics_by_product")]
        public IHttpActionResult GetProduct(int pageNumber, int pageSize,int staff_id, bool month, bool week, bool day)
        {
            ResponseDataDTO<PagedResults<customerorderviewmodel>> response = new ResponseDataDTO<PagedResults<customerorderviewmodel>>();
            try
            {
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;

                response.Data = _customerorderservice.ResultStatisticsCustomerOrder(pageNumber: pageNumber, pageSize: pageSize, staff_id, month, week, day, company_id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;
                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
        #endregion
        #region[Statistics by month]
        [HttpGet]
        [Route("api/dashboards/statistics-revenue")]
        public IHttpActionResult Getaddresss()
        {
            ResponseDataDTO<statisticsbyrevenueviewmodel> response = new ResponseDataDTO<statisticsbyrevenueviewmodel>();
            try
            {
                int staff_id = BaseController.get_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;

                response.Data = _customerorderservice.ResultStatisticsByRevenue(staff_id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;
                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
        [HttpGet]
        [Route("api/dashboards/statistic-revenue-by-month")]
        public IHttpActionResult ResultStatisticByMonth()
        {
            ResponseDataDTO<List<revenue>> response = new ResponseDataDTO<List<revenue>>();
            try
            {
                int staff_id = BaseController.get_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;

                response.Data = _customerorderservice.ResultStatisticByMonth(staff_id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;
                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
        [HttpGet]
        [Route("api/dashboards/statistics-order")]
        public IHttpActionResult StatisticsOrderProduct(int pageNumber, int pageSize,  bool month, bool week, bool day, string search_name)
        {
            ResponseDataDTO<PagedResults<statisticsorderviewmodel>> response = new ResponseDataDTO<PagedResults<statisticsorderviewmodel>>();
            try
            {
                int staff_id = BaseController.get_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _orderproductservice.ResultStatisticsOrder(pageNumber,pageSize,staff_id,month,week,day,search_name);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;
                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
        [HttpGet]
        [Route("api/dashboard/statistic-customer-group")]
        public IHttpActionResult GetRevenueCustomerGroup()
        {
            ResponseDataDTO<List<statisticrevenueviewmodel>> response = new ResponseDataDTO<List<statisticrevenueviewmodel>>();
            try
            {
                int staff_id = BaseController.get_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customergroupservice.GetRevenueCustomerGroup(staff_id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;
            }

            return Ok(response);
        }
        [HttpGet]
        [Route("api/dashboard/statistic-source")]
        public IHttpActionResult GetRevenueSource()
        {
            ResponseDataDTO<List<statisticrevenueviewmodel>> response = new ResponseDataDTO<List<statisticrevenueviewmodel>>();
            try
            {
                int staff_id = BaseController.get_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _sourceservice.GetRevenueSource(staff_id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;
            }

            return Ok(response);
        }
        [HttpGet]
        [Route("api/dashboard/statistic-transaction-rate")]
        public IHttpActionResult GetTransactionRate()
        {
            ResponseDataDTO<List<transactionstatisticrateviewmodel>> response = new ResponseDataDTO<List<transactionstatisticrateviewmodel>>();
            try
            {
                int staff_id = BaseController.get_id_current();
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _transactionservice.GetTransactionStatisticRate(staff_id,company_id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;
            }

            return Ok(response);
        }

        #endregion


    }
}