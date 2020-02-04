using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Extension.Extensions;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ERP.API.Controllers.Dashboard
{
    [EnableCors("*", "*", "*")]
    public class ManagerSmsStrategyController : ApiController
    {
        private readonly ISmsStrategyService _smsstrategyservice;

        private readonly IMapper _mapper;

        public ManagerSmsStrategyController() { }
        public ManagerSmsStrategyController(ISmsStrategyService sms_strategyservice, IMapper mapper)
        {
            this._smsstrategyservice = sms_strategyservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/sms-strategys/all")]
        public IHttpActionResult Getsms_strategys()
        {
            ResponseDataDTO<IEnumerable<sms_strategy>> response = new ResponseDataDTO<IEnumerable<sms_strategy>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _smsstrategyservice.GetAll();
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }

        [Route("api/sms-strategys/page")]
        public IHttpActionResult Getsms_strategysPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<sms_strategy>> response = new ResponseDataDTO<PagedResults<sms_strategy>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _smsstrategyservice.CreatePagedResults(pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("api/sms-strategys/create")]

        public async Task<IHttpActionResult> Createsms_strategy()
        {
            ResponseDataDTO<sms_strategy> response = new ResponseDataDTO<sms_strategy>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);

                // get data from formdata
                SmsStrategyCreateViewModel sms_strategyCreateViewModel = new SmsStrategyCreateViewModel
                {
                    smss_code = Convert.ToString(streamProvider.FormData["smss_code"]),
                    smss_title = Convert.ToString(streamProvider.FormData["smss_title"]),

                    smss_send_count = Convert.ToInt32(streamProvider.FormData["smss_send_count"]),
                    sms_id = Convert.ToInt32(streamProvider.FormData["sms_id"]),
                    sms_template_id = Convert.ToInt32(streamProvider.FormData["sms_template_id"]),
                    customer_group_id = Convert.ToInt32(streamProvider.FormData["customer_group_id"]),

                    smss_send_date = Convert.ToDateTime(streamProvider.FormData["smss_send_date"]),
                    smss_created_date = Convert.ToDateTime(streamProvider.FormData["smss_created_date"]),

                    smss_cost = Convert.ToDouble(streamProvider.FormData["smss_cost"]),



                };

                // mapping view model to entity
                var createdsms_strategy = _mapper.Map<sms_strategy>(sms_strategyCreateViewModel);


                // save new sms_strategy
                _smsstrategyservice.Create(createdsms_strategy);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createdsms_strategy;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }

        }




        [HttpPut]
        [Route("api/sms-strategys/update")]

        public async Task<IHttpActionResult> Updatesms_strategy(int? smss_id)
        {
            ResponseDataDTO<sms_strategy> response = new ResponseDataDTO<sms_strategy>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);


                // get data from formdata
                SmsStrategyUpdateViewModel sms_strategyUpdateViewModel = new SmsStrategyUpdateViewModel
                {
                    smss_id = Convert.ToInt32(streamProvider.FormData["smss_id"]),
                    smss_code = Convert.ToString(streamProvider.FormData["smss_code"]),
                    smss_title = Convert.ToString(streamProvider.FormData["smss_title"]),

                    smss_send_count = Convert.ToInt32(streamProvider.FormData["smss_send_count"]),
                    sms_id = Convert.ToInt32(streamProvider.FormData["sms_id"]),
                    sms_template_id = Convert.ToInt32(streamProvider.FormData["sms_template_id"]),
                    customer_group_id = Convert.ToInt32(streamProvider.FormData["customer_group_id"]),

                    smss_send_date = Convert.ToDateTime(streamProvider.FormData["smss_send_date"]),
                    smss_created_date = Convert.ToDateTime(streamProvider.FormData["smss_created_date"]),

                    smss_cost = Convert.ToDouble(streamProvider.FormData["smss_cost"]),

                };



                // mapping view model to entity
                var updatedsms_strategy = _mapper.Map<sms_strategy>(sms_strategyUpdateViewModel);



                // update sms_strategy
                _smsstrategyservice.Update(updatedsms_strategy, smss_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedsms_strategy;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }

        [HttpDelete]
        [Route("api/sms-strategys/delete/{sms_strategyId}")]
        public IHttpActionResult Deletesms_strategy(int sms_strategyId)
        {
            ResponseDataDTO<sms_strategy> response = new ResponseDataDTO<sms_strategy>();
            try
            {
                var sms_strategyDeleted = _smsstrategyservice.Find(sms_strategyId);
                if (sms_strategyDeleted != null)
                {
                    _smsstrategyservice.Delete(sms_strategyDeleted);

                    // return response
                    response.Code = HttpCode.OK;
                    response.Message = MessageResponse.SUCCESS;
                    response.Data = null;
                    return Ok(response);
                }
                else
                {
                    // return response
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = MessageResponse.FAIL;
                    response.Data = null;

                    return Ok(response);
                }


            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }
        #endregion

        #region dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _smsstrategyservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}