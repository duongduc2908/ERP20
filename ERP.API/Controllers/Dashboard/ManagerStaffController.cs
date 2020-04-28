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
using ERP.Data.ModelsERP.ModelView.Staff;
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
        private readonly ITrainingService _trainingService;
        private readonly IMapper _mapper;
        private static string pass_word;
        private static List<string> list_email;
        private static List<string> list_pass;
        private static List<string> list_username;
        public ManagerstaffsController() { }
        public ManagerstaffsController(ITrainingService trainingService, IStaffWorkTimeService staffworktimeService, ITrainingStaffService trainingStaffService, ICustomerService customerservice, IStaffService staffservice, IMapper mapper, IDepartmentService departmentService, IGroupRoleService groupRoleService, IPositionService positionService, IUndertakenLocationService undertakenlocationService, ISocialService socialService)
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
            this._trainingService = trainingService;
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
        public IHttpActionResult GetStaffSearch(int pageSize, int pageNumber, int? status, DateTime? start_date, DateTime? end_date, string name, int? sta_working_status)
        {
            ResponseDataDTO<PagedResults<staffviewmodel>> response = new ResponseDataDTO<PagedResults<staffviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _staffservice.GetAllPageSearch(pageNumber, pageSize, status, start_date, end_date, name, sta_working_status);
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

        [Route("api/staff/get_by_id")]
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
                response.Data = _staffservice.GetAllActive(pageNumber, pageSize, status);
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

                if (staff.sta_fullname == null || staff.sta_fullname.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Họ và tên không được để trống";
                    response.Error = "sta_fullname";
                    return Ok(response);
                }
                if (staff.group_role_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nhóm quyền không được để trống";
                    response.Error = "group_role_id";
                    return Ok(response);
                }

                if (staff.sta_sex == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Giới tính không được để trống";
                    response.Error = "sta_sex";
                    return Ok(response);
                }

                if (staff.sta_username == null || staff.sta_username.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên đăng nhập không được để trống";
                    response.Error = "sta_username";
                    return Ok(response);
                }

                if (staff.position_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Chức vụ không được để trống";
                    response.Error = "position_id";
                    return Ok(response);
                }
                if (staff.department_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Phòng ban không được để trống";
                    response.Error = "department_id";
                    return Ok(response);
                }

                if (staff.sta_mobile == null || staff.sta_mobile.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Số điện thoại không được để trống";
                    response.Error = "sta_mobile";
                    return Ok(response);
                }

                #endregion
                //Kiểm tra các trường rằng buộc
                if (check_username(staff.sta_username.Trim()))
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Đã có người dùng '" + staff.sta_username + " ' trong hệ thống.";
                    response.Error = "sta_username";

                    return Ok(response);
                }
                if (staff.sta_type_contact == 0)
                {
                    if (staff.sta_email == null || staff.sta_email.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Email không được để trống";
                        response.Error = "sta_email";
                        return Ok(response);
                    }
                    if (!Utilis.IsValidEmail(staff.sta_email))
                    {
                        response.Code = HttpCode.NOT_FOUND;
                        response.Message = "Email sai định dạng";
                        response.Error = "sta_email";
                        return Ok(response);
                    }
                    if (check_email(staff.sta_email))
                    {
                        response.Code = HttpCode.NOT_FOUND;
                        response.Message = "Đã có người dùng '" + staff.sta_email + " ' trong hệ thống.";
                        response.Error = "sta_email";
                        return Ok(response);
                    }
                }
                if (staff.sta_type_contact == 1)
                {
                    if (staff.sta_email != null)
                    {
                        if (staff.sta_email.Trim() != "")
                        {
                            if (!Utilis.IsValidEmail(staff.sta_email))
                            {
                                response.Code = HttpCode.NOT_FOUND;
                                response.Message = "Email sai định dạng";
                                response.Error = "sta_email";
                                return Ok(response);
                            }
                            if (check_email(staff.sta_email))
                            {
                                response.Code = HttpCode.NOT_FOUND;
                                response.Message = "Đã có người dùng '" + staff.sta_email + " ' trong hệ thống.";
                                response.Error = "sta_email";
                                return Ok(response);
                            }
                        }

                    }
                }
                if (check_phone(staff.sta_mobile))
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Đã có số điện thoại người dùng '" + staff.sta_mobile + " ' trong hệ thống.";
                    response.Error = "sta_mobile";
                    return Ok(response);
                }
                //Save staff to database
                staff staff_create = new staff();
                //Thong tin chung 

                staff_create.sta_type_contact = staff.sta_type_contact;
                staff_create.sta_fullname = staff.sta_fullname.Trim();
                staff_create.group_role_id = staff.group_role_id;
                staff_create.sta_status = staff.sta_status;
                staff_create.sta_sex = staff.sta_sex;
                if (staff_create.sta_sex == 1)
                {
                    staff_create.sta_thumbnai = "/Uploads/Images/default/man.png";
                }
                else
                {
                    staff_create.sta_thumbnai = "/Uploads/Images/default/girl.png";
                }
                staff_create.sta_start_work_date = staff.sta_start_work_date;
                staff_create.sta_salary = staff.sta_salary;
                staff_create.sta_tax_code = staff.sta_tax_code;
                staff_create.sta_reason_to_end_work = staff.sta_reason_to_end_work;
                staff_create.sta_note = staff.sta_note;

                staff_create.sta_username = staff.sta_username.Trim();
                staff_create.position_id = staff.position_id;
                staff_create.department_id = staff.department_id;
                staff_create.sta_traffic = staff.sta_traffic;
                staff_create.sta_birthday = staff.sta_birthday;
                staff_create.sta_working_status = staff.sta_working_status;
                staff_create.sta_end_work_date = staff.sta_end_work_date;
                //Thong tin lien he
                if (staff.sta_mobile != null) staff_create.sta_mobile = staff.sta_mobile.Trim();
                if (staff.sta_email != null) staff_create.sta_email = staff.sta_email.Trim();

                // CMTND
                staff_create.sta_identity_card = staff.sta_identity_card;
                staff_create.sta_identity_card_date = staff.sta_identity_card_date;
                staff_create.sta_identity_card_date_end = staff.sta_identity_card_date_end;
                staff_create.sta_identity_card_location = staff.sta_identity_card_location;

                //Lấy ra bản ghi cuối cùng tạo mã code 
                var x = _staffservice.GetLast();
                if (x == null) staff_create.sta_code = "NV000000";
                else staff_create.sta_code = Utilis.CreateCodeByCode("NV", x.sta_code, 8);
                string sta_pass = "";
                if (staff.sta_type_contact == 0)
                {
                    sta_pass = staff_create.sta_code;
                }
                if (staff.sta_type_contact == 1)
                {
                    sta_pass = Utilis.MakeRandomPassword(8);
                }

                staff_create.sta_password = HashMd5.convertMD5(sta_pass);
                pass_word = staff_create.sta_password;
                staff_create.sta_created_date = DateTime.Now;
                //Lần đầu đăng nhập login == true
                staff_create.sta_login = true;
                // save new staff
                _staffservice.Create(staff_create);
                staff staff_last = _staffservice.GetLast();

                //save staff_work_time
                foreach (staff_work_timejson swt in staff.list_staff_work_time)
                {
                    staff_work_time sta_work_time = new staff_work_time();
                    sta_work_time.staff_id = staff_last.sta_id;
                    sta_work_time.sw_time_start = swt.sw_time_start;
                    sta_work_time.sw_time_end = swt.sw_time_end;
                    sta_work_time.sw_day_flag = swt.sw_day_flag;
                    _staffworktimeService.Create(sta_work_time);
                }

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
                //Save list training và training sftaff

                foreach (stafftrainingjson tr in staff.list_training)
                {
                    if (Utilis.IsNumber(tr.tn_id))
                    {
                        training_staff create_training_staff = new training_staff();
                        create_training_staff.staff_id = staff_last.sta_id;
                        create_training_staff.training_id = Convert.ToInt32(tr.tn_id);
                        create_training_staff.ts_evaluate = tr.ts_evaluate;
                        _trainingStaffService.Create(create_training_staff);
                    }
                    else
                    {
                        training create_training = new training();
                        create_training.tn_name = tr.tn_name;
                        create_training.tn_content = tr.tn_content;
                        create_training.tn_start_date = tr.tn_start_date;
                        create_training.tn_end_date = tr.tn_end_date;
                        create_training.tn_purpose = tr.tn_purpose;
                        var t = _trainingService.GetLast();
                        if (t == null) create_training.tn_code = "DT000000";
                        else create_training.tn_code = Utilis.CreateCodeByCode("DT", t.tn_code, 8);
                        _trainingService.Create(create_training);
                        training tr_last = _trainingService.GetLast();
                        training_staff create_training_staff = new training_staff();
                        create_training_staff.staff_id = staff_last.sta_id;
                        create_training_staff.training_id = tr_last.tn_id;
                        create_training_staff.ts_evaluate = tr.ts_evaluate;
                        _trainingStaffService.Create(create_training_staff);
                    }

                }
                //Save list_undertaken_location
                foreach (staffundertaken_locationjson location in staff.list_undertaken_location)
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

                if (staff.sta_fullname == null || staff.sta_fullname.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Họ và tên không được để trống";
                    response.Error = "sta_fullname";
                    return Ok(response);
                }
                if (staff.group_role_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nhóm quyền không được để trống";
                    response.Error = "group_role_id";
                    return Ok(response);
                }

                if (staff.sta_sex == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Giới tính không được để trống";
                    response.Error = "sta_sex";
                    return Ok(response);
                }

                if (staff.sta_username == null || staff.sta_username.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên đăng nhập không được để trống";
                    response.Error = "sta_username";
                    return Ok(response);
                }

                if (staff.position_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Chức vụ không được để trống";
                    response.Error = "position_id";
                    return Ok(response);
                }
                if (staff.department_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Phòng ban không được để trống";
                    response.Error = "department_id";
                    return Ok(response);
                }

                if (staff.sta_mobile == null || staff.sta_mobile.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Số điện thoại không được để trống";
                    response.Error = "sta_mobile";
                    return Ok(response);
                }

                #endregion
                //Check exist
                if (check_username_update(staff.sta_username.Trim(), staff.sta_id))
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Đã có người dùng '" + staff.sta_username + " ' trong hệ thống.";
                    response.Error = "sta_username";
                    return Ok(response);
                }
                if (staff.sta_type_contact == 0)
                {

                    if (staff.sta_email == null || staff.sta_email.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Email không được để trống";
                        response.Error = "sta_email";
                        return Ok(response);
                    }
                    if (!Utilis.IsValidEmail(staff.sta_email))
                    {
                        response.Code = HttpCode.NOT_FOUND;
                        response.Message = "Email sai định dạng";
                        response.Error = "sta_email";
                        return Ok(response);
                    }
                    if (check_email_update(staff.sta_email, staff.sta_id))
                    {
                        response.Code = HttpCode.NOT_FOUND;
                        response.Message = "Đã có người dùng '" + staff.sta_email + " ' trong hệ thống.";
                        response.Error = "sta_email";
                        return Ok(response);
                    }
                }
                if (staff.sta_type_contact == 1)
                {
                    if (staff.sta_email != null)
                    {
                        if (staff.sta_email.Trim() != "")
                        {
                            if (!Utilis.IsValidEmail(staff.sta_email))
                            {
                                response.Code = HttpCode.NOT_FOUND;
                                response.Message = "Email sai định dạng";
                                response.Error = "sta_email";
                                return Ok(response);
                            }
                            if (check_email_update(staff.sta_email,staff.sta_id))
                            {
                                response.Code = HttpCode.NOT_FOUND;
                                response.Message = "Đã có người dùng '" + staff.sta_email + " ' trong hệ thống.";
                                response.Error = "sta_email";
                                return Ok(response);
                            }
                        }
                    }
                }

                if (check_phone_update(staff.sta_mobile, staff.sta_id))
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Đã có số điện thoại người dùng '" + staff.sta_mobile + " ' trong hệ thống.";
                    response.Error = "sta_mobile";
                    return Ok(response);
                }
                //Update 
                //Thong tin chung 
                existstaff.sta_type_contact = staff.sta_type_contact;
                existstaff.sta_fullname = staff.sta_fullname.Trim();
                existstaff.group_role_id = staff.group_role_id;
                existstaff.sta_status = staff.sta_status;
                existstaff.sta_sex = staff.sta_sex;
                existstaff.sta_start_work_date = staff.sta_start_work_date;
                existstaff.sta_salary = staff.sta_salary;
                existstaff.sta_tax_code = staff.sta_tax_code;
                existstaff.sta_reason_to_end_work = staff.sta_reason_to_end_work;
                if (staff.sta_note != null)
                    existstaff.sta_note = staff.sta_note.Trim();

                existstaff.sta_username = staff.sta_username.Trim();
                existstaff.position_id = staff.position_id;
                existstaff.department_id = staff.department_id;
                existstaff.sta_traffic = staff.sta_traffic;
                existstaff.sta_birthday = staff.sta_birthday;
                existstaff.sta_working_status = staff.sta_working_status;
                existstaff.sta_end_work_date = staff.sta_end_work_date;
                //Thong tin lien he
                if (staff.sta_mobile != null) existstaff.sta_mobile = staff.sta_mobile.Trim();
                if (staff.sta_email != null) existstaff.sta_email = staff.sta_email.Trim();
                // CMTND
                existstaff.sta_identity_card = staff.sta_identity_card;
                existstaff.sta_identity_card_date = staff.sta_identity_card_date;
                existstaff.sta_identity_card_date_end = staff.sta_identity_card_date_end;
                existstaff.sta_identity_card_location = staff.sta_identity_card_location;

                // save new staff
                _staffservice.Update(existstaff, staff.sta_id);

                //save staff_work_time

                List<staff_work_time> lts_sw_db = _staffworktimeService.GetAllIncluing(x => x.staff_id == staff.sta_id).ToList();
                List<staff_work_timejson> lts_sw_v = new List<staff_work_timejson>(staff.list_staff_work_time);
                foreach (staff_work_timejson ul_f in lts_sw_v)
                {
                    if (Utilis.IsNumber(ul_f.sw_id))
                    {
                        int _id = Convert.ToInt32(ul_f.sw_id);
                        foreach (staff_work_time ul in lts_sw_db)
                        {
                            if (ul.sw_id == _id)
                            {
                                //update
                                staff_work_time exist_work_time = _staffworktimeService.Find(_id);
                                exist_work_time.sw_time_start = ul_f.sw_time_start;
                                exist_work_time.sw_time_end = ul_f.sw_time_end;
                                exist_work_time.sw_day_flag = ul_f.sw_day_flag;
                                _staffworktimeService.Update(exist_work_time, exist_work_time.sw_id);
                                lts_sw_db.Remove(_staffworktimeService.Find(ul.sw_id));
                                break;
                            }
                        }
                    }
                    else
                    {
                        //Create
                        staff_work_time create_work_time = new staff_work_time();
                        create_work_time.sw_time_start = ul_f.sw_time_start;
                        create_work_time.staff_id = staff.sta_id;
                        create_work_time.sw_time_end = ul_f.sw_time_end;
                        create_work_time.sw_day_flag = ul_f.sw_day_flag;
                        _staffworktimeService.Create(create_work_time);
                    }
                }
                foreach (staff_work_time ul_d in lts_sw_db)
                {
                    _staffworktimeService.Delete(ul_d);
                }


                //save address thường trú 
                undertaken_location exist_address_permanent = _undertakenlocationService.GetAllIncluing(x => x.staff_id == staff.sta_id && x.unl_flag_center == 1).FirstOrDefault();
                if (exist_address_permanent != null)
                {
                    exist_address_permanent.unl_ward = staff.unl_ward_permanent;
                    exist_address_permanent.unl_province = staff.unl_province_permanent;
                    exist_address_permanent.unl_district = staff.unl_district_permanent;
                    exist_address_permanent.unl_geocoding = staff.unl_geocoding_permanent;
                    exist_address_permanent.unl_detail = staff.unl_detail_permanent;
                    exist_address_permanent.unl_note = staff.unl_note_permanent;
                    _undertakenlocationService.Update(exist_address_permanent, exist_address_permanent.unl_id);
                }
                else
                {
                    undertaken_location address_permanent = new undertaken_location();
                    address_permanent.staff_id = staff.sta_id;
                    address_permanent.unl_ward = staff.unl_ward_permanent;
                    address_permanent.unl_province = staff.unl_province_permanent;
                    address_permanent.unl_district = staff.unl_district_permanent;
                    address_permanent.unl_geocoding = staff.unl_geocoding_permanent;
                    address_permanent.unl_detail = staff.unl_detail_permanent;
                    address_permanent.unl_note = staff.unl_note_permanent;
                    address_permanent.unl_flag_center = 1;
                    _undertakenlocationService.Create(address_permanent);
                   
                }


                //update address hiện tại 
                undertaken_location exist_address_now = _undertakenlocationService.GetAllIncluing(x => x.staff_id == staff.sta_id && x.unl_flag_center == 2).FirstOrDefault();
                if (exist_address_now != null)
                {
                    exist_address_now.unl_ward = staff.unl_ward_now;
                    exist_address_now.unl_province = staff.unl_province_now;
                    exist_address_now.unl_district = staff.unl_district_now;
                    exist_address_now.unl_geocoding = staff.unl_geocoding_now;
                    exist_address_now.unl_detail = staff.unl_detail_now;
                    exist_address_now.unl_note = staff.unl_note_now;
                    _undertakenlocationService.Update(exist_address_now, exist_address_now.unl_id);
                }
                else
                {
                    //save address hiện tại 
                    undertaken_location address_now = new undertaken_location();
                    address_now.staff_id = staff.sta_id;
                    address_now.unl_ward = staff.unl_ward_now;
                    address_now.unl_province = staff.unl_province_now;
                    address_now.unl_district = staff.unl_district_now;
                    address_now.unl_geocoding = staff.unl_geocoding_now;
                    address_now.unl_detail = staff.unl_detail_now;
                    address_now.unl_note = staff.unl_note_now;
                    address_now.unl_flag_center = 2;
                    _undertakenlocationService.Create(address_now);
                }

                //update training 
                //Xóa bản ghi cũ update cái mới 
                List<training_staff> lts_training_staff_db = _trainingStaffService.GetAllIncluing(x => x.staff_id == staff.sta_id).ToList();
                List<stafftrainingjson> lts_training_v = new List<stafftrainingjson>(staff.list_training);
                foreach (stafftrainingjson tr_f in lts_training_v)
                {
                    if (Utilis.IsNumber(tr_f.tn_id))
                    {
                        int temp = 0;
                        int _id = Convert.ToInt32(tr_f.tn_id);
                        foreach (training_staff tr in lts_training_staff_db)
                        {
                            if (tr.training_id == _id)
                            {
                                //update
                                training exist_tr = _trainingService.Find(tr.training_id);

                                exist_tr.tn_content = tr_f.tn_content;
                                exist_tr.tn_end_date = tr_f.tn_end_date;
                                exist_tr.tn_name = tr_f.tn_name;
                                exist_tr.tn_start_date = tr_f.tn_start_date;
                                exist_tr.tn_purpose = tr_f.tn_purpose;
                                _trainingService.Update(exist_tr, exist_tr.tn_id);
                                training_staff exist_tr_s = _trainingStaffService.GetAllIncluing(x =>x.training_id == _id && x.staff_id == update_staff.sta_id).FirstOrDefault();
                                exist_tr_s.ts_evaluate = tr_f.ts_evaluate;
                                _trainingStaffService.Update(exist_tr_s, exist_tr_s.ts_id);
                                lts_training_staff_db.Remove(tr);
                                temp = 1;
                                break;
                            }
                        }
                        if (temp == 0)
                        {
                            //Khi view trả về những cái chọn mà k có trong db thì thêm phần này 
                            training_staff create_trs = new training_staff();
                            create_trs.staff_id = staff.sta_id;
                            create_trs.training_id = _id;
                            create_trs.ts_evaluate = tr_f.ts_evaluate;
                            _trainingStaffService.Create(create_trs);
                        }

                    }
                    else
                    {
                        //Create training 
                        training create_training = new training();
                        create_training.tn_name = tr_f.tn_name;
                        create_training.tn_content = tr_f.tn_content;
                        create_training.tn_start_date = tr_f.tn_start_date;
                        create_training.tn_end_date = tr_f.tn_end_date;
                        create_training.tn_purpose = tr_f.tn_purpose;
                        var x = _trainingService.GetLast();
                        if (x == null) create_training.tn_code = "DT000000";
                        else create_training.tn_code = Utilis.CreateCodeByCode("DT", x.tn_code, 8);
                        _trainingService.Create(create_training);
                        training tr_last = _trainingService.GetLast();
                        //Create training staff
                        training_staff create_trs = new training_staff();
                        create_trs.staff_id = staff.sta_id;
                        create_trs.training_id = tr_last.tn_id;
                        create_trs.ts_evaluate = tr_f.ts_evaluate;
                        _trainingStaffService.Create(create_trs);
                    }
                }
                foreach (training_staff trs in lts_training_staff_db)
                {
                    _trainingStaffService.Delete(trs);
                }
                //update list_undertaken_location
                List<undertaken_location> lts_ul_db = _undertakenlocationService.GetAllIncluing(x => x.staff_id == staff.sta_id && x.unl_flag_center == 0).ToList();
                List<staffundertaken_locationjson> lts_ul_v = new List<staffundertaken_locationjson>(staff.list_undertaken_location);
                foreach (staffundertaken_locationjson ul_f in lts_ul_v)
                {
                    if (Utilis.IsNumber(ul_f.unl_id))
                    {
                        int _id = Convert.ToInt32(ul_f.unl_id);
                        foreach (undertaken_location ul in lts_ul_db)
                        {
                            if (ul.unl_id == _id)
                            {
                                //update
                                undertaken_location exist_address = _undertakenlocationService.Find(_id);
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
                    }
                    else
                    {
                        //Create
                        undertaken_location create_address = new undertaken_location();
                        create_address.unl_ward = ul_f.unl_ward;
                        create_address.unl_province = ul_f.unl_province;
                        create_address.unl_district = ul_f.unl_district;
                        create_address.unl_geocoding = ul_f.unl_geocoding;
                        create_address.unl_detail = ul_f.unl_detail;
                        create_address.unl_note = ul_f.unl_note;
                        create_address.staff_id = staff.sta_id;
                        create_address.unl_flag_center = 0;
                        _undertakenlocationService.Create(create_address);
                    }
                }
                foreach (undertaken_location ul_d in lts_ul_db)
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
                var text_send = text1 + sta_username + text2 + pass_word + text3;
                BaseController.send_mail(text_send, sta_email, "New User Created!!!");
                pass_word = "";
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

        [HttpDelete]
        [Route("api/staff/delete")]
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
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nhập đúng mật khẩu mới.";
                    response.Error = "NewPassword";
                    return Ok(response);
                }
                else
                {
                    bool check = _staffservice.ChangePassword(model, id);
                    if (check == false)
                    {
                        // return response
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Mật khẩu cũ không đúng.";
                        response.Error = "OldPassword";
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
                if (staff == null)
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
            catch (Exception ex)
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
                            fileName = FileExtension.SaveFileStaffOnDiskExcel(fileData, "test", BaseController.folder(), BaseController.get_timestamp());
                        }
                        else
                        {
                            throw new Exception("File excel import không hợp lệ!");
                        }

                    }
                }
                var list = new List<staffview>();
                //fileName = "C:/inetpub/wwwroot/coerp" + fileName;
                fileName = "D:/ERP20/ERP.API" + fileName;
                var dataset = ExcelImport.ImportExcelXLS(fileName, true);
                DataTable table = (DataTable)dataset.Tables[0];
                if (table != null && table.Rows.Count > 0)
                {
                    if (table.Columns.Count != 16)
                    {
                        exitsData = "File excel import không hợp lệ!";
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = exitsData;
                        response.Data = null;
                        return Ok(response);
                    }
                    list = DataTableCmUtils.ToListof<staffview>(table);
                    foreach (staffview i in list)
                    {
                        #region["Check tồn tại"]
                        var us = _staffservice.GetAllIncluing(t => t.sta_username.Equals(i.sta_username)).FirstOrDefault();
                        if (us == null)
                        {
                            exitsData = "Đã có username '" + i.sta_username + "' tồn tại trong cơ sở dữ liệu!";
                            response.Code = HttpCode.NOT_FOUND;
                            response.Message = exitsData;
                            response.Error = "sta_username";
                        }
                        var dt = _staffservice.GetAllIncluing(y => y.sta_mobile.Equals(i.sta_mobile)).FirstOrDefault();
                        if (dt == null)
                        {
                            exitsData = "Đã có số điện thoại '" + i.sta_mobile + "' tồn tại trong cơ sở dữ liệu!";
                            response.Code = HttpCode.NOT_FOUND;
                            response.Message = exitsData;
                            response.Error = "sta_mobile";
                        }
                        #endregion

                        #region["Create staff"]
                        staff staff_create = new staff();
                        staff_create = _mapper.Map<staff>(i);
                        if (i.sta_sex_name.Trim().Contains("nam"))
                        {
                            staff_create.sta_sex = 1;
                            staff_create.sta_thumbnai = "/Uploads/Images/default/man.png";
                        }
                        else
                        {
                            staff_create.sta_sex = 2;
                            staff_create.sta_thumbnai = "/Uploads/Images/default/girl.png";
                        }
                        staff_create.group_role_id = 2;
                        staff_create.sta_status = 1;
                        staff_create.position_id = 3;
                        staff_create.department_id = 3;
                        staff_create.sta_working_status = 1;
                        staff_create.sta_type_contact = 1;
                        var x = _staffservice.GetLast();
                        if (x == null) staff_create.sta_code = "NV000000";
                        else staff_create.sta_code = Utilis.CreateCodeByCode("NV", x.sta_code, 8);
                        staff_create.sta_password = HashMd5.convertMD5(staff_create.sta_code);
                        staff_create.sta_created_date = DateTime.Now;
                        staff_create.sta_login = true;

                        _staffservice.Create(staff_create);
                        int last_id = _staffservice.GetLast().sta_id;
                        #endregion

                        #region["Xử lý địa chỉ hiện tại"]
                        string[] diachi = new string[4];
                        try
                        {
                            if(i.sta_address_now!=null)
                            {
                                var dc1 = i.sta_address_now.Trim().Split('-');
                                for (int j = 0; j < dc1.Length; j++)
                                {
                                    diachi[j] = dc1[j];
                                }
                                undertaken_location un_create = new undertaken_location();
                                if (diachi[0] != null)
                                    un_create.unl_province = diachi[0].Trim();
                                if (diachi[1] != null)
                                    un_create.unl_district = diachi[1].Trim();
                                if (diachi[2] != null)
                                    un_create.unl_ward = diachi[2].Trim();
                                if (diachi[3] != null)
                                    un_create.unl_detail = diachi[3].Trim();
                                un_create.unl_flag_center = 1;
                                un_create.staff_id = last_id;
                                if (_staffservice.Check_location(un_create))
                                {
                                    _undertakenlocationService.Create(un_create);
                                }
                            }
                        }
                        catch
                        {
                            exitsData = "Nhập lại địa chỉ nhân sự '" + i.sta_fullname + " ' !";
                            response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                            response.Message = exitsData;
                            response.Data = null;
                            return Ok(response);
                        }


                        #endregion

                        #region["Xử lý địa chỉ thường chú"]
                        string[] diachi_tc = new string[4];
                        try
                        {
                            if(i.sta_address_permanent!=null)
                            {
                                var dc = i.sta_address_permanent.Trim().Split('-');
                                for(int j =0;j<dc.Length;j++)
                                {
                                    diachi_tc[j] = dc[j];
                                }
                                undertaken_location un_create = new undertaken_location();
                                if(diachi_tc[0]!=null)
                                    un_create.unl_province = diachi_tc[0].Trim();
                                if (diachi_tc[1] != null)
                                    un_create.unl_district = diachi_tc[1].Trim();
                                if (diachi_tc[2] != null)
                                    un_create.unl_ward = diachi_tc[2].Trim();
                                if (diachi_tc[3] != null)
                                    un_create.unl_detail = diachi_tc[3].Trim();
                                un_create.unl_flag_center = 2;
                                un_create.staff_id = last_id;
                                if (_staffservice.Check_location(un_create))
                                {
                                    _undertakenlocationService.Create(un_create);
                                }
                            }
                            
                        }
                        catch
                        {
                            exitsData = "Nhập lại địa chỉ nhân sự '" + i.sta_fullname + " ' !";
                            response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                            response.Message = exitsData;
                            response.Data = null;
                            return Ok(response);
                        }


                        #endregion
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
                for (int i = 0; i < list_email.Count - 1; i++)
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
        [Route("api/staff/export")]
        public async Task<IHttpActionResult> Export(int pageNumber, int pageSize, int? status, DateTime? start_date, DateTime? end_date, string name, int? sta_working_status)
        {
            ResponseDataDTO<string> response = new ResponseDataDTO<string>();
            try
            {
                var listStaff = new List<staffview>();
                //Đưa ra danh sách staff trong trang nào đó 
                var objRT_Mst_Staff = _staffservice.ExportStaff(pageNumber, pageSize, status, start_date, end_date, name, sta_working_status);
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

        [HttpGet]
        [Route("api/staff/export_template")]
        public async Task<IHttpActionResult> ExportTemplate()
        {

            ResponseDataDTO<string> response = new ResponseDataDTO<string>();
            try
            {
                var listStaff = new List<staffview>();
                Dictionary<string, string> dicColNames = GetImportDicColumsTemplate();

                string url = "";
                string filePath = GenExcelExportFilePath(string.Format(typeof(staff).Name), ref url);

                ExcelExport.ExportToExcelTemplate(listStaff, dicColNames, filePath, string.Format("Nhân sự"));
                //Input: http://27.72.147.222:1230/TempFiles/2020-03-11/department_200311210940.xlsx
                //"D:\\BootAi\\ERP20\\ERP.API\\TempFiles\\2020-03-12\\department_200312092643.xlsx"

                filePath = filePath.Replace("\\", "/");
                int index = filePath.IndexOf("TempFiles");
                filePath = filePath.Substring(index);
                response.Code = HttpCode.OK;
                response.Message = "Đã xuất excel thành công!";
                response.Data = filePath;
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
                 {"sta_code","Mã nhân viên" },
                 {"sta_fullname","Họ và tên" },
                 {"sta_mobile","SĐT"},
                 {"sta_start_work_date","Ngày vào làm"},
                 {"sta_end_work_date","Ngày nghỉ việc"},
                 {"sta_reason_to_end_work","Lý do nghỉ việc"},
                 {"sta_type_contact_name","Chức danh"},
                 {"sta_birthday","Ngày sinh"},
                 {"sta_identity_card","CMT"},
                 {"sta_identity_card_date","Ngày cấp"},
                 {"sta_identity_card_location","Nơi cấp"},
                 {"sta_address_now","Nơi ở"},
                 {"sta_address_permanent","Nơi DK hộ khẩu thường trú"},
                 {"sta_traffic","Phương tiện đi lại"},
                 {"sta_salary","Lương"},
                 {"sta_sex_name","Giới tính"},
                 {"sta_tax_code","MST"}
            };
        }
        private Dictionary<string, string> GetImportDicColumsTemplate()
        {
            return new Dictionary<string, string>()
            {
                 {"sta_username","Tài khoản đăng nhập" },
                 {"sta_fullname","Họ và tên" },
                 {"sta_mobile","SĐT"},
                 {"sta_start_work_date","Ngày vào làm"},
                 {"sta_end_work_date","Ngày nghỉ việc"},
                 {"sta_reason_to_end_work","Lý do nghỉ việc"},
                 {"sta_birthday","Ngày sinh"},
                 {"sta_identity_card","CMT"},
                 {"sta_identity_card_date","Ngày cấp"},
                 {"sta_identity_card_location","Nơi cấp"},
                 {"sta_address_now","Nơi ở"},
                 {"sta_address_permanent","Nơi DK hộ khẩu thường trú"},
                 {"sta_traffic","Phương tiện đi lại"},
                 {"sta_salary","Lương"},
                 {"sta_sex_name","Giới tính"},
                 {"sta_tax_code","MST"}
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
            bool res = _staffservice.Exist(x => x.sta_username == _username);
            return res;
        }
        private bool check_username_update(string _username, int sta_id)
        {
            List<staff> lts_st = _staffservice.GetAllIncluing().ToList();
            staff update = _staffservice.Find(sta_id);
            lts_st.Remove(update);
            bool res = lts_st.Exists(x => x.sta_username == _username);
            return res;
        }
        private bool check_email(string _email)
        {

            bool res = _staffservice.Exist(x => x.sta_email == _email && _email != null);
            return res;
        }
        private bool check_email_update(string _email, int sta_id)
        {

            List<staff> lts_st = _staffservice.GetAllIncluing().ToList();
            staff update = _staffservice.Find(sta_id);
            lts_st.Remove(update);

            bool res = lts_st.Exists(x => x.sta_email == _email && _email != null);
            return res;
        }
        private bool check_phone(string _phone)
        {
            bool res = _staffservice.Exist(x => x.sta_mobile == _phone && _phone != null);
            return res;
        }
        private bool check_phone_update(string _phone, int sta_id)
        {
            List<staff> lts_st = _staffservice.GetAllIncluing().ToList();
            staff update = _staffservice.Find(sta_id);
            lts_st.Remove(update);
            bool res = lts_st.Exists(x => x.sta_mobile == _phone && _phone != null);
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
                if (existSocial != null)
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
