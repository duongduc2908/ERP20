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

        [Key]
        public int cu_id { get; set; }

        [StringLength(45)]
        public string cu_code { get; set; }

        [StringLength(10)]
        public string cu_mobile { get; set; }

        [StringLength(45)]
        public string cu_thumbnail { get; set; }

        [StringLength(40)]
        public string cu_email { get; set; }

        [StringLength(45)]
        public string cu_fullname { get; set; }

        public int? cu_type { get; set; }

        [StringLength(120)]
        public string cu_address { get; set; }

        public DateTime? cu_create_date { get; set; }

        [StringLength(250)]
        public string cu_note { get; set; }

        public string cu_geocoding { get; set; }

        public int? customer_group_id { get; set; }

        public byte? cu_status { get; set; }

        public int? source_id { get; set; }

        public DateTime? cu_birthday { get; set; }

        public int? staff_id { get; set; }
        public int? cu_curator_id { get; set; }
        public int? cu_age { get; set; }

    }
}