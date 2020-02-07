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
    public class ManagerDepartmentController  : ApiController
    {
        private readonly IDepartmentService _departmentservice;

        private readonly IMapper _mapper;

        public ManagerDepartmentController() { }
        public ManagerDepartmentController(IDepartmentService departmentservice, IMapper mapper)
        {
            this._departmentservice = departmentservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/departments/all")]
        public IHttpActionResult Getdepartments()
        {
            ResponseDataDTO<IEnumerable<department>> response = new ResponseDataDTO<IEnumerable<department>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _departmentservice.GetAll();
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }

        [Route("api/departments/page")]
        public IHttpActionResult GetdepartmentsPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<department>> response = new ResponseDataDTO<PagedResults<department>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _departmentservice.CreatePagedResults(pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("api/departments/create")]

        public async Task<IHttpActionResult> Createdepartment()
        {
            ResponseDataDTO<department> response = new ResponseDataDTO<department>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);
                // save file
                string fileName = "";
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    fileName = (FileExtension.SaveFileOnDisk(fileData));
                }
                // get data from formdata
                DepartmentCreateViewModel departmentCreateViewModel = new DepartmentCreateViewModel
                {
                    de_name = Convert.ToString(streamProvider.FormData["de_name"]),
                    de_description = Convert.ToString(streamProvider.FormData["de_description"]),
                    de_manager = Convert.ToString(streamProvider.FormData["de_manager"]),

                    company_id = Convert.ToByte(streamProvider.FormData["company_id"]),



                };

                // mapping view model to entity
                var createddepartment = _mapper.Map<department>(departmentCreateViewModel);
                createddepartment.de_thumbnail = fileName;

                // save new department
                _departmentservice.Create(createddepartment);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createddepartment;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }

        }




        [HttpPut]
        [Route("api/departments/update")]

        public async Task<IHttpActionResult> Updatedepartment(int? de_id)
        {
            ResponseDataDTO<department> response = new ResponseDataDTO<department>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);

                // save file
                string fileName = "";
                if (streamProvider.FileData.Count > 0)
                {
                    foreach (MultipartFileData fileData in streamProvider.FileData)
                    {
                        fileName = FileExtension.SaveFileOnDisk(fileData);
                    }
                }
                // get data from formdata
                DepartmentUpdateViewModel departmentUpdateViewModel = new DepartmentUpdateViewModel
                {
                    de_id = Convert.ToInt32(streamProvider.FormData["de_id"]),

                    de_name = Convert.ToString(streamProvider.FormData["de_name"]),
                    de_description = Convert.ToString(streamProvider.FormData["de_description"]),
                    de_manager = Convert.ToString(streamProvider.FormData["de_manager"]),

                    company_id = Convert.ToByte(streamProvider.FormData["company_id"]),

                };
                var existstaff = _departmentservice.Find(de_id);
                if (fileName != "")
                {
                    departmentUpdateViewModel.de_thumbnail = fileName;
                }
                else
                {

                    departmentUpdateViewModel.de_thumbnail = existstaff.de_thumbnail;
                }

                // mapping view model to entity
                var updateddepartment = _mapper.Map<department>(departmentUpdateViewModel);



                // update department
                _departmentservice.Update(updateddepartment, de_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updateddepartment;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }

        [HttpDelete]
        [Route("api/departments/delete")]
        public IHttpActionResult Deletedepartment(int departmentId)
        {
            ResponseDataDTO<department> response = new ResponseDataDTO<department>();
            try
            {
                var departmentDeleted = _departmentservice.Find(departmentId);
                if (departmentDeleted != null)
                {
                    _departmentservice.Delete(departmentDeleted);

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
                response.Message = ex.Message;;
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
                _departmentservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}