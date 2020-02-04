using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class EmailTemplateCreateViewModel
    {
        public EmailTemplateCreateViewModel() { }
        

        [StringLength(50)]
        public string emt_code { get; set; }

        [StringLength(50)]
        public string emt_name { get; set; }

        public string emt_content { get; set; }

        public DateTime? emt_create_date { get; set; }

        public int? staff_id { get; set; }
    }
}