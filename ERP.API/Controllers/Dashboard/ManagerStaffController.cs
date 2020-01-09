using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.Identity;
using ERP.Data.ModelsERP;
using ERP.Extension.Extensions;
using ERP.Service.Services.IServices;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ERP.API.Controllers.Dashboard
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ManagerstaffsController : ApiController
    {
        private readonly IStaffService _staffservice;

        private readonly IMapper _mapper;
        public ManagerstaffsController(IStaffService staffservice, IMapper mapper)
        {
            this._staffservice = staffservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/staffs/all")]
        public IHttpActionResult Getstaffs()
        {
            ResponseDataDTO<IEnumerable<staff>> response = new ResponseDataDTO<IEnumerable<staff>>();
            try
            {
                AppIdentityClaims current = new AppIdentityClaims((ClaimsIdentity)User.Identity);
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _staffservice.GetAll();
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

        [Route("api/staffs/page")]
        public IHttpActionResult GetstaffsPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<staff>> response = new ResponseDataDTO<PagedResults<staff>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _staffservice.CreatePagedResults(pageNumber, pageSize);
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
        [Route("api/staffs/create")]
        public async Task<IHttpActionResult> Createstaff()
        {
            ResponseDataDTO<int> response = new ResponseDataDTO<int>();
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
                StaffCreateViewModel StaffCreateViewModel = new StaffCreateViewModel
                {
                    sta_fullname = Convert.ToString(streamProvider.FormData["sta_fullname"])
                };
                // mapping view model to entity
                var createdstaff = _mapper.Map<staff>(StaffCreateViewModel);
                createdstaff.sta_thumbnai = fileName;

                //gia su staffhumail của anh bây giờ có 3 ảnh.
                //Upload vẫn giữ nguyên hàm đó thì xử lý của e là gì ?

                // save new staff
                _staffservice.Create(createdstaff);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = 1;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = 0;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }

        }


        [HttpPut]
        [Route("api/staffs/update")]
        public async Task<IHttpActionResult> Updatestaff()
        {
            ResponseDataDTO<int> response = new ResponseDataDTO<int>();
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
                StaffUpdateViewModel staffUpdateViewModel = new StaffUpdateViewModel
                {
                    sta_code = Convert.ToString(streamProvider.FormData["sta_code"])
                };
                var existstaff = _staffservice.Find(staffUpdateViewModel.sta_code);
                if (fileName != "")
                {
                    staffUpdateViewModel.sta_thumbnai = fileName;
                }
                else
                {

                    staffUpdateViewModel.sta_thumbnai = existstaff.sta_thumbnai;
                }
                // mapping view model to entity
                var updatedstaff = _mapper.Map<staff>(staffUpdateViewModel);

                // update staff
                _staffservice.Update(updatedstaff, updatedstaff.sta_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = 1;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = 0;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }

        [HttpDelete]
        [Route("api/staffs/delete/{staffId}")]
        public IHttpActionResult Deletestaff(int staffId)
        {
            ResponseDataDTO<int> response = new ResponseDataDTO<int>();
            try
            {
                var staffDeleted = _staffservice.Find(staffId);
                if (staffDeleted != null)
                {
                    _staffservice.Delete(staffDeleted);

                    // return response
                    response.Code = HttpCode.OK;
                    response.Message = MessageResponse.SUCCESS;
                    response.Data = 1;
                    return Ok(response);
                }
                else
                {
                    // return response
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = MessageResponse.FAIL;
                    response.Data = 0;

                    return Ok(response);
                }


            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = 0;
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
                _staffservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
