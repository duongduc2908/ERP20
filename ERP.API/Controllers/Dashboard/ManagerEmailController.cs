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
    public class ManagerEmailController : ApiController
    {
        private readonly IEmailService _emailservice;

        private readonly IMapper _mapper;

        public ManagerEmailController() { }
        public ManagerEmailController(IEmailService emailservice, IMapper mapper)
        {
            this._emailservice = emailservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/emails/all")]
        public IHttpActionResult Getemails()
        {
            ResponseDataDTO<IEnumerable<email>> response = new ResponseDataDTO<IEnumerable<email>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _emailservice.GetAll();
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
        [Route("api/emails/page")]
        public IHttpActionResult GetemailsPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<email>> response = new ResponseDataDTO<PagedResults<email>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _emailservice.CreatePagedResults(pageNumber, pageSize);
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
        [Route("api/emails/create")]

        public async Task<IHttpActionResult> Createemail()
        {
            ResponseDataDTO<email> response = new ResponseDataDTO<email>();
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
                EmailCreateViewModel emailCreateViewModel = new EmailCreateViewModel
                {
                    ema_username = Convert.ToString(streamProvider.FormData["ema_username"]),
                    ema_password = Convert.ToString(streamProvider.FormData["ema_password"]),
                    ema_api = Convert.ToString(streamProvider.FormData["ema_api"]),
                    ema_pop_or_imap_server = Convert.ToString(streamProvider.FormData["ema_pop_or_imap_server"]),
                    ema_smtp_server = Convert.ToString(streamProvider.FormData["ema_smtp_server"]),
                    ema_note = Convert.ToString(streamProvider.FormData["ema_note"]),


                    company_id = Convert.ToInt32(streamProvider.FormData["company_id"]),
                    ema_pop_or_imap_port = Convert.ToInt32(streamProvider.FormData["ema_pop_or_imap_port"]),
                    ema_smtp_port = Convert.ToInt32(streamProvider.FormData["ema_smtp_port"]),
                 
                };

                // mapping view model to entity
                var createdemail = _mapper.Map<email>(emailCreateViewModel);


                // save new email
                _emailservice.Create(createdemail);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createdemail;
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
        [Route("api/emails/update")]

        public async Task<IHttpActionResult> Updateemail(int? ema_id)
        {
            ResponseDataDTO<email> response = new ResponseDataDTO<email>();
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
                EmailUpdateViewModel emailUpdateViewModel = new EmailUpdateViewModel
                {
                    ema_id = Convert.ToInt32(streamProvider.FormData["ema_id"]),
                    ema_username = Convert.ToString(streamProvider.FormData["ema_username"]),
                    ema_password = Convert.ToString(streamProvider.FormData["ema_password"]),
                    ema_api = Convert.ToString(streamProvider.FormData["ema_api"]),
                    ema_pop_or_imap_server = Convert.ToString(streamProvider.FormData["ema_pop_or_imap_server"]),
                    ema_smtp_server = Convert.ToString(streamProvider.FormData["ema_smtp_server"]),
                    ema_note = Convert.ToString(streamProvider.FormData["ema_note"]),


                    company_id = Convert.ToInt32(streamProvider.FormData["company_id"]),
                    ema_pop_or_imap_port = Convert.ToInt32(streamProvider.FormData["ema_pop_or_imap_port"]),
                    ema_smtp_port = Convert.ToInt32(streamProvider.FormData["ema_smtp_port"]),

                };



                // mapping view model to entity
                var updatedemail = _mapper.Map<email>(emailUpdateViewModel);



                // update email
                _emailservice.Update(updatedemail, ema_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedemail;
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
        [Route("api/emails/delete")]
        public IHttpActionResult Deleteemail(int emailId)
        {
            ResponseDataDTO<email> response = new ResponseDataDTO<email>();
            try
            {
                var emailDeleted = _emailservice.Find(emailId);
                if (emailDeleted != null)
                {
                    _emailservice.Delete(emailDeleted);

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
                _emailservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}