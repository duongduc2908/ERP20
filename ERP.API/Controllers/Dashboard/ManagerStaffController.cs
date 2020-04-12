using AutoMapper;
using ERP.API.Models;
using ERP.API.Providers;
using ERP.Common.Constants;
using ERP.Common.Excel;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.StatisticStaff;
using ERP.Extension.Extensions;
using ERP.Service.Services.IServices;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
        private readonly ICustomerService _customerservice;
        private readonly IGroupRoleService _groupRoleservice;
        private readonly IPositionService _positionService;
        private readonly ISocialService _socialService;
        private readonly IStaffWorkTimeService _staffworktimeService;
        private readonly IUndertakenLocationService _undertakenlocationService;
        private readonly ITrainingStaffService _trainingStaffService;
        private readonly IMapper _mapper;
        private static string pass_word;
        private static List<string> list_email;
        private static List<string> list_pass;
        private static List<string> list_username;
        public ManagerstaffsController() { }
        public ManagerstaffsController(IStaffWorkTimeService staffworktimeService,ITrainingStaffService trainingStaffService, ICustomerService customerservice,IStaffService staffservice, IMapper mapper, IDepartmentService departmentService, IGroupRoleService groupRoleService, IPositionService positionService, IUndertakenLocationService undertakenlocationService, ISocialService socialService)
        {
            this._staffservice = staffservice;
            this._mapper = mapper;
            this._departmentservice = departmentService;
            this._groupRoleservice = groupRoleService;
            this._positionService = positionService;
            this._undertakenlocationService = undertakenlocationService;
            this._socialService = socialService;
            this._customerservice = customerservice;
            this._trainingStaffService = trainingStaffService;
            this._staffworktimeService = staffworktimeService;
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

        [Route("api/staff/search")]
        public IHttpActionResult GetStaffSearch(int pageSize, int pageNumber, int? status, DateTime? start_date, DateTime? end_date, string name)
        {
            ResponseDataDTO<PagedResults<staffviewmodel>> response = new ResponseDataDTO<PagedResults<staffviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _staffservice.GetAllPageSearch(  pageNumber, pageSize, status, start_date, end_date, name);
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

        [Route("api/staff/infor")]
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
        [Route("api/staff/create")]
        public async Task<IHttpActionResult> CreateStaff([FromBody] StaffCreateViewModelJson create_staff)
        {
            ResponseDataDTO<staff> response = new ResponseDataDTO<staff>();
            try
            {
                var staff = create_staff;
                #region["Check null"]

                if (staff.sta_fullname == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Họ và tên không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (staff.group_role_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nhóm quyền không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (staff.sta_sex == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Giới tính không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (staff.sta_username == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên đăng nhập không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (staff.position_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Chức vụ không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (staff.department_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Phòng ban không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (staff.sta_mobile == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Số điện thoại không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                #endregion
                //Kiểm tra các trường rằng buộc
                if (check_username(staff.sta_username))
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Đã có người dùng '" + staff.sta_username+" ' trong hệ thống.";
                    response.Data = null;
                    return Ok(response);
                }
                if (check_email(staff.sta_email))
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Đã có người dùng '" + staff.sta_email + " ' trong hệ thống.";
                    response.Data = null;
                    return Ok(response);
                }
                if (check_phone(staff.sta_mobile))
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Đã có số điện thoại người dùng '" + staff.sta_mobile + " ' trong hệ thống.";
                    response.Data = null;
                    return Ok(response);
                }
                //Save staff to database
                staff staff_create = new staff();
                //Thong tin chung 
                staff_create.sta_fullname = staff.sta_fullname;
                staff_create.group_role_id = staff.group_role_id;
                staff_create.sta_status = staff.sta_status;
                staff_create.sta_sex = staff.sta_sex;
                staff_create.sta_start_work_date = staff.sta_start_work_date;
                staff_create.sta_salary_to = staff.sta_salary_to;
                staff_create.sta_salary_end = staff.sta_salary_end;
                staff_create.sta_tax_code = staff.sta_tax_code;
                staff_create.sta_reason_to_end_work = staff.sta_reason_to_end_work;
                staff_create.sta_note = staff.sta_note;
               
                staff_create.sta_username = staff.sta_username;
                staff_create.position_id = staff.position_id;
                staff_create.department_id = staff.department_id;
                staff_create.sta_type_contact = staff.sta_type_contact;
                staff_create.sta_traffic = staff.sta_traffic;
                staff_create.sta_birthday = staff.sta_birthday;
                staff_create.sta_working_status = staff.sta_working_status;
                staff_create.sta_end_work_date = staff.sta_end_work_date;
                //Thong tin lien he
                staff_create.sta_mobile = staff.sta_mobile;
                staff_create.sta_email = staff.sta_email;

                // CMTND
                staff_create.sta_identity_card = staff.sta_identity_card;
                staff_create.sta_identity_card_date = staff.sta_identity_card_date;
                staff_create.sta_identity_card_date_end = staff.sta_identity_card_date_end;
                staff_create.sta_identity_card_location = staff.sta_identity_card_location;
                
                //Lấy ra bản ghi cuối cùng tạo mã code 
                var x = _staffservice.GetLast();
                if(x == null) staff_create.sta_code = "NV000000";
                else staff_create.sta_code = Utilis.CreateCodeByCode("NV", x.sta_code, 8);
                string sta_pass = "";
                if(staff.sta_type_contact == 0)
                {
                    sta_pass = staff_create.sta_code;
                }
                if(staff.sta_type_contact == 1)
                {
                    sta_pass = Utilis.MakeRandomPassword(8);
                }

                staff_create.sta_password = HashMd5.convertMD5(sta_pass);

                staff_create.sta_created_date = DateTime.Now;
                //Lần đầu đăng nhập login == true
                staff_create.sta_login = true;
                // save new staff
                _staffservice.Create(staff_create);
                staff staff_last = _staffservice.GetLast();

                //save staff_work_time
                staff_work_time sta_work_time = new staff_work_time();
                sta_work_time.staff_id = staff_last.sta_id;
                sta_work_time.sw_time_start = staff.sw_time_start;
                sta_work_time.sw_time_end = staff.sw_time_end;
                sta_work_time.st_fri_flag = staff.st_fri_flag;
                sta_work_time.st_mon_flag = staff.st_mon_flag;
                sta_work_time.st_sat_flag = staff.st_sat_flag;
                sta_work_time.st_sun_flag = staff.st_sun_flag;
                sta_work_time.st_thu_flag = staff.st_thu_flag;
                sta_work_time.st_tue_flag = staff.st_tue_flag;
                sta_work_time.st_wed_flag = staff.st_wed_flag;
                _staffworktimeService.Create(sta_work_time);
                //save address thường trú 
                undertaken_location address_permanent = new undertaken_location();
                address_permanent.staff_id = staff_last.sta_id;
                address_permanent.unl_ward = staff.unl_ward_permanent;
                address_permanent.unl_province = staff.unl_province_permanent;
                address_permanent.unl_district = staff.unl_district_permanent;
                address_permanent.unl_geocoding = staff.unl_geocoding_permanent;
                address_permanent.unl_detail = staff.unl_detail_permanent;
                address_permanent.unl_note = staff.unl_note_permanent;
                address_permanent.unl_flag_center = 1;
                _undertakenlocationService.Create(address_permanent);
                //save address hiện tại 
                undertaken_location address_now = new undertaken_location();
                address_now.staff_id = staff_last.sta_id;
                address_now.unl_ward = staff.unl_ward_now;
                address_now.unl_province = staff.unl_province_now;
                address_now.unl_district = staff.unl_district_now;
                address_now.unl_geocoding = staff.unl_geocoding_now;
                address_now.unl_detail = staff.unl_detail_now;
                address_now.unl_note = staff.unl_note_now;
                address_now.unl_flag_center = 2;
                _undertakenlocationService.Create(address_now);
                //Save list training 
                foreach(int training_id in staff.list_training)
                {
                    training_staff create_training_staff = new training_staff();
                    create_training_staff.staff_id = staff_last.sta_id;
                    create_training_staff.training_id = training_id;
                    _trainingStaffService.Create(create_training_staff);
                }
                //Save list_undertaken_location
                foreach(undertaken_location location in staff.list_undertaken_location)
                {
                    undertaken_location address = new undertaken_location();
                    address.staff_id = staff_last.sta_id;
                    address.unl_ward = location.unl_ward;
                    address.unl_province = location.unl_province;
                    address.unl_district = location.unl_district;
                    address.unl_geocoding = location.unl_geocoding;
                    address.unl_detail = location.unl_detail;
                    address.unl_note = location.unl_note;
                    address.unl_flag_center = 0;
                    _undertakenlocationService.Create(address);
                }

                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = staff_create;
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
        [Route("api/staff/update")]
        public async Task<IHttpActionResult> UpdateStaff([FromBody] StaffUpdateViewModelJson update_staff)
        {
            ResponseDataDTO<staff> response = new ResponseDataDTO<staff>();
            try
            {
                
                var staff = update_staff;
                staff existstaff = _staffservice.Find(staff.sta_id);
                #region["Check null"]

                if (staff.sta_fullname == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Họ và tên không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (staff.group_role_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nhóm quyền không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (staff.sta_sex == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Giới tính không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (staff.sta_username == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên đăng nhập không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (staff.position_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Chức vụ không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (staff.department_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Phòng ban không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (staff.sta_mobile == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Số điện thoại không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                #endregion
                //Update 
                //Thong tin chung 
                existstaff.sta_fullname = staff.sta_fullname;
                existstaff.group_role_id = staff.group_role_id;
                existstaff.sta_status = staff.sta_status;
                existstaff.sta_sex = staff.sta_sex;
                existstaff.sta_start_work_date = staff.sta_start_work_date;
                existstaff.sta_salary_to = staff.sta_salary_to;
                existstaff.sta_salary_end = staff.sta_salary_end;
                existstaff.sta_tax_code = staff.sta_tax_code;
                existstaff.sta_reason_to_end_work = staff.sta_reason_to_end_work;
                existstaff.sta_note = staff.sta_note;

                existstaff.sta_username = staff.sta_username;
                existstaff.position_id = staff.position_id;
                existstaff.department_id = staff.department_id;
                existstaff.sta_traffic = staff.sta_traffic;
                existstaff.sta_birthday = staff.sta_birthday;
                existstaff.sta_working_status = staff.sta_working_status;
                existstaff.sta_end_work_date = staff.sta_end_work_date;
                //Thong tin lien he
                existstaff.sta_mobile = staff.sta_mobile;
                existstaff.sta_email = staff.sta_email;

                // CMTND
                existstaff.sta_identity_card = staff.sta_identity_card;
                existstaff.sta_identity_card_date = staff.sta_identity_card_date;
                existstaff.sta_identity_card_date_end = staff.sta_identity_card_date_end;
                existstaff.sta_identity_card_location = staff.sta_identity_card_location;
                
                // save new staff
                _staffservice.Update(existstaff,staff.sta_id);

                //save staff_work_time
                staff_work_time exist_work_time = _staffworktimeService.GetAllIncluing(x => x.staff_id == staff.sta_id).FirstOrDefault();

                exist_work_time.sw_time_start = staff.sw_time_start;
                exist_work_time.sw_time_end = staff.sw_time_end;
                exist_work_time.st_fri_flag = staff.st_fri_flag;
                exist_work_time.st_mon_flag = staff.st_mon_flag;
                exist_work_time.st_sat_flag = staff.st_sat_flag;
                exist_work_time.st_sun_flag = staff.st_sun_flag;
                exist_work_time.st_thu_flag = staff.st_thu_flag;
                exist_work_time.st_tue_flag = staff.st_tue_flag;
                exist_work_time.st_wed_flag = staff.st_wed_flag;
                _staffworktimeService.Update(exist_work_time, exist_work_time.sw_id);
                //save address thường trú 
                undertaken_location exist_address_permanent = _undertakenlocationService.GetAllIncluing(x => x.staff_id == staff.sta_id && x.unl_flag_center == 1).FirstOrDefault();

                exist_address_permanent.unl_ward = staff.unl_ward_permanent;
                exist_address_permanent.unl_province = staff.unl_province_permanent;
                exist_address_permanent.unl_district = staff.unl_district_permanent;
                exist_address_permanent.unl_geocoding = staff.unl_geocoding_permanent;
                exist_address_permanent.unl_detail = staff.unl_detail_permanent;
                exist_address_permanent.unl_note = staff.unl_note_permanent;
                _undertakenlocationService.Update(exist_address_permanent, exist_address_permanent.unl_id);
                //update address hiện tại 
                undertaken_location exist_address_now = new undertaken_location();
                exist_address_now.unl_ward = staff.unl_ward_now;
                exist_address_now.unl_province = staff.unl_province_now;
                exist_address_now.unl_district = staff.unl_district_now;
                exist_address_now.unl_geocoding = staff.unl_geocoding_now;
                exist_address_now.unl_detail = staff.unl_detail_now;
                exist_address_now.unl_note = staff.unl_note_now;
                _undertakenlocationService.Update(exist_address_now, exist_address_now.unl_id);
                //update training 
                //Xóa bản ghi cũ update cái mới 
                List<training_staff> training_staff_old = _trainingStaffService.GetAllIncluing(x => x.staff_id == staff.sta_id).ToList();
                foreach(training_staff ts in training_staff_old)
                {
                    _trainingStaffService.Delete(ts);
                }
                foreach(int training_id in staff.list_training)
                {
                    training_staff create_training_staff = new training_staff();
                    create_training_staff.staff_id = staff.sta_id;
                    create_training_staff.training_id = training_id;
                    _trainingStaffService.Create(create_training_staff);

                }
                //update list_undertaken_location
                List<undertaken_location> lts_ul_db = _undertakenlocationService.GetAllIncluing(x => x.staff_id == staff.sta_id && x.unl_flag_center == 0).ToList();
                List<undertaken_location> lts_ul_v = new List<undertaken_location>(staff.list_undertaken_location);
                foreach(undertaken_location ul_f in lts_ul_v)
                {
                    foreach(undertaken_location ul in lts_ul_db)
                    {
                        if(ul.unl_id == ul_f.unl_id)
                        {
                            //update
                            undertaken_location exist_address = _undertakenlocationService.Find(ul_f.unl_id);
                            exist_address.unl_ward = ul_f.unl_ward;
                            exist_address.unl_province = ul_f.unl_province;
                            exist_address.unl_district = ul_f.unl_district;
                            exist_address.unl_geocoding = ul_f.unl_geocoding;
                            exist_address.unl_detail = ul_f.unl_detail;
                            exist_address.unl_note = ul_f.unl_note;
                            _undertakenlocationService.Update(exist_address, exist_address.unl_id);
                            lts_ul_db.Remove(_undertakenlocationService.Find(ul.unl_id));
                            break;
                        }
                    }
                    if(ul_f.unl_id == 0 )
                    {
                        //Create
                        ul_f.staff_id = staff.sta_id;
                        ul_f.unl_flag_center = 0;
                        _undertakenlocationService.Create(ul_f);
                    }
                }
                foreach(undertaken_location ul_d in lts_ul_db)
                {
                    _undertakenlocationService.Delete(ul_d);
                }
               

                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = existstaff;
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

        [HttpPost]
        [Route("api/staffs/sendmail_created")]
        public IHttpActionResult send_mail_created(string sta_username, string sta_email)
        {
            ResponseDataDTO<staff> response = new ResponseDataDTO<staff>();
            try
            {
                string text1 = File.ReadAllText("D:/ERP20/ERP.Common/TemplateMail/SetPassWord/Set1.txt");
                string text2 = File.ReadAllText("D:/ERP20/ERP.Common/TemplateMail/SetPassWord/Set2.txt");
                string text3 = File.ReadAllText("D:/ERP20/ERP.Common/TemplateMail/SetPassWord/Set3.txt");
                var text_send = text1+ sta_username + text2 + pass_word + text3;
                BaseController.send_mail(text_send, sta_email, "New User Created!!!");
                pass_word = "";
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = null;
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

       

        [HttpDelete]
        [Route("api/staff/delete")]
        public IHttpActionResult Deletestaff(int staff_id)
        {
            ResponseDataDTO<staff> response = new ResponseDataDTO<staff>();
            try
            {
                var staffDeleted = _staffservice.Find(staff_id);
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
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nhập đúng mật khẩu mới.";
                    response.Data = false;
                    return Ok(response);
                }
                else {
                    bool check = _staffservice.ChangePassword(model, id);
                    if (check == false)
                    {
                        // return response
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Mật khẩu cũ không đúng.";
                        response.Data = false;
                        return Ok(response);
                    }
                    else
                    {
                        // return response
                        response.Code = HttpCode.OK;
                        response.Message = MessageResponse.SUCCESS;
                        response.Data = true;
                        return Ok(response);
                    }
                }
                
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
                                    exitsData = "Không có mã phòng ban "+ dr["department_id"].ToString() + " trong hệ thống.";
                                    response.Code = HttpCode.NOT_FOUND;
                                    response.Message = exitsData;
                                    response.Data = null;
                                    return Ok(response);
                                }
                                if (!check_position(Convert.ToInt32(dr["position_id"])))
                                {
                                    exitsData = "Không có mã bộ phận "+ dr["position_id"].ToString()+" trong hệ thống.";
                                    response.Code = HttpCode.NOT_FOUND;
                                    response.Message = exitsData;
                                    response.Data = null;
                                    return Ok(response);
                                }
                                if (!check_grouprole(Convert.ToInt32(dr["group_role_id"])))
                                {
                                    exitsData = "Không có mã nhóm quyền "+ dr["group_role_id"].ToString()+" trong hệ thống.";
                                    response.Code = HttpCode.NOT_FOUND;
                                    response.Message = exitsData;
                                    response.Data = null;
                                    return Ok(response);
                                }
                                if (check_username(dr["sta_username"].ToString()))
                                {
                                    response.Code = HttpCode.NOT_FOUND;
                                    response.Message = "Đã có người dùng '" + dr["sta_username"].ToString() + " ' trong hệ thống.";
                                    response.Data = null;
                                    return Ok(response);
                                }
                                if (check_email(dr["sta_email"].ToString()))
                                {
                                    response.Code = HttpCode.NOT_FOUND;
                                    response.Message = "Đã có người dùng '" + dr["sta_email"].ToString() + " ' trong hệ thống.";
                                    response.Data = null;
                                    return Ok(response);
                                }
                                if (check_username(dr["sta_mobile"].ToString()))
                                {
                                    response.Code = HttpCode.NOT_FOUND;
                                    response.Message = "Đã có số điện thoại người dùng '" + dr["sta_mobile"].ToString() + " ' trong hệ thống.";
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
                        var pass = Utilis.MakeRandomPassword(8);
                        list_username.Add(i.sta_username);
                        list_email.Add(i.sta_email);
                        list_pass.Add(pass);
                        i.sta_password = HashMd5.convertMD5(pass);
                        if (i.sta_sex == 1)
                        {
                            i.sta_thumbnai = "/Uploads/Images/default/girl.png";
                        }
                        else
                        {
                            i.sta_thumbnai = "/Uploads/Images/default/man.png";
                        }
                        _staffservice.Create(i);
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

        [HttpPost]
        [Route("api/staffs/sendmail_import")]
        public IHttpActionResult send_mail_imported()
        {
            ResponseDataDTO<staff> response = new ResponseDataDTO<staff>();
            try
            {
                string text1 = File.ReadAllText("D:/ERP20/ERP.Common/TemplateMail/ResetPassWord/Set1.txt");
                string text2 = File.ReadAllText("D:/ERP20/ERP.Common/TemplateMail/ResetPassWord/Set2.txt");
                string text3 = File.ReadAllText("D:/ERP20/ERP.Common/TemplateMail/ResetPassWord/Set3.txt");
                for(int i=0;i< list_email.Count-1;i++)
                {
                    var text_send = text1 + list_username[i] + text2 + list_pass[i] + text3;
                    BaseController.send_mail(text_send, list_email[i], "New User Created!!!");
                }
                list_username.Clear();
                list_email.Clear();
                list_pass.Clear();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = null;
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
        #endregion

        #region["Export Excel"]
        [HttpGet]
        [Route("api/satff/export")]
        public async Task<IHttpActionResult> Export(int pageSize, int pageNumber, int? status, DateTime? start_date, DateTime? end_date, string name)
        {
            ResponseDataDTO<string> response = new ResponseDataDTO<string>();
            try
            {
                var listStaff = new List<staffview>();
                //Đưa ra danh sách staff trong trang nào đó 
                var objRT_Mst_Staff = _staffservice.ExportStaff(pageNumber, pageSize,status, start_date,end_date, name);
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
        private bool check_username(string _username)
        {
            bool res = _staffservice.Exist(x => x.sta_username == _username );
            return res;
        }
        private bool check_email(string _email)
        {
            bool res = _staffservice.Exist(x => x.sta_email == _email && _email != null);
            return res;
        }
        private bool check_phone(string _phone)
        {
            bool res = _staffservice.Exist(x => x.sta_mobile == _phone && _phone != null);
            return res;
        }
        #endregion

        #region profile-staff
        [HttpGet]
        [Route("api/staff/profile")]
        public IHttpActionResult GetInfor()
        {
            ResponseDataDTO<statisticstaffviewmodel> response = new ResponseDataDTO<statisticstaffviewmodel>();
            try
            {
                int staff_id = BaseController.get_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _staffservice.GetInfor(staff_id);
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

        #endregion

        #region Update Profile 
        [HttpPut]
        [Route("api/profile/update")]
        public async Task<IHttpActionResult> UpdateProfile()
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
                if (streamProvider.FormData["sta_email"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Email không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["sta_address"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Mật khẩu không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                //Cập nhập thông tin nhân sự 
                int staff_id = BaseController.get_id_current();
                var existstaff = _staffservice.Find(staff_id);
                existstaff.sta_fullname = Convert.ToString(streamProvider.FormData["sta_fullname"]);
                existstaff.sta_email = Convert.ToString(streamProvider.FormData["sta_email"]);
                existstaff.sta_mobile = Convert.ToString(streamProvider.FormData["sta_mobile"]);
                existstaff.sta_aboutme = Convert.ToString(streamProvider.FormData["sta_aboutme"]);
                //Cập nhập thông tin mạng xã hội 
                
                var existSocial = _socialService.GetAllIncluing(t => t.staff_id == staff_id).FirstOrDefault();
                if(existSocial != null)
                {
                    existSocial.soc_facebook = Convert.ToString(streamProvider.FormData["soc_facebook"]);
                    existSocial.soc_instagram = Convert.ToString(streamProvider.FormData["soc_instagram"]);
                    existSocial.soc_twitter = Convert.ToString(streamProvider.FormData["soc_twitter"]);
                    existSocial.soc_skype = Convert.ToString(streamProvider.FormData["soc_skype"]);
                    existSocial.soc_linkedin = Convert.ToString(streamProvider.FormData["soc_linkedin"]);
                    existSocial.soc_github = Convert.ToString(streamProvider.FormData["soc_github"]);
                }
                
                //update social
                _socialService.Update(existSocial, existSocial.soc_id);

                // update staff
                _staffservice.Update(existstaff, staff_id);
                

                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = existstaff;
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
        #endregion
        #region[Update curator]
        [HttpPut]
        [Route("api/staff/update-curator")]
        public async Task<IHttpActionResult> UpdateCurator([FromBody] StaffUpdateCuratorViewModel staff_update)
        {
            ResponseDataDTO<string> response = new ResponseDataDTO<string>();
            try
            {
                // Sử dụng thuật toán trong stack hay hon nhưng chưa có thời gian 
                var assignment = staff_update;
                List<customer> lts_cus = _customerservice.GetAllIncluing(x => x.cu_curator_id == null).ToList();
                Stack<customer> myStack = new Stack<customer>(lts_cus);
                int total_customer = lts_cus.Count(); // so luong khach hang 
                int number = assignment.list_staff_id.Length; //So luong satff
                int skip = total_customer / number;
                int mod = total_customer % number;
                if (assignment.customer_group_id == 0)
                {
                    if(number < total_customer)
                    {
                        foreach (int s in assignment.list_staff_id)
                        {
                            for(int i = 0; i< skip; i++)
                            {
                                customer update_customer;
                                if (myStack.Count() == 0) break;
                                else
                                {
                                    update_customer = myStack.Pop();
                                }
                                update_customer.cu_curator_id = s;
                                _customerservice.Update(update_customer, update_customer.cu_id);
                            }
                        }
                        foreach (int s in assignment.list_staff_id)
                        {
                            customer update_customer;
                            if (myStack.Count() == 0) break;
                            else
                            {
                                update_customer = myStack.Pop();
                            }
                            update_customer.cu_curator_id = s;
                            _customerservice.Update(update_customer, update_customer.cu_id);
                        }
                    }
                    else
                    {
                        foreach (int s in assignment.list_staff_id)
                        {
                            customer update_customer;
                            if (myStack.Count() == 0) break;
                            else
                            {
                                update_customer = myStack.Pop();
                            }
                            update_customer.cu_curator_id = s;
                            _customerservice.Update(update_customer, update_customer.cu_id);
                        }
                    }
                    
                }
                else
                {
                    lts_cus = lts_cus.Where(x => x.customer_group_id == assignment.customer_group_id).ToList();
                    myStack = new Stack<customer>(lts_cus);
                    total_customer = lts_cus.Count();
                    skip = total_customer / number;
                    mod = total_customer % number;
                    if (number < total_customer)
                    {
                        foreach (int s in assignment.list_staff_id)
                        {
                            for (int i = 0; i < skip; i++)
                            {
                                customer update_customer;
                                if (myStack.Count() == 0) break;
                                else
                                {
                                    update_customer = myStack.Pop();
                                }
                                update_customer.cu_curator_id = s;
                                _customerservice.Update(update_customer, update_customer.cu_id);
                            }
                        }
                        foreach (int s in assignment.list_staff_id)
                        {
                            customer update_customer;
                            if (myStack.Count() == 0) break;
                            else
                            {
                                update_customer = myStack.Pop();
                            }
                            update_customer.cu_curator_id = s;
                            _customerservice.Update(update_customer, update_customer.cu_id);
                        }
                    }
                    else
                    {
                        foreach (int s in assignment.list_staff_id)
                        {
                            customer update_customer;
                            if (myStack.Count() == 0) break;
                            else
                            {
                                update_customer = myStack.Pop();
                            }
                            update_customer.cu_curator_id = s;
                            _customerservice.Update(update_customer, update_customer.cu_id);
                        }
                    }

                }
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = null;
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
