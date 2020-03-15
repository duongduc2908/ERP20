using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class SmsStrategyViewModelUpdate
    {
        public int smss_id { get; set; }

        public int[] customer_group_id;

        [StringLength(50)]
        public string smss_title { get; set; }

        public int? sms_template_id { get; set; }
    }
}