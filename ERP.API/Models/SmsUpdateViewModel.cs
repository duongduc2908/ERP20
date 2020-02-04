using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class SmsUpdateViewModel
    {
        public SmsUpdateViewModel() { }

        [Key]
        public int sms_id { get; set; }
        [StringLength(150)]
        public string sms_api_key { get; set; }

        [StringLength(150)]
        public string sms_secret_key { get; set; }

        [StringLength(50)]
        public string sms_brand_name_code { get; set; }

        [StringLength(50)]
        public string sms_call_back_url { get; set; }

        public int? company_id { get; set; }
    }
}