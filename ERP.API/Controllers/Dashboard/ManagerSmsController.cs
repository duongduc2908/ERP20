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
    [Authorize]
    public class ManagerSmsController : ApiController
    {
        private readonly ISmsService _smsservice;

        private readonly IMapper _mapper;

        public ManagerSmsController() { }
        public ManagerSmsController(ISmsService smsservice, IMapper mapper)
        {
            this._smsservice = smsservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/smss/all")]
        public IHttpActionResult Getsmss()
        {
            ResponseDataDTO<IEnumerable<sms>> response = new ResponseDataDTO<IEnumerable<sms>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _smsservice.GetAll();
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

        [Route("api/smss/page")]
        public IHttpActionResult GetsmssPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<sms>> response = new ResponseDataDTO<PagedResults<sms>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _smsservice.CreatePagedResults(pageNumber, pageSize);
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
        [Route("api/smss/create")]

        public async Task<IHttpActionResult> Createsms()
        {
            ResponseDataDTO<sms> response = new ResponseDataDTO<sms>();
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
                SmsCreateViewModel smsCreateViewModel = new SmsCreateViewModel
                {
                    sms_api_key = Convert.ToString(streamProvider.FormData["sms_api_key"]),
                    sms_secret_key = Convert.ToString(streamProvider.FormData["sms_secret_key"]),
                    sms_brand_name_code = Convert.ToString(streamProvider.FormData["sms_brand_name_code"]),
                    sms_call_back_url = Convert.ToString(streamProvider.FormData["sms_call_back_url"]),

                    company_id = Convert.ToInt32(streamProvider.FormData["company_id"]),

                };

                // mapping view model to entity
                var createdsms = _mapper.Map<sms>(smsCreateViewModel);


                // save new sms
                _smsservice.Create(createdsms);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createdsms;
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
        [Route("api/smss/update")]

        public async Task<IHttpActionResult> Updatesms(int? sms_id)
        {
            ResponseDataDTO<sms> response = new ResponseDataDTO<sms>();
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
                SmsUpdateViewModel smsUpdateViewModel = new SmsUpdateViewModel
                {
                    sms_id = Convert.ToInt32(streamProvider.FormData["sms_id"]),
                    sms_api_key = Convert.ToString(streamProvider.FormData["sms_api_key"]),
                    sms_secret_key = Convert.ToString(streamProvider.FormData["sms_secret_key"]),
                    sms_brand_name_code = Convert.ToString(streamProvider.FormData["sms_brand_name_code"]),
                    sms_call_back_url = Convert.ToString(streamProvider.FormData["sms_call_back_url"]),

                    company_id = Convert.ToInt32(streamProvider.FormData["company_id"]),

                };



                // mapping view model to entity
                var updatedsms = _mapper.Map<sms>(smsUpdateViewModel);



                // update sms
                _smsservice.Update(updatedsms, sms_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedsms;
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
        [Route("api/smss/delete/{smsId}")]
        public IHttpActionResult Deletesms(int smsId)
        {
            ResponseDataDTO<sms> response = new ResponseDataDTO<sms>();
            try
            {
                var smsDeleted = _smsservice.Find(smsId);
                if (smsDeleted != null)
                {
                    _smsservice.Delete(smsDeleted);

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
                _smsservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}