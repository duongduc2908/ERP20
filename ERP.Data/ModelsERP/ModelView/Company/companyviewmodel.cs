using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Company
{
    public class companyviewmodel
    {
        [Key]
        public int co_id { get; set; }
        [StringLength(45)]
        public string co_code { get; set; }

        [StringLength(250)]
        public string co_name { get; set; }

        [StringLength(500)]
        public string co_vision { get; set; }

        [StringLength(250)]
        public string co_address { get; set; }

        [StringLength(500)]
        public string co_mission { get; set; }

        public string co_target { get; set; }

        public string co_description { get; set; }

        [StringLength(250)]
        public string co_logo { get; set; }

        public int? co_no_of_employees { get; set; }
        public string sta_name { get; set; }

        public int? co_revenue { get; set; }

        public double? co_price { get; set; }

        public int? co_duration { get; set; }
        public List<packagefunctionviewmodel> list_function { get; set; }
    }
}