using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Statistics;
using ERP.Extension.Extensions;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;


namespace ERP.API.Controllers.Dashboard
{
    public class ManagerStatisticalController : BaseController
    {
        private readonly ICustomerOrderService _customerorderservice;
        private readonly IOrderProductService _orderproductservice;
        public ManagerStatisticalController() { }
        private readonly IMapper _mapper;
        public ManagerStatisticalController(ICustomerOrderService customerorderservice, IOrderProductService orderproductservice, IMapper mapper)
        {
            this._customerorderservice = customerorderservice;
            this._orderproductservice = orderproductservice;
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
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;

                response.Data = _customerorderservice.ResultStatisticsCustomerOrder(pageNumber:pageNumber, pageSize:pageSize, staff_id,month,week,day);
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
        public IHttpActionResult Getaddresss(int staff_id)
        {
            ResponseDataDTO<statisticsbyrevenueviewmodel> response = new ResponseDataDTO<statisticsbyrevenueviewmodel>();
            try
            {
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
        #endregion


    }
}