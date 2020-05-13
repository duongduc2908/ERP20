using ERP.Data.ModelsERP.ModelView.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class CustomerUpdateViewModelJson
    {
        public int cu_id { get; set; }
        //Thồng tin chung 
        [StringLength(40)]
        public string cu_email { get; set; }

        [StringLength(45)]
        public string cu_fullname { get; set; }

        public int? cu_type { get; set; }

        [StringLength(250)]
        public string cu_note { get; set; }

        public int? customer_group_id { get; set; }

        public byte? cu_status { get; set; }

        public int? source_id { get; set; }

        public DateTime? cu_birthday { get; set; }

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
        public List<customer_phonejson> list_customer_phone { get; set; }
        //Danh sách địa chỉ ship
        public List<customer_ship_addressjson> list_ship_address { get; set; }
    }
}