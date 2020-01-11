using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class CustomerUpdateViewModel
    {
        public CustomerUpdateViewModel()
        {

        }
        
        public int cu_id { get; set; }

        [StringLength(12)]
        public string cu_code { get; set; }

        [StringLength(20)]
        public string cu_mobile { get; set; }

        [StringLength(50)]
        public string cu_thumbnail { get; set; }

        [StringLength(50)]
        public string cu_email { get; set; }

        [StringLength(50)]
        public string cu_fullname { get; set; }

        public byte? cu_type { get; set; }

        [StringLength(250)]
        public string cu_address { get; set; }

        public DateTime? cu_create_date { get; set; }

        public string cu_note { get; set; }

        public int? social_id { get; set; }

        public int? customer_group_id { get; set; }

        public int? customer_address_id { get; set; }

        public int? source_id { get; set; }
    }
}