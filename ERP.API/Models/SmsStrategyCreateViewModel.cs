using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class SmsStrategyCreateViewModel
    {
        public SmsStrategyCreateViewModel() { }
        
        [StringLength(50)]
        public string smss_code { get; set; }

        [StringLength(50)]
        public string smss_title { get; set; }

        public double? smss_cost { get; set; }

        public DateTime? smss_send_date { get; set; }

        public int? smss_send_count { get; set; }

        public DateTime? smss_created_date { get; set; }

        public int? sms_id { get; set; }

        public int? sms_template_id { get; set; }

        public int? customer_group_id { get; set; }
    }
}