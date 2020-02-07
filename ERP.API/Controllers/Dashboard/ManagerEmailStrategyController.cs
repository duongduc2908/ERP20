using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Extension.Extensions;
using ERP.Service.Services;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ERP.API.Controllers.Dashboard
{
    [EnableCors("*", "*", "*")]
    [Authorize]
    public class ManagerEmailStrategyController : ApiController
    {
        private readonly IEmailStrategyService _email_strategyservice;

        private readonly IMapper _mapper;

        public ManagerEmailStrategyController() { }
        public ManagerEmailStrategyController(IEmailStrategyService email_strategyservice, IMapper mapper)
        {
            this._email_strategyservice = email_strategyservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/email-strategys/all")]
        public IHttpActionResult Getemail_strategys()
        {
            ResponseDataDTO<IEnumerable<email_strategy>> response = new ResponseDataDTO<IEnumerable<email_strategy>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _email_strategyservice.GetAll();
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

        [Route("api/email-strategys/page")]
        public IHttpActionResult Getemail_strategysPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<email_strategy>> response = new ResponseDataDTO<PagedResults<email_strategy>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _email_strategyservice.CreatePagedResults(pageNumber, pageSize);
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

        [HttpPost]
        [Route("api/email-strategys/create")]

        public async Task<IHttpActionResult> Createemail_strategy()
        {
            ResponseDataDTO<email_strategy> response = new ResponseDataDTO<email_strategy>();
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
                EmailStrategyCreateViewModel emailstrategyCreateViewModel = new EmailStrategyCreateViewModel
                {
                    ems_code = Convert.ToString(streamProvider.FormData["ems_code"]),
                    ems_name = Convert.ToString(streamProvider.FormData["ems_name"]),

                    ems_send_count = Convert.ToInt32(streamProvider.FormData["ems_send_count"]),
                    ems_click_count = Convert.ToInt32(streamProvider.FormData["ems_click_count"]),
                    ems_recevied_count = Convert.ToInt32(streamProvider.FormData["ems_recevied_count"]),
                    ems_open_count = Convert.ToInt32(streamProvider.FormData["ems_open_count"]),
                    email_id = Convert.ToInt32(streamProvider.FormData["email_id"]),
                    email_template_id = Convert.ToInt32(streamProvider.FormData["email_template_id"]),
                    customer_group_id = Convert.ToInt32(streamProvider.FormData["customer_group_id"]),

                    ems_send_date = Convert.ToDateTime(streamProvider.FormData["ems_send_date"]),
                    ems_create_date = Convert.ToDateTime(streamProvider.FormData["ems_create_date"]),
                   
                    ems_type = Convert.ToByte(streamProvider.FormData["ems_type"]),
                    ems_cost = Convert.ToDouble(streamProvider.FormData["ems_cost"]),



                };

                // mapping view model to entity
                var createdemail_strategy = _mapper.Map<email_strategy>(emailstrategyCreateViewModel);


                // save new email_strategy
                _email_strategyservice.Create(createdemail_strategy);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createdemail_strategy;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }

        }




        [HttpPut]
        [Route("api/email-strategys/update")]

        public async Task<IHttpActionResult> Updateemail_strategy(int? ems_id)
        {
            ResponseDataDTO<email_strategy> response = new ResponseDataDTO<email_strategy>();
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
                EmailStrategyUpdateViewModel emailstrategyUpdateViewModel = new EmailStrategyUpdateViewModel
                {
                    ems_id = Convert.ToInt32(streamProvider.FormData["ems_id"]),
                    ems_code = Convert.ToString(streamProvider.FormData["ems_code"]),
                    ems_name = Convert.ToString(streamProvider.FormData["ems_name"]),

                    ems_send_count = Convert.ToInt32(streamProvider.FormData["ems_send_count"]),
                    ems_click_count = Convert.ToInt32(streamProvider.FormData["ems_click_count"]),
                    ems_recevied_count = Convert.ToInt32(streamProvider.FormData["ems_recevied_count"]),
                    ems_open_count = Convert.ToInt32(streamProvider.FormData["ems_open_count"]),
                    email_id = Convert.ToInt32(streamProvider.FormData["email_id"]),
                    email_template_id = Convert.ToInt32(streamProvider.FormData["email_template_id"]),
                    customer_group_id = Convert.ToInt32(streamProvider.FormData["customer_group_id"]),

                    ems_send_date = Convert.ToDateTime(streamProvider.FormData["ems_send_date"]),
                    ems_create_date = Convert.ToDateTime(streamProvider.FormData["ems_create_date"]),

                    ems_type = Convert.ToByte(streamProvider.FormData["ems_type"]),
                    ems_cost = Convert.ToDouble(streamProvider.FormData["ems_cost"]),

                };



                // mapping view model to entity
                var updatedemail_strategy = _mapper.Map<email_strategy>(emailstrategyUpdateViewModel);



                // update email_strategy
                _email_strategyservice.Update(updatedemail_strategy, ems_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedemail_strategy;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }

        [HttpDelete]
        [Route("api/email-strategys/delete")]
        public IHttpActionResult Deleteemail_strategy(int email_strategyId)
        {
            ResponseDataDTO<email_strategy> response = new ResponseDataDTO<email_strategy>();
            try
            {
                var email_strategyDeleted = _email_strategyservice.Find(email_strategyId);
                if (email_strategyDeleted != null)
                {
                    _email_strategyservice.Delete(email_strategyDeleted);

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
                response.Message = ex.Message;
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
                _email_strategyservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}