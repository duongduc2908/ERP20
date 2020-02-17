using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Extension.Extensions;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
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
                response.Message = ex.Message;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
        [HttpGet]
        [Route("api/staffs/export")]
        public void Exports(int pageSize, int pageNumber)
        {
            _staffservice.Export(pageSize, pageNumber);
           
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
            ResponseDataDTO<PagedResults<string>> response = new ResponseDataDTO<PagedResults<string>>();
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
                // save file
                string fileName = "";
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    fileName = (FileExtension.SaveFileOnDisk(fileData));
                }
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
                if (streamProvider.FormData["sta_password"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Mật khẩu không được để trống";
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
                StaffCreateViewModel StaffCreateViewModel = new StaffCreateViewModel
                {
                    sta_fullname = Convert.ToString(streamProvider.FormData["sta_fullname"]),
                    //sta_code = Convert.ToString(streamProvider.FormData["sta_code"]),
                    sta_username = Convert.ToString(streamProvider.FormData["sta_username"]),
                    sta_password = Convert.ToString(streamProvider.FormData["sta_password"]),
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
                int count = _staffservice.Count();
                StaffCreateViewModel.sta_code = Utilis.CreateCode("KH", count, 7);
                // mapping view model to entity
                var createdstaff = _mapper.Map<staff>(StaffCreateViewModel);
                createdstaff.sta_thumbnai = fileName;
                createdstaff.sta_password = StaffCreateViewModel.sta_password;

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
                // save file
                string fileName = "";
                if (streamProvider.FileData.Count > 0)
                {
                    foreach (MultipartFileData fileData in streamProvider.FileData)
                    {
                        fileName = FileExtension.SaveFileOnDisk(fileData);
                    }
                }
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
                if (streamProvider.FormData["sta_password"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Mật khẩu không được để trống";
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
                    sta_password = Convert.ToString(streamProvider.FormData["sta_password"]),
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
                    if (fileName != "")
                    {
                        staffUpdateViewModel.sta_thumbnai = fileName;
                    }
                    else
                    {

                        staffUpdateViewModel.sta_thumbnai = existstaff.sta_thumbnai;
                    }
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
                    if(existstaff.sta_birthday != null)
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
                    if (existstaff.sta_identity_card != null)
                    {
                        staffUpdateViewModel.sta_identity_card = existstaff.sta_identity_card;
                    }
                    else
                    {
                        staffUpdateViewModel.sta_identity_card = null;
                    }
                }
                else
                {
                    staffUpdateViewModel.sta_identity_card = Convert.ToString(streamProvider.FormData["sta_identity_card"]);
                }

                if (streamProvider.FormData["sta_identity_card_date"] == null)
                {
                    if(existstaff.sta_identity_card_date != null)
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
                    if(existstaff.sta_end_work_date != null)
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
                    if(existstaff.sta_start_work_date != null)
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

                staffUpdateViewModel.sta_created_date =existstaff.sta_created_date;
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
                response.Message = ex.Message;
                response.Data = false;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }

        //[HttpPost]
        ////public async Task<IHttpActionResult> UploadExcel(User users, HttpPostedFileBase FileUpload)
        //{

            //List<string> data = new List<string>();
            //if (FileUpload != null)
            //{
            //    // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
            //    if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            //    {


            //        string filename = FileUpload.FileName;
            //        string targetpath = Server.MapPath("~/Doc/");
            //        FileUpload.SaveAs(targetpath + filename);
            //        string pathToExcelFile = targetpath + filename;
            //        var connectionString = "";
            //        if (filename.EndsWith(".xls"))
            //        {
            //            connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
            //        }
            //        else if (filename.EndsWith(".xlsx"))
            //        {
            //            connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
            //        }

            //        var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
            //        var ds = new DataSet();

            //        adapter.Fill(ds, "ExcelTable");

            //        DataTable dtable = ds.Tables["ExcelTable"];

            //        string sheetName = "Sheet1";

            //        var excelFile = new ExcelQueryFactory(pathToExcelFile);
            //        var artistAlbums = from a in excelFile.Worksheet<User>(sheetName) select a;

            //        foreach (var a in artistAlbums)
            //        {
            //            try
            //            {
            //                if (a.Name != "" && a.Address != "" && a.ContactNo != "")
            //                {
            //                    User TU = new User();
            //                    TU.Name = a.Name;
            //                    TU.Address = a.Address;
            //                    TU.ContactNo = a.ContactNo;
            //                    db.Users.Add(TU);

            //                    db.SaveChanges();



            //                }
            //                else
            //                {
            //                    data.Add("<ul>");
            //                    if (a.Name == "" || a.Name == null) data.Add("<li> name is required</li>");
            //                    if (a.Address == "" || a.Address == null) data.Add("<li> Address is required</li>");
            //                    if (a.ContactNo == "" || a.ContactNo == null) data.Add("<li>ContactNo is required</li>");

            //                    data.Add("</ul>");
            //                    data.ToArray();
            //                    return Json(data, JsonRequestBehavior.AllowGet);
            //                }
            //            }

            //            catch (DbEntityValidationException ex)
            //            {
            //                foreach (var entityValidationErrors in ex.EntityValidationErrors)
            //                {

            //                    foreach (var validationError in entityValidationErrors.ValidationErrors)
            //                    {

            //                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

            //                    }

            //                }
            //            }
            //        }
            //        //deleting excel file from folder  
            //        if ((System.IO.File.Exists(pathToExcelFile)))
            //        {
            //            System.IO.File.Delete(pathToExcelFile);
            //        }
            //        return Json("success", JsonRequestBehavior.AllowGet);
            //    }
            //    else
            //    {
            //        //alert message for invalid file format  
            //        data.Add("<ul>");
            //        data.Add("<li>Only Excel file format is allowed</li>");
            //        data.Add("</ul>");
            //        data.ToArray();
            //        return Json(data, JsonRequestBehavior.AllowGet);
            //    }
            //}
            //else
            //{
            //    data.Add("<ul>");
            //    if (FileUpload == null) data.Add("<li>Please choose Excel file</li>");
            //    data.Add("</ul>");
            //    data.ToArray();
            //    return Json(data, JsonRequestBehavior.AllowGet);
            //}
        //}
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
