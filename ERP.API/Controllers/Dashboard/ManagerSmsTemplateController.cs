using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView.Sms;
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
    public class ManagerSmsTemplateController : ApiController
    {
        private readonly ISmsTemplateService _smstemplateservice;

        private readonly IMapper _mapper;

        public ManagerSmsTemplateController() { }
        public ManagerSmsTemplateController(ISmsTemplateService smstemplateservice, IMapper mapper)
        {
            this._smstemplateservice = smstemplateservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/sms-templates/all")]
        public IHttpActionResult Getsms_templates()
        {
            ResponseDataDTO<IEnumerable<sms_template>> response = new ResponseDataDTO<IEnumerable<sms_template>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _smstemplateservice.GetAll();
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
        [Route("api/sms-templates/page-search")]
        public IHttpActionResult Getsms_templatesPaging(int pageSize, int pageNumber, string search_name)
        {
            ResponseDataDTO<PagedResults<smstemplatemodelview>> response = new ResponseDataDTO<PagedResults<smstemplatemodelview>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _smstemplateservice.CreatePagedResults(pageNumber, pageSize, search_name);
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
        [Route("api/smsstrategy-templates/search")]
        public IHttpActionResult Getsmsstrategy_templatesPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<smstemplatestrategyviewmodel>> response = new ResponseDataDTO<PagedResults<smstemplatestrategyviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _smstemplateservice.CreatePagedSmsTrategy(pageNumber, pageSize);
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
        [Route("api/sms-templates/create")]

        public async Task<IHttpActionResult> Createsms_template()
        {
            ResponseDataDTO<sms_template> response = new ResponseDataDTO<sms_template>();
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
                SmsTemplateCreateViewModel smstemplateCreateViewModel = new SmsTemplateCreateViewModel
                {
                    //smt_code = Convert.ToString(streamProvider.FormData["smt_code"]),
                    smt_title = Convert.ToString(streamProvider.FormData["smt_title"]),
                    smt_content = Convert.ToString(streamProvider.FormData["smt_content"]),
                    staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]),
                    smt_created_date = Convert.ToDateTime(streamProvider.FormData["smt_created_date"]),
                    
                };
                // mapping view model to entity
                var createdsms_template = _mapper.Map<sms_template>(smstemplateCreateViewModel);

                // create smt_code
                var x = _smstemplateservice.GetLast();
                createdsms_template.smt_code = Utilis.CreateCode("SMS", x.smt_id, 7);
                // save new sms_template
                _smstemplateservice.Create(createdsms_template);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createdsms_template;
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
        [Route("api/sms-templates/update")]

        public async Task<IHttpActionResult> Updatesms_template()
        {
            ResponseDataDTO<sms_template> response = new ResponseDataDTO<sms_template>();
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
                SmsTemplateUpdateViewModel smstemplateUpdateViewModel = new SmsTemplateUpdateViewModel
                {
                    smt_id = Convert.ToInt32(streamProvider.FormData["smt_id"]),
                    smt_code = Convert.ToString(streamProvider.FormData["smt_code"]),
                    smt_title = Convert.ToString(streamProvider.FormData["smt_title"]),
                    smt_content = Convert.ToString(streamProvider.FormData["smt_content"]),

                    staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]),

                    smt_created_date = Convert.ToDateTime(streamProvider.FormData["smt_created_date"]),

                };
                if( smstemplateUpdateViewModel.smt_id == null)
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Khong tim thay ma sms trong csdl!";
                    response.Data = null;
                    return Ok(response);
                }
                // mapping view model to entity
                var updatedsms_template = _mapper.Map<sms_template>(smstemplateUpdateViewModel);
                // update sms_template
                _smstemplateservice.Update(updatedsms_template, updatedsms_template.smt_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedsms_template;
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
        [Route("api/sms-templates/delete")]
        public IHttpActionResult Deletesms_template(int sms_templateId)
        {
            ResponseDataDTO<sms_template> response = new ResponseDataDTO<sms_template>();
            try
            {
                var sms_templateDeleted = _smstemplateservice.Find(sms_templateId);
                if (sms_templateDeleted != null)
                {
                    _smstemplateservice.Delete(sms_templateDeleted);

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
                _smstemplateservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}