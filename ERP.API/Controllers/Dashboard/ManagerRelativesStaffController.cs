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
    public class ManagerRelativesStaffController : ApiController
    {
        // GET: ManagerCompany
        private readonly IRelativesStaffService _relatives_staffservice;

        private readonly IMapper _mapper;

        public ManagerRelativesStaffController() { }
        public ManagerRelativesStaffController(IRelativesStaffService relatives_staffservice, IMapper mapper)
        {
            this._relatives_staffservice = relatives_staffservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/relatives_staff/getall")]
        public IHttpActionResult GetAllName()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _relatives_staffservice.GetAllDropDown();
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
        [Route("api/relatives_staff/search")]
        public IHttpActionResult Getcompanys(int pageNumber, int pageSize, string search_name)
        {
            ResponseDataDTO<PagedResults<relatives_staff>> response = new ResponseDataDTO<PagedResults<relatives_staff>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _relatives_staffservice.GetAllSearch(pageNumber, pageSize, search_name);
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
        [Route("api/relatives_staff/get_by_id")]
        public IHttpActionResult GetById(int rels_id)
        {
            ResponseDataDTO<relatives_staff> response = new ResponseDataDTO<relatives_staff>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _relatives_staffservice.GetById(rels_id);
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
        [Route("api/relatives_staff/create")]

        public async Task<IHttpActionResult> Create()
        {
            ResponseDataDTO<relatives_staff> response = new ResponseDataDTO<relatives_staff>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);

                relatives_staff create_relatives_staff = new relatives_staff();
                create_relatives_staff.rels_fullname = Convert.ToString(streamProvider.FormData["rels_fullname"]);
                create_relatives_staff.rels_phone = Convert.ToString(streamProvider.FormData["rels_phone"]);
                create_relatives_staff.rels_relatives = Convert.ToString(streamProvider.FormData["rels_relatives"]);
                create_relatives_staff.rels_address = Convert.ToString(streamProvider.FormData["rels_address"]);
                create_relatives_staff.staff_id = BaseController.get_id_current();

               
                // save new relatives_staff
                _relatives_staffservice.Create(create_relatives_staff);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = create_relatives_staff;
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
        [Route("api/relatives_staff/update")]

        public async Task<IHttpActionResult> Update()
        {
            ResponseDataDTO<relatives_staff> response = new ResponseDataDTO<relatives_staff>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);
                int rels_id = Convert.ToInt32(streamProvider.FormData["rels_id"]);
                relatives_staff exists = _relatives_staffservice.Find(rels_id);
                exists.rels_fullname = Convert.ToString(streamProvider.FormData["rels_fullname"]);
                exists.rels_phone = Convert.ToString(streamProvider.FormData["rels_phone"]);
                exists.rels_relatives = Convert.ToString(streamProvider.FormData["rels_relatives"]);
                exists.rels_address = Convert.ToString(streamProvider.FormData["rels_address"]);


                // update relatives_staff
                _relatives_staffservice.Update(exists, exists.rels_id);
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
        [Route("api/relatives_staff/delete")]
        public IHttpActionResult Deleterelatives_staff(int rels_id)
        {
            ResponseDataDTO<relatives_staff> response = new ResponseDataDTO<relatives_staff>();
            try
            {
                var relatives_staffDelete = _relatives_staffservice.Find(rels_id);
                if (relatives_staffDelete != null)
                {
                    _relatives_staffservice.Delete(relatives_staffDelete);

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