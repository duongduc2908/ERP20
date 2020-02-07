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
    public class ManagerEmailTemplateController : ApiController
    {
        private readonly IEmailTemplateService _email_templateservice;

        private readonly IMapper _mapper;

        public ManagerEmailTemplateController() { }
        public ManagerEmailTemplateController(IEmailTemplateService email_templateservice, IMapper mapper)
        {
            this._email_templateservice = email_templateservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/email-templates/all")]
        public IHttpActionResult Getemail_templates()
        {
            ResponseDataDTO<IEnumerable<email_template>> response = new ResponseDataDTO<IEnumerable<email_template>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _email_templateservice.GetAll();
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

        [Route("api/email-templates/page")]
        public IHttpActionResult Getemail_templatesPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<email_template>> response = new ResponseDataDTO<PagedResults<email_template>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _email_templateservice.CreatePagedResults(pageNumber, pageSize);
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
        [Route("api/email-templates/create")]

        public async Task<IHttpActionResult> Createemail_template()
        {
            ResponseDataDTO<email_template> response = new ResponseDataDTO<email_template>();
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
                EmailTemplateCreateViewModel email_templateCreateViewModel = new EmailTemplateCreateViewModel
                {
                    emt_code = Convert.ToString(streamProvider.FormData["emt_code"]),
                    emt_name = Convert.ToString(streamProvider.FormData["emt_name"]),
                    emt_content = Convert.ToString(streamProvider.FormData["emt_content"]),
                   
                    staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]),

                    emt_create_date = Convert.ToDateTime(streamProvider.FormData["emt_create_date"]),
                    

                };

                // mapping view model to entity
                var createdemail_template = _mapper.Map<email_template>(email_templateCreateViewModel);


                // save new email_template
                _email_templateservice.Create(createdemail_template);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createdemail_template;
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
        [Route("api/email-templates/update")]

        public async Task<IHttpActionResult> Updateemail_template(int? emt_id)
        {
            ResponseDataDTO<email_template> response = new ResponseDataDTO<email_template>();
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
                EmailTemplateUpdateViewModel email_templateUpdateViewModel = new EmailTemplateUpdateViewModel
                {
                    emt_id = Convert.ToInt32(streamProvider.FormData["emt_id"]),
                    emt_code = Convert.ToString(streamProvider.FormData["emt_code"]),
                    emt_name = Convert.ToString(streamProvider.FormData["emt_name"]),
                    emt_content = Convert.ToString(streamProvider.FormData["emt_content"]),

                    staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]),

                    emt_create_date = Convert.ToDateTime(streamProvider.FormData["emt_create_date"]),


                };



                // mapping view model to entity
                var updatedemail_template = _mapper.Map<email_template>(email_templateUpdateViewModel);



                // update email_template
                _email_templateservice.Update(updatedemail_template, emt_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedemail_template;
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
        [Route("api/email-templates/delete")]
        public IHttpActionResult Deleteemail_template(int email_templateId)
        {
            ResponseDataDTO<email_template> response = new ResponseDataDTO<email_template>();
            try
            {
                var email_templateDeleted = _email_templateservice.Find(email_templateId);
                if (email_templateDeleted != null)
                {
                    _email_templateservice.Delete(email_templateDeleted);

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
                _email_templateservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}