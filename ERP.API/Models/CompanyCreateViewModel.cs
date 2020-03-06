using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class CompanyCreateViewModel
    {
        public CompanyCreateViewModel()
        {

        }
        

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

        public string co_bio { get; set; }

        public byte? co_type { get; set; }

        public int? co_no_of_employees { get; set; }

        public int? co_revenue { get; set; }
        public int? package_id { get; set; }
    }
}