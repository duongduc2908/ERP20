using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class CustomerGroupCreateViewModel
    {

        
        public int cg_id { get; set; }

        [StringLength(50)]
        public string cg_name { get; set; }

        [StringLength(120)]
        public string cg_thumbnail { get; set; }

        public string cg_description { get; set; }

        public DateTime? cg_created_date { get; set; }

        public int? staff_id { get; set; }
    }
}