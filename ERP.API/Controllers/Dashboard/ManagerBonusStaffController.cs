using ERP.Data.Dto;
using AutoMapper;
using ERP.Common.Constants;
using ERP.Data.ModelsERP;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;

using System.Web.Http;
using ERP.Data.ModelsERP.ModelView;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net;

namespace ERP.API.Controllers.Dashboard
{
    public class ManagerBonusStaffController : ApiController
    {
        // GET: ManagerCompany
        private readonly IBonusStaffService _bonusstaffservice;

        private readonly IMapper _mapper;

        public ManagerBonusStaffController() { }
        public ManagerBonusStaffController(IBonusStaffService bonusstaffservice, IMapper mapper)
        {
            this._bonusstaffservice = bonusstaffservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/bonus_staff/getall")]
        public IHttpActionResult GetAllName()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _bonusstaffservice.GetAllDropDown();
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
        /*
        [HttpGet]
        [Route("api/bonus_staff/search")]
        public IHttpActionResult Getcompanys(int pageNumber, int pageSize, string search_name)
        {
            ResponseDataDTO<PagedResults<bonus_staff>> response = new ResponseDataDTO<PagedResults<bonus_staff>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _bonusstaffservice.GetAllSearch(pageNumber, pageSize, search_name);
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
        [HttpGet]
        [Route("api/bonus_staff/get_by_id")]
        public IHttpActionResult GetById(int bos_id)
        {
            ResponseDataDTO<bonus_staff> response = new ResponseDataDTO<bonus_staff>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _bonusstaffservice.GetById(bos_id);
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
        */
        [HttpPost]
        [Route("api/bonus_staff/create")]

        public async Task<IHttpActionResult> Create()
        {
            ResponseDataDTO<bonus_staff> response = new ResponseDataDTO<bonus_staff>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);

                bonus_staff create_bonus_staff = new bonus_staff();
                create_bonus_staff.bos_content = Convert.ToString(streamProvider.FormData["bos_content"]);
                create_bonus_staff.bos_title = Convert.ToString(streamProvider.FormData["bos_title"]);
                create_bonus_staff.bos_note = Convert.ToString(streamProvider.FormData["bos_note"]);
                create_bonus_staff.bos_value = Convert.ToString(streamProvider.FormData["bos_value"]);
                create_bonus_staff.bos_type = Convert.ToInt32(streamProvider.FormData["bos_type"]);
                create_bonus_staff.bos_time = Convert.ToDateTime(streamProvider.FormData["bos_time"]);
                create_bonus_staff.bos_reason = Convert.ToString(streamProvider.FormData["bos_reason"]);
                create_bonus_staff.staff_id = BaseController.get_id_current();

                
                // save new bonus_staff
                _bonusstaffservice.Create(create_bonus_staff);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = create_bonus_staff;
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
        [Route("api/bonus_staff/update")]

        public async Task<IHttpActionResult> Update()
        {
            ResponseDataDTO<bonus_staff> response = new ResponseDataDTO<bonus_staff>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);
                int bos_id = Convert.ToInt32(streamProvider.FormData["bos_id"]);
                bonus_staff exists = _bonusstaffservice.Find(bos_id);
                exists.bos_content = Convert.ToString(streamProvider.FormData["bos_content"]);
                exists.bos_title = Convert.ToString(streamProvider.FormData["bos_title"]);
                exists.bos_note = Convert.ToString(streamProvider.FormData["bos_note"]);
                exists.bos_value = Convert.ToString(streamProvider.FormData["bos_value"]);
                exists.bos_type = Convert.ToInt32(streamProvider.FormData["bos_type"]);
                exists.bos_time = Convert.ToDateTime(streamProvider.FormData["bos_time"]);
                exists.bos_reason = Convert.ToString(streamProvider.FormData["bos_reason"]);


                // update bonus_staff
                _bonusstaffservice.Update(exists, exists.bos_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = exists;
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
        [Route("api/bonus_staff/delete")]
        public IHttpActionResult Deletebonus_staff(int bos_id)
        {
            ResponseDataDTO<bonus_staff> response = new ResponseDataDTO<bonus_staff>();
            try
            {
                var bonus_staffDelete = _bonusstaffservice.Find(bos_id);
                if (bonus_staffDelete != null)
                {
                    _bonusstaffservice.Delete(bonus_staffDelete);

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
    }
}