using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
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
        public ManagerStatisticalController() { }
        private readonly IMapper _mapper;
        public ManagerStatisticalController(ICustomerOrderService customerorderservice, IMapper mapper)
        {
            this._customerorderservice = customerorderservice;
            this._mapper = mapper;
        }
        #region[Statistics by month]
        [HttpGet]
        [Route("api/dashboards/statistics_by_month")]
        public IHttpActionResult Getaddresss(int staff_id)
        {
            ResponseDataDTO<int> response = new ResponseDataDTO<int>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;

                response.Data = _customerorderservice.ResultStatisticsByMonth(staff_id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = 0;
                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
        #endregion
        #region[Statistics by]
        [HttpGet]
        [Route("api/dashboards/statistics_by_product")]
        public IHttpActionResult GetProduct(int staff_id, bool month, bool week, bool day)
        {
            ResponseDataDTO<int> response = new ResponseDataDTO<int>();
            try
            {
                DateTime datetimesearch = new DateTime();
                if (month) datetimesearch = Utilis.GetFirstDayOfMonth(DateTime.Now);
                if (week) datetimesearch = Utilis.GetFirstDayOfWeek(DateTime.Now); 
                if (day) datetimesearch = DateTime.Now;

                var x = _customerorderservice.GetAllIncluing(i => i.cuo_date <= DateTime.Now && i.cuo_date >= datetimesearch);
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;

                response.Data = _customerorderservice.ResultStatisticsByMonth(staff_id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = 0;
                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
        #endregion
        #region[Statistics by week]
        [HttpGet]
        [Route("api/dashboards/statistics_by_week")]
        public IHttpActionResult GetWeek(int staff_id)
        {
            ResponseDataDTO<int> response = new ResponseDataDTO<int>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;

                response.Data = _customerorderservice.ResultStatisticsByWeek(staff_id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = 0;
                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
        #endregion
        #region[Statistics by day]
        [HttpGet]
        [Route("api/dashboards/statistics_by_week")]
        public IHttpActionResult GetDay(int staff_id)
        {
            ResponseDataDTO<int> response = new ResponseDataDTO<int>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;

                response.Data = _customerorderservice.ResultStatisticsByDay(staff_id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = 0;
                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
        #endregion
    }
}