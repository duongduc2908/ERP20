using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Excel;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Extension.Extensions;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Security;

namespace ERP.API.Controllers.Dashboard
{
    [EnableCors("*", "*", "*")]
    [Authorize]
    public class ManagerstaffsController : BaseController
    {
        private readonly IStaffService _staffservice;
        private readonly IDepartmentService _departmentservice;
        private readonly IGroupRoleService _groupRoleservice;
        private readonly IPositionService _positionService;
        private readonly IUndertakenLocationService _undertakenlocationService;
        private readonly IMapper _mapper;

        public ManagerstaffsController() { }
        public ManagerstaffsController(IStaffService staffservice, IMapper mapper, IDepartmentService departmentService, IGroupRoleService groupRoleService, IPositionService positionService, IUndertakenLocationService undertakenlocationService)
        {
            this._staffservice = staffservice;
            this._mapper = mapper;
            this._departmentservice = departmentService;
            this._groupRoleservice = groupRoleService;
            this._positionService = positionService;
            this._undertakenlocationService = undertakenlocationService;
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
                response.Message = ex.Message;
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
                response.Message = ex.Message;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }

        [Route("api/staffs/search-active-name")]
        public IHttpActionResult GetstaffsPaging(int pageSize, int pageNumber, int? status, string name)
        {
            ResponseDataDTO<PagedResults<staffviewmodel>> response = new ResponseDataDTO<PagedResults<staffviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _staffservice.GetAllPageSearch(  pageNumber, pageSize, status,name);
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

        [Route("api/staffs/infor")]
        public IHttpActionResult GetInforById(int id)
        {
            ResponseDataDTO<staffviewmodel> response = new ResponseDataDTO<staffviewmodel>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _staffservice.GetInforById(id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = "Không tìm thấy nhân sự";
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }

        [Route("api/staffs/active")]
        public IHttpActionResult GetAllActive(int pageNumber, int pageSize, int status)
        {
            ResponseDataDTO<PagedResults<staffviewmodel>> response = new ResponseDataDTO<PagedResults<staffviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _staffservice.GetAllActive( pageNumber,pageSize,status);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = "Không tìm thấy nhân sự";
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }

        [Route("api/staffs/manager")]
        public IHttpActionResult GetInforManager()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _staffservice.GetInforManager();
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;

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
                #region["Check null"]

                if (streamProvider.FormData["sta_fullname"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Họ và tên không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["sta_username"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên đăng nhập không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (streamProvider.FormData["sta_mobile"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Số điện thoại không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (streamProvider.FormData["sta_status"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Trạng thái không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (streamProvider.FormData["department_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Phòng ban không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["sta_email"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Email không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (streamProvider.FormData["position_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Chức vụ không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                #endregion

                #region["Check exits"]
                if (!check_department(Convert.ToInt32(streamProvider.FormData["department_id"])))
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Khong co ma phong ban trong csdl!";
                    response.Data = null;
                    return Ok(response);
                }
                if (!check_position(Convert.ToInt32(Convert.ToInt32(streamProvider.FormData["position_id"]))))
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Khong co ma bo phan trong csdl!";
                    response.Data = null;
                    return Ok(response);
                }
                if (!check_grouprole(Convert.ToInt32(streamProvider.FormData["group_role_id"])))
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Khong co ma nhom quyen trong csdl!";
                    response.Data = null;
                    return Ok(response);
                }
                
                #endregion
                // get data from formdata những trường bắt buộc
                StaffCreateViewModel StaffCreateViewModel = new StaffCreateViewModel
                {
                    sta_fullname = Convert.ToString(streamProvider.FormData["sta_fullname"]),
                    //sta_code = Convert.ToString(streamProvider.FormData["sta_code"]),
                    sta_username = Convert.ToString(streamProvider.FormData["sta_username"]),
                    sta_email = Convert.ToString(streamProvider.FormData["sta_email"]),

                    sta_aboutme = Convert.ToString(streamProvider.FormData["sta_aboutme"]),
                    sta_mobile = Convert.ToString(streamProvider.FormData["sta_mobile"]),
                    sta_identity_card = Convert.ToString(streamProvider.FormData["sta_identity_card"]),
                    sta_address = Convert.ToString(streamProvider.FormData["sta_address"]),
                    sta_hometown = Convert.ToString(streamProvider.FormData["sta_hometown"]),
                    sta_reason_to_end_work = Convert.ToString(streamProvider.FormData["sta_reason_to_end_work"]),
                    sta_note = Convert.ToString(streamProvider.FormData["sta_note"]),

                    department_id = Convert.ToInt32(streamProvider.FormData["department_id"]),
                    group_role_id = Convert.ToInt32(streamProvider.FormData["group_role_id"]),
                    social_id = Convert.ToInt32(streamProvider.FormData["social_id"]),
                    position_id = Convert.ToInt32(streamProvider.FormData["position_id"]),
                    sta_leader_flag = Convert.ToByte(streamProvider.FormData["sta_leader_flag"]),




                    sta_status = Convert.ToByte(streamProvider.FormData["sta_status"]),
                    sta_sex = Convert.ToByte(streamProvider.FormData["sta_sex"]),
                };
                //Kiểm tra các trường rằng buộc
                if (check_username_email(StaffCreateViewModel.sta_username, StaffCreateViewModel.sta_email))
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Da co username '" + StaffCreateViewModel.sta_username+"' hoac email '"+ StaffCreateViewModel.sta_email+"' trong csdl";
                    response.Data = null;
                    return Ok(response);
                }
                //md5
                if (StaffCreateViewModel.sta_email != null)
                {
                    if (CheckEmail.IsValidEmail(StaffCreateViewModel.sta_email) == false)
                    {
                        response.Message = "Định dạng email không hợp lệ !";
                        response.Data = null;
                        return Ok(response);
                    }
                }

                //check_phone_number
                
               if (CheckNumber.IsPhoneNumber(StaffCreateViewModel.sta_mobile) == false)
               {
                   response.Message = "Số điện thoại không hợp lệ";
                   response.Data = null;
                   return Ok(response);
               }
               //Bắt các truongf còn lại 
                //check datetime

                if (streamProvider.FormData["sta_birthday"] == null)
                {
                    StaffCreateViewModel.sta_birthday = null;
                }
                else
                {
                    StaffCreateViewModel.sta_birthday = Convert.ToDateTime(streamProvider.FormData["sta_birthday"]);
                }

                if (streamProvider.FormData["sta_identity_card_date"] == null)
                {
                    StaffCreateViewModel.sta_identity_card_date = null;
                }
                else
                {
                    StaffCreateViewModel.sta_identity_card_date = Convert.ToDateTime(streamProvider.FormData["sta_identity_card_date"]);
                }
                if (streamProvider.FormData["sta_end_work_date"] == null)
                {
                    StaffCreateViewModel.sta_end_work_date = null;
                }
                else
                {
                    StaffCreateViewModel.sta_end_work_date = Convert.ToDateTime(streamProvider.FormData["sta_end_work_date"]);
                }
                if (streamProvider.FormData["sta_start_work_date"] == null)
                {
                    StaffCreateViewModel.sta_start_work_date = null;
                }
                else
                {
                    StaffCreateViewModel.sta_start_work_date = Convert.ToDateTime(streamProvider.FormData["sta_start_work_date"]);
                }


                if (streamProvider.FormData["sta_created_date"] == null)
                {
                    StaffCreateViewModel.sta_created_date = DateTime.Now;
                }
                //Lấy ra bản ghi cuối cùng tạo mã code 
                var x = _staffservice.GetLast();
                if(x == null) StaffCreateViewModel.sta_code = Utilis.CreateCode("NV", 0, 7);
                else StaffCreateViewModel.sta_code = Utilis.CreateCode("NV", x.sta_id, 7);
                var pass_new = Utilis.MakeRandomPassword(8);
                StaffCreateViewModel.sta_password = HashMd5.convertMD5(pass_new);
                // mapping view model to entity
                // save file
                string fileName = "";
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    fileName = (FileExtension.SaveFileStaffOnDisk(fileData, StaffCreateViewModel.sta_code));
                }
                var createdstaff = _mapper.Map<staff>(StaffCreateViewModel);
                if(fileName == null && createdstaff.sta_sex == 1)
                {
                    createdstaff.sta_thumbnai = "/Uploads/Images/default/girl.png";
                }
                else if (fileName == null && createdstaff.sta_sex == 0)
                {
                    createdstaff.sta_thumbnai = "/Uploads/Images/default/man.png";
                }
                else createdstaff.sta_thumbnai = fileName;
                createdstaff.sta_password = HashMd5.convertMD5( StaffCreateViewModel.sta_password);
                createdstaff.sta_login = true;
                // save new staff
                _staffservice.Create(createdstaff);
                BaseController.send_mail("New User In Coerp With Username: " + createdstaff.sta_username + " . Password: " + pass_new, createdstaff.sta_email, "New User Created!!!");
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createdstaff;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;

                return Ok(response);
            }

        }

        [HttpPut]
        [Route("api/staffs/update")]
        public async Task<IHttpActionResult> Updatestaff()
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
                if (streamProvider.FormData["sta_fullname"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Họ và tên không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["sta_username"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên đăng nhập không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (streamProvider.FormData["sta_mobile"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Số điện thoại không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (streamProvider.FormData["sta_status"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Trạng thái không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (streamProvider.FormData["department_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Phòng ban không được để trống";
                    response.Data = null;
                    return Ok(response);
                }


                if (streamProvider.FormData["position_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Chức vụ không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                // get data from formdata

                StaffUpdateViewModel staffUpdateViewModel = new StaffUpdateViewModel
                {
                    sta_id = Convert.ToInt32(streamProvider.FormData["sta_id"]),

                    sta_fullname = Convert.ToString(streamProvider.FormData["sta_fullname"]),
                    sta_code = Convert.ToString(streamProvider.FormData["sta_code"]),
                    sta_username = Convert.ToString(streamProvider.FormData["sta_username"]),
                    sta_email = Convert.ToString(streamProvider.FormData["sta_email"]),

                    sta_aboutme = Convert.ToString(streamProvider.FormData["sta_aboutme"]),
                    sta_mobile = Convert.ToString(streamProvider.FormData["sta_mobile"]),
                    sta_identity_card = Convert.ToString(streamProvider.FormData["sta_identity_card"]),
                    sta_address = Convert.ToString(streamProvider.FormData["sta_address"]),
                    sta_hometown = Convert.ToString(streamProvider.FormData["sta_hometown"]),
                    sta_reason_to_end_work = Convert.ToString(streamProvider.FormData["sta_reason_to_end_work"]),
                    sta_note = Convert.ToString(streamProvider.FormData["sta_note"]),

                    department_id = Convert.ToInt32(streamProvider.FormData["department_id"]),
                    group_role_id = Convert.ToInt32(streamProvider.FormData["group_role_id"]),
                    social_id = Convert.ToInt32(streamProvider.FormData["social_id"]),
                    position_id = Convert.ToInt32(streamProvider.FormData["position_id"]),
                    sta_leader_flag = Convert.ToByte(streamProvider.FormData["sta_leader_flag"]),




                    sta_status = Convert.ToByte(streamProvider.FormData["sta_status"]),
                    sta_sex = Convert.ToByte(streamProvider.FormData["sta_sex"]),

                };


                var existstaff = _staffservice.Find(staffUpdateViewModel.sta_id);

                if (streamProvider.FormData["cu_thumbnail"] != null)
                {
                  staffUpdateViewModel.sta_thumbnai = existstaff.sta_thumbnai;
                }


                //md5
                if (staffUpdateViewModel.sta_email != null)
                {
                    if (CheckEmail.IsValidEmail(staffUpdateViewModel.sta_email) == false)
                    {
                        response.Message = "Định dạng email không hợp lệ !";
                        response.Data = null;
                        return Ok(response);
                    }
                }
                else
                {
                    staffUpdateViewModel.sta_email = null;
                }
                //check_phone_number

                if (CheckNumber.IsPhoneNumber(staffUpdateViewModel.sta_mobile) == false)
                {
                    response.Message = "Số điện thoại không hợp lệ";
                    response.Data = null;
                    return Ok(response);
                }
                //Ma code
                staffUpdateViewModel.sta_code = existstaff.sta_code;

                //Address 
                if (streamProvider.FormData["sta_address"] == null)
                {
                    staffUpdateViewModel.sta_address = null;

                }
                // Option choose 
                if (streamProvider.FormData["sta_sex"] == null)
                {
                    if (existstaff.sta_sex != null)
                    {
                        staffUpdateViewModel.sta_sex = existstaff.sta_sex;
                    }
                    else
                    {
                        staffUpdateViewModel.sta_sex = null;
                    }
                }


                //checkdatetime
                if (streamProvider.FormData["sta_birthday"] == null)
                {
                    if (existstaff.sta_birthday != null)
                    {
                        staffUpdateViewModel.sta_birthday = existstaff.sta_birthday;
                    }
                    else
                    {
                        staffUpdateViewModel.sta_birthday = null;
                    }
                }
                else
                {
                    staffUpdateViewModel.sta_birthday = Convert.ToDateTime(streamProvider.FormData["sta_birthday"]);
                }
                if (streamProvider.FormData["sta_identity_card"] == null)
                {
                    staffUpdateViewModel.sta_identity_card = null;
                }
                else
                {
                    staffUpdateViewModel.sta_identity_card = Convert.ToString(streamProvider.FormData["sta_identity_card"]);
                }

                if (streamProvider.FormData["sta_identity_card_date"] == null)
                {
                    if (existstaff.sta_identity_card_date != null)
                    {
                        staffUpdateViewModel.sta_identity_card_date = existstaff.sta_identity_card_date;
                    }
                    else
                    {
                        staffUpdateViewModel.sta_identity_card_date = null;
                    }

                }
                else
                {
                    staffUpdateViewModel.sta_identity_card_date = Convert.ToDateTime(streamProvider.FormData["sta_identity_card_date"]);
                }
                if (streamProvider.FormData["sta_end_work_date"] == null)
                {
                    if (existstaff.sta_end_work_date != null)
                    {
                        staffUpdateViewModel.sta_end_work_date = existstaff.sta_end_work_date;
                    }
                    else
                    {
                        staffUpdateViewModel.sta_end_work_date = null;
                    }

                }
                else
                {
                    staffUpdateViewModel.sta_end_work_date = Convert.ToDateTime(streamProvider.FormData["sta_end_work_date"]);
                }

                if (streamProvider.FormData["sta_start_work_date"] == null)
                {
                    if (existstaff.sta_start_work_date != null)
                    {
                        staffUpdateViewModel.sta_start_work_date = existstaff.sta_start_work_date;
                    }
                    else
                    {
                        staffUpdateViewModel.sta_start_work_date = null;
                    }

                }
                else
                {
                    staffUpdateViewModel.sta_start_work_date = Convert.ToDateTime(streamProvider.FormData["sta_start_work_date"]);
                }
                staffUpdateViewModel.sta_code = existstaff.sta_code; 
                staffUpdateViewModel.sta_created_date = existstaff.sta_created_date;
                staffUpdateViewModel.sta_password = existstaff.sta_password;
                // mapping view model to entity
                var updatedstaff = _mapper.Map<staff>(staffUpdateViewModel);

                // update staff
                _staffservice.Update(updatedstaff, staffUpdateViewModel.sta_id);
                
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedstaff;
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
        [Route("api/staffs/delete")]
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
                response.Message = ex.Message;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }

        #endregion

        #region["Authentication"]
        [HttpPut]
        [Route("api/staffs/ChangePassword")]
        public async Task<IHttpActionResult> Change_Password(ERP.Data.ChangePasswordBindingModel model, int id)
        {
            ResponseDataDTO<bool> response = new ResponseDataDTO<bool>();
            try
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    response.Code = HttpCode.OK;
                    response.Message = "ConfirmPassword not true";
                    response.Data = false;
                    return Ok(response);
                }
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _staffservice.ChangePassword(model, id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
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
                response.Message = ex.Message;
                response.Data = false;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }
        #endregion

        #region["Update Avatar"]
        [HttpPut]
        [Route("api/staffs/update_avatar")]
        public async Task<IHttpActionResult> UpdateAvatar()
        {
            ResponseDataDTO<string> response = new ResponseDataDTO<string>();
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
                int staid = BaseController.get_id_current();
                var staff = _staffservice.Find(staid);
                if(staff == null)
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = MessageResponse.FAIL;
                    response.Data = null;
                    return Ok(response);
                }
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    fileName = FileExtension.SaveFileStaffOnDisk(fileData, staff.sta_code);
                }
                staff.sta_thumbnai = fileName;
                _staffservice.Update(staff, staid);
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = fileName;
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;
                return Ok(response);
            }
        }
        #endregion

        #region["Import Excel"]
        [HttpPost]
        [Route("api/satffs/import")]
        public async Task<IHttpActionResult> Import()
        {
            ResponseDataDTO<staff> response = new ResponseDataDTO<staff>();
            var exitsData = "";
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
                        fileName = fileData.Headers.ContentDisposition.FileName;
                        //fileName = fileName.Replace(@"","");

                        string fileFormat = Utilis.GetFileFormat(fileName);
                        if (fileFormat.Equals("xlsm") || fileFormat.Equals("xlsx"))
                        {
                            fileName = FileExtension.SaveFileStaffOnDiskExcel(fileData, "test",BaseController.folder(),BaseController.get_timestamp());
                        }
                        else
                        {
                            throw new Exception("File excel import không hợp lệ!");
                        }

                    }
                }
                var list = new List<staff>();
                fileName = "C:/inetpub/wwwroot/coerp" + fileName;
                //fileName = "D:/ERP20/ERP.API" + fileName;
                var dataset = ExcelImport.ImportExcelXLS(fileName, true);
                DataTable table = (DataTable)dataset.Tables[6];
                if (table != null && table.Rows.Count > 0)
                {
                    if (table.Columns.Count != 20)
                    {
                        exitsData = "File excel import không hợp lệ!";
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = exitsData;
                        response.Data = null;
                        return Ok(response);
                    }
                    else
                    {
                        #region["Check null"]
                        foreach (DataRow dr in table.Rows)
                        {
                            if (dr.IsNull("sta_fullname"))
                            {
                                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                response.Message = "Họ và tên không được để trống";
                                response.Data = null;
                                return Ok(response);
                            }
                            if (dr.IsNull("sta_username"))
                            {
                                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                response.Message = "Tên đăng nhập không được để trống";
                                response.Data = null;
                                return Ok(response);
                            }

                            if (dr.IsNull("sta_mobile"))
                            {
                                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                response.Message = "Số điện thoại không được để trống";
                                response.Data = null;
                                return Ok(response);
                            }

                            if (dr.IsNull("sta_status"))
                            {
                                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                response.Message = "Trạng thái không được để trống";
                                response.Data = null;
                                return Ok(response);
                            }

                            if (dr.IsNull("department_id"))
                            {
                                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                response.Message = "Phòng ban không được để trống";
                                response.Data = null;
                                return Ok(response);
                            }
                            if (dr.IsNull("position_id"))
                            {
                                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                response.Message = "Chức vụ không được để trống";
                                response.Data = null;
                                return Ok(response);
                            }
                        }
                        #endregion

                        #region["Check duplicate"]
                        for (var i = 0; i < table.Rows.Count; i++)
                        {
                            var usernameCur = table.Rows[i]["sta_username"].ToString().Trim();
                            var emailCur = table.Rows[i]["sta_email"].ToString().Trim();
                            for (var j = 0; j < table.Rows.Count; j++)
                            {
                                if (i != j)
                                {
                                    var _usernameCur = table.Rows[j]["sta_username"].ToString().Trim();
                                    var _emailCur = table.Rows[j]["sta_email"].ToString().Trim();
                                    if (usernameCur.Equals(_usernameCur))
                                    {
                                        exitsData = "Username '" + usernameCur + "' bị lặp trong file excel!";
                                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                        response.Message = exitsData;
                                        response.Data = null;
                                        return Ok(response);
                                    }
                                    if (emailCur.Equals(_emailCur))
                                    {
                                        exitsData = "Email '" + emailCur + "' bị lặp trong file excel!";
                                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                        response.Message = exitsData;
                                        response.Data = null;
                                        return Ok(response);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region["Check logic"]
                        foreach (DataRow dr in table.Rows)
                        {
                            int i = 1;
                            if (i == 2)
                            {
                                if (!check_department(Convert.ToInt32(dr["department_id"])))
                                {
                                    exitsData = "Khong co ma phong ban trong csdl!";
                                    response.Code = HttpCode.NOT_FOUND;
                                    response.Message = exitsData;
                                    response.Data = null;
                                    return Ok(response);
                                }
                                if (!check_position(Convert.ToInt32(dr["position_id"])))
                                {
                                    exitsData = "Khong co ma bo phan trong csdl!";
                                    response.Code = HttpCode.NOT_FOUND;
                                    response.Message = exitsData;
                                    response.Data = null;
                                    return Ok(response);
                                }
                                if (!check_grouprole(Convert.ToInt32(dr["group_role_id"])))
                                {
                                    exitsData = "Khong co ma nhom quyen trong csdl!";
                                    response.Code = HttpCode.NOT_FOUND;
                                    response.Message = exitsData;
                                    response.Data = null;
                                    return Ok(response);
                                }
                            }
                            i++;
                        }
                        #endregion
                    }
                    list = DataTableCmUtils.ToListof<staff>(table);
                    // Gọi hàm save data
                    foreach(staff i in list)
                    {
                        var x = _staffservice.GetLast();
                        if(x == null) i.sta_code = Utilis.CreateCode("NV", 0, 7);
                        else i.sta_code = Utilis.CreateCode("NV", x.sta_id, 7);
                        i.sta_password = Utilis.MakeRandomPassword(8);
                        if (i.sta_sex == 1)
                        {
                            i.sta_thumbnai = "/Uploads/Images/default/girl.png";
                        }
                        else
                        {
                            i.sta_thumbnai = "/Uploads/Images/default/man.png";
                        }
                        _staffservice.Create(i);
                        BaseController.send_mail(i.sta_password, i.sta_email, "New User Coerp");
                    }
                    exitsData = "Đã nhập dữ liệu excel thành công!";
                    response.Code = HttpCode.OK;
                    response.Message = exitsData;
                    response.Data = null;
                    return Ok(response);
                }
                else
                {
                    exitsData = "File excel import không có dữ liệu!";
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = exitsData;
                    response.Data = null;
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message; ;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }
        #endregion

        #region["Export Excel"]
        [HttpGet]
        [Route("api/satffs/export")]
        public async Task<IHttpActionResult> Export(int pageSize, int pageNumber, int? status, string name)
        {
            ResponseDataDTO<string> response = new ResponseDataDTO<string>();
            try
            {
                var listStaff = new List<staffview>();
                
                //Đưa ra danh sách staff trong trang nào đó 
                var objRT_Mst_Staff = _staffservice.ExportStaff(pageNumber, pageSize,status,name);
                if (objRT_Mst_Staff != null)
                {
                    listStaff.AddRange(objRT_Mst_Staff.Results);

                    Dictionary<string, string> dicColNames = GetImportDicColums();

                    string url = "";
                    string filePath = GenExcelExportFilePath(string.Format(typeof(staff).Name), ref url);

                    ExcelExport.ExportToExcelFromList(listStaff, dicColNames, filePath, string.Format("Nhân sự"));
                    //Input: http://27.72.147.222:1230/TempFiles/2020-03-11/department_200311210940.xlsx
                    //"D:\\BootAi\\ERP20\\ERP.API\\TempFiles\\2020-03-12\\department_200312092643.xlsx"
                    
                    filePath = filePath.Replace("\\", "/");
                    int index = filePath.IndexOf("TempFiles");
                    filePath = filePath.Substring(index);
                    response.Code = HttpCode.OK;
                    response.Message = "Đã xuất excel thành công!";
                    response.Data = filePath;
                }
                else
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "File excel import không có dữ liệu!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message; ;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
            return Ok(response);
        }
        #endregion

        #region["DicColums"]
        private Dictionary<string, string> GetImportDicColums()
        {
            return new Dictionary<string, string>()
            {
                 {"sta_code","MNV"},
                 {"sta_fullname","Họ và tên" },
                 {"sta_username","Tên đăng nhập"},
                 {"department_name","Phòng ban"},
                 {"position_name","Chức vụ"},
                 {"sta_sex_name","Giới tính"},
                 {"sta_address","Địa chỉ"},
                 {"sta_birthday","Ngày sinh"},
                 {"sta_mobile","Số điện thoại"},
                 {"sta_email","Email"},
                 {"sta_identity_card","Thẻ căn cước"},
                 {"sta_identity_card_date","Ngày cấp"},
                 {"sta_created_date","Ngày tạo"},
                 {"sta_start_work_date","Ngày vào làm"},
                  {"sta_end_work_date","Ngày nghỉ việc"},
                 {"sta_reason_to_end_work","Lý do nghỉ việc"},
                 {"sta_aboutme","Về bản thân"},
                 {"sta_status_name","Trạng thái"},
                 {"sta_leader_name","Quản lý"}
            };
        }
        private Dictionary<string, string> GetImportDicColumsTemplate()
        {
            return new Dictionary<string, string>()
            {
                  {"email","Email phong ban"},
                 {"id","Ma bộ phận phòng ban"}
            };
        }
    #endregion

        #region["Common funtion"]
        private bool check_department(int _id)
        {
            bool res = _departmentservice.Exist(x => x.de_id == _id);
            return res;
        }

        private bool check_grouprole(int _id)
        {
            bool res = _groupRoleservice.Exist(x => x.gr_id == _id);
            return res;
        }

        private bool check_position(int _id)
        {
            bool res = _positionService.Exist(x => x.pos_id == _id);
            return res;
        }

        private bool check_username_email(string _username, string _email)
        {
            bool res = _staffservice.Exist(x => x.sta_username == _username || x.sta_email == _email && _email != null);
            return res;
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
