using ERP.Data.ModelsERP.ModelView.DeviceStaff;
using ERP.Data.ModelsERP.ModelView.Staff;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView
{
    public class staffviewmodel
    {
        //Thông tin thời gian loại hợp đồng partime hay fulltime 9 trường 
        public List<staff_work_time> list_staff_work_time{ get; set; }
        [StringLength(3)]
        public string sw_day_flag { get; set; }
        public int sw_id { get; set; }

        //Thông tin chung 
        public int sta_id { get; set; }
        public string sta_code { get; set; }

        [StringLength(120)]
        public string sta_thumbnai { get; set; }

        [StringLength(45)]
        public string sta_fullname { get; set; }

        [StringLength(45)]
        public string sta_username { get; set; }

        [StringLength(120)]
        public string sta_password { get; set; }

        public byte? sta_sex { get; set; }
       
        public DateTime? sta_start_work_date { get; set; }

        public DateTime? sta_end_work_date { get; set; }

        [StringLength(255)]
        public string sta_reason_to_end_work { get; set; }

        public int? position_id { get; set; }

        public DateTime? sta_birthday { get; set; }

        [StringLength(100)]
        public string sta_email { get; set; }

        [StringLength(20)]
        public string sta_identity_card { get; set; }

        public DateTime? sta_identity_card_date { get; set; }

        public DateTime? sta_identity_card_date_end { get; set; }
        [StringLength(250)]
        public string sta_identity_card_location { get; set; }

        public byte? sta_status { get; set; }

        [StringLength(500)]
        public string sta_aboutme { get; set; }

        [StringLength(11)]
        public string sta_mobile { get; set; }

        public int? department_id { get; set; }

        public int? group_role_id { get; set; }
        public bool? sta_login { get; set; }

        [Column(TypeName = "text")]
        public string sta_note { get; set; }

        public DateTime? sta_created_date { get; set; }

        public int? social_id { get; set; }

        public byte? sta_leader_flag { get; set; }

        [StringLength(50)]
        public string sta_traffic { get; set; }

        public int? sta_salary { get; set; }
        [StringLength(50)]
        public string sta_health_card { get; set; }

        [StringLength(50)]
        public string sta_social_insurance { get; set; }

        [StringLength(100)]
        public string sta_tax_code { get; set; }
        public byte? sta_working_status { get; set; }

        public byte? sta_type_contact { get; set; }
        //Địa chỉ thường trú
        public int unl_id_permanent { get; set; }
        public string unl_ward_permanent { get; set; }

        [StringLength(50)]
        public string unl_district_permanent { get; set; }

        [StringLength(50)]
        public string unl_province_permanent { get; set; }

        [StringLength(50)]
        public string unl_detail_permanent { get; set; }

        [StringLength(50)]
        public string unl_note_permanent { get; set; }

        [StringLength(50)]
        public string unl_geocoding_permanent { get; set; }
        //Địa chỉ hiện tại 
        public int unl_id_now { get; set; }
        [StringLength(50)]
        public string unl_ward_now { get; set; }

        [StringLength(50)]
        public string unl_district_now { get; set; }

        [StringLength(50)]
        public string unl_province_now { get; set; }

        [StringLength(50)]
        public string unl_detail_now { get; set; }

        [StringLength(50)]
        public string unl_note_now { get; set; }

        [StringLength(50)]
        public string unl_geocoding_now { get; set; }

        public byte? achieved { get; set; }
        public string achieved_name { get; set; }
        [StringLength(50)]
        public string comment { get; set; }
        //Danh sách đào tạo 
        public List<stafftraningviewmodel> list_training { get; set; }
        //Danh sách địa chỉ làm việc 
        public List<undertakenlocationviewmodel> list_undertaken_location { get; set; }
        public List<staff_bankviewmodel> list_bank { get; set; }
        public List<relatives_staffviewmodel> list_relatives { get; set; }
        public List<bonus_staffviewmodel> list_bonus { get; set; }
        public List<attachmentviewmodel> list_attachments { get; set; }
        //Các trường bổ sung trong view search
        public string sta_sex_name { get; set; }
        public string position_name { get; set; }
        public string department_name { get; set; }
        public string group_role_name { get; set; }
        public string status_name { get; set; }
        public string sta_working_status_name { get; set; }
        // Danh sách vật tư
        public List<device_staff_viewmodel> list_devices { get; set; }
    }
}