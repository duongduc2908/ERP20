using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.ExportDB
{
    public class customerviewexport
    {
        //Thồng tin chung 
        public string cu_mobile { get; set; }
        public string cu_code { get; set; }
        [StringLength(40)]
        public string cu_email { get; set; }

        [StringLength(45)]
        public string cu_fullname { get; set; }
        [StringLength(45)]
        public string cu_thumbnail { get; set; }


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
        public string cu_address { get; set; }

        [StringLength(50)]
        public string sha_geocoding_now { get; set; }
        public string cu_type_name { get; set; }
        public string customer_group_name { get; set; }
        public string cu_status_name { get; set; }
        public string source_name { get; set; }
        public string cu_flag_used_name { get; set; }
        public string cu_flag_order_name{ get; set; }
    }
}