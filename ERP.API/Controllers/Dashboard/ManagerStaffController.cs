using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Extension.Extensions;
using ERP.Service.Services.IServices;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
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
using System.Web.Security;

namespace ERP.API.Controllers.Dashboard
{
    [EnableCors("*", "*", "*")]
    [Authorize]
    public class ManagerstaffsController : ApiController
    {
        private readonly IStaffService _staffservice;

        private readonly IMapper _mapper;

        public ManagerstaffsController() { }
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
            ResponseDataDTO<PagedResults<staffviewmodel>> response = new ResponseDataDTO<PagedResults<staffviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _staffservice.GetAllPage(pageNumber, pageSize);
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
        [Route("api/staffs/infor")]
        public IHttpActionResult GetInforById(int id)
        {
            ResponseDataDTO<PagedResults<staffviewmodel>> response = new ResponseDataDTO<PagedResults<staffviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _staffservice.GetInforById(id);
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
            ResponseDataDTO<staff> response = new ResponseDataDTO<staff>();
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
                    sta_fullname = Convert.ToString(streamProvider.FormData["sta_fullname"]),
                    sta_code = Convert.ToString(streamProvider.FormData["sta_code"]),
                    sta_username = Convert.ToString(streamProvider.FormData["sta_username"]),
                    sta_password = Convert.ToString(streamProvider.FormData["sta_password"]),
                    sta_email = Convert.ToString(streamProvider.FormData["sta_email"]),

                    sta_aboutme = Convert.ToString(streamProvider.FormData["sta_aboutme"]),
                    sta_mobile = Convert.ToString(streamProvider.FormData["sta_mobile"]),
                    sta_identity_card = Convert.ToString(streamProvider.FormData["sta_identity_card"]),
                    sta_address = Convert.ToString(streamProvider.FormData["sta_address"]),
                    sta_hometown = Convert.ToString(streamProvider.FormData["sta_hometown"]),

                    department_id = Convert.ToInt32(streamProvider.FormData["department_id"]),
                    group_role_id = Convert.ToInt32(streamProvider.FormData["group_role_id"]),
                    social_id = Convert.ToInt32(streamProvider.FormData["social_id"]),
                    position_id = Convert.ToInt32(streamProvider.FormData["position_id"]),
                    sta_leader_id = Convert.ToInt32(streamProvider.FormData["sta_leader_id"]),


                    sta_birthday = Convert.ToDateTime(streamProvider.FormData["sta_birthday"]),
                    sta_identity_card_date = Convert.ToDateTime(streamProvider.FormData["sta_identity_card_date"]),
                    sta_created_date = Convert.ToDateTime(streamProvider.FormData["sta_created_date"]),

                    sta_status = Convert.ToByte(streamProvider.FormData["sta_status"]),
                    sta_sex = Convert.ToByte(streamProvider.FormData["sta_sex"]),



                };
                //md5

                if (CheckEmail.IsValidEmail(StaffCreateViewModel.sta_email) == false && StaffCreateViewModel.sta_email == "")
                {
                    response.Message = "Định dạng email không hợp lệ !";
                    response.Data = null;
                    return Ok(response);
                }
                //check_phone_number
                if (CheckNumber.IsPhoneNumber(StaffCreateViewModel.sta_mobile) == false && StaffCreateViewModel.sta_mobile == "")
                {
                    response.Message = "Số điện thoại không hợp lệ";
                    response.Data = null;
                    return Ok(response);
                }
                // mapping view model to entity
                var createdstaff = _mapper.Map<staff>(StaffCreateViewModel);
                createdstaff.sta_thumbnai = fileName;
                createdstaff.sta_password = HashMd5.convertMD5(StaffCreateViewModel.sta_password);

                // save new staff
                _staffservice.Create(createdstaff);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createdstaff;
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
        [Route("api/staffs/update")]
        public async Task<IHttpActionResult> Updatestaff(int? sta_id)
        {
            ResponseDataDTO<staff> response = new ResponseDataDTO<staff>();
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
                    sta_id = Convert.ToInt32(streamProvider.FormData["sta_id"]),

                    sta_fullname = Convert.ToString(streamProvider.FormData["sta_fullname"]),
                    sta_code = Convert.ToString(streamProvider.FormData["sta_code"]),
                    sta_username = Convert.ToString(streamProvider.FormData["sta_username"]),
                    sta_password = Convert.ToString(streamProvider.FormData["sta_password"]),
                    sta_email = Convert.ToString(streamProvider.FormData["sta_email"]),

                    sta_aboutme = Convert.ToString(streamProvider.FormData["sta_aboutme"]),
                    sta_mobile = Convert.ToString(streamProvider.FormData["sta_mobile"]),
                    sta_identity_card = Convert.ToString(streamProvider.FormData["sta_identity_card"]),
                    sta_address = Convert.ToString(streamProvider.FormData["sta_address"]),
                    sta_hometown = Convert.ToString(streamProvider.FormData["sta_hometown"]),

                    department_id = Convert.ToInt32(streamProvider.FormData["department_id"]),
                    group_role_id = Convert.ToInt32(streamProvider.FormData["group_role_id"]),
                    social_id = Convert.ToInt32(streamProvider.FormData["social_id"]),
                    position_id = Convert.ToInt32(streamProvider.FormData["position_id"]),
                    sta_leader_id = Convert.ToInt32(streamProvider.FormData["sta_leader_id"]),


                    sta_birthday = Convert.ToDateTime(streamProvider.FormData["sta_birthday"]),
                    sta_identity_card_date = Convert.ToDateTime(streamProvider.FormData["sta_identity_card_date"]),
                    sta_created_date = Convert.ToDateTime(streamProvider.FormData["sta_created_date"]),

                    sta_status = Convert.ToByte(streamProvider.FormData["sta_status"]),
                    sta_sex = Convert.ToByte(streamProvider.FormData["sta_sex"]),

                };


                var existstaff = _staffservice.Find(sta_id);

                if (fileName != "")
                {
                    staffUpdateViewModel.sta_thumbnai = fileName;
                }
                else
                {

                    staffUpdateViewModel.sta_thumbnai = existstaff.sta_thumbnai;
                }
                //md5

                if (CheckEmail.IsValidEmail(staffUpdateViewModel.sta_email) == false && staffUpdateViewModel.sta_email == "")
                {
                    response.Message = "Định dạng email không hợp lệ !";
                    response.Data = null;
                    return Ok(response);
                }
                //check_phone_number
                if (CheckNumber.IsPhoneNumber(staffUpdateViewModel.sta_mobile) == false && staffUpdateViewModel.sta_mobile == "")
                {
                    response.Message = "Số điện thoại không hợp lệ";
                    response.Data = null;
                    return Ok(response);
                }
                // mapping view model to entity
                var updatedstaff = _mapper.Map<staff>(staffUpdateViewModel);



                // update staff
                _staffservice.Update(updatedstaff, sta_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedstaff;
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
        [Route("api/staffs/delete/{staffId}")]
        public IHttpActionResult Deletestaff(int staffId)
        {
            ResponseDataDTO<staff> response = new ResponseDataDTO<staff>();
            try
            {
                var staffDeleted = _staffservice.Find(staffId);
                if (staffDeleted != null)
                {
                    _staffservice.Delete(staffDeleted);

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

        [HttpPut]
        [Route("api/staffs/ChangePassword")]
        public async Task<IHttpActionResult> ChangePasswordTest(ERP.Data.ChangePasswordBindingModel model, int id)
        {
            ResponseDataDTO<bool> response = new ResponseDataDTO<bool>();
            try
            {
                if( model.NewPassword != model.ConfirmPassword)
                {
                    response.Code = HttpCode.OK;
                    response.Message = "ConfirmPassword not true";
                    response.Data = false;
                    return Ok(response);
                }
                _staffservice.ChangePassword(model, id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = false;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }

        [Route("api/staffs/Logout")]
        public IHttpActionResult Logout()
        {
            ResponseDataDTO<bool> response = new ResponseDataDTO<bool>();
            try
            {
                FormsAuthentication.SignOut();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = false;
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
