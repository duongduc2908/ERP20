using ERP.Data.ModelsERP.ModelView.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView
{
    public class customerviewmodel
    {
        [Key]
        public int cu_id { get; set; }

        [StringLength(45)]
        public string cu_code { get; set; }

        [StringLength(45)]
        public string cu_thumbnail { get; set; }

        [StringLength(40)]
        public string cu_email { get; set; }
        public string cu_phone_number { get; set; }

        [StringLength(45)]
        public string cu_fullname { get; set; }

        public int? cu_type { get; set; }

        public DateTime? cu_create_date { get; set; }

        [StringLength(250)]
        public string cu_note { get; set; }

        [StringLength(50)]
        public string cu_geocoding { get; set; }

        public int? customer_group_id { get; set; }

        public byte? cu_status { get; set; }

        public int? source_id { get; set; }

        public DateTime? cu_birthday { get; set; }

        public int? cu_curator_id { get; set; }

        public int? cu_age { get; set; }

        public int? staff_id { get; set; }

        public byte? cu_flag_used { get; set; }

        public byte? cu_flag_order { get; set; }
        //Địa chỉ hiện tại 
        [StringLength(50)]
        public string sha_ward_now { get; set; }

        [StringLength(50)]
        public string sha_district_now { get; set; }

        [StringLength(50)]
        public string sha_province_now { get; set; }

        [StringLength(50)]
        public string sha_detail_now { get; set; }

        [StringLength(50)]
        public string sha_note_now { get; set; }

        [StringLength(50)]
        public string sha_geocoding_now { get; set; }
        //Danh sach so dien thoai 
        public List<customer_phoneviewmodel> list_customer_phone { get; set; }
        //Danh sách địa chỉ ship
        public List<shipaddressviewmodel> list_ship_address { get; set; }
        //Danh sach cham soc 
        public List<customertransactionviewmodel> list_transaction { get; set; }
        //Danh sach order
        public List<customerorderservicehistoryviewmodel> list_customer_order_service { get; set; }
        public List<customerorderproducthistoryviewmodel> list_customer_order_product { get; set; }

        //Name
        public string cu_type_name{ get; set; }

        public string customer_group_name { get; set; }

        public string cu_status_name { get; set; }

        public string source_name { get; set; }
        public string staff_name { get; set; }

        public string cu_flag_used_name { get; set; }

        public string cu_flag_order_name { get; set; }
        public string cu_curator_name { get; set; }
    }
}