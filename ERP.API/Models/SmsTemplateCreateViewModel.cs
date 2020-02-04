using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class SmsTemplateCreateViewModel
    {
        public SmsTemplateCreateViewModel() { }
        

        [StringLength(50)]
        public string smt_code { get; set; }

        [StringLength(50)]
        public string smt_title { get; set; }

        public DateTime? smt_created_date { get; set; }

        public string smt_content { get; set; }

        public int? staff_id { get; set; }
    }
}