using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class DepartmentCreateViewModel
    {
        public DepartmentCreateViewModel() { }
        [Key]
        public int de_id { get; set; }

        [StringLength(45)]
        public string de_name { get; set; }

        [StringLength(45)]
        public string de_thumbnail { get; set; }

        [StringLength(45)]
        public string de_description { get; set; }

        [StringLength(45)]
        public string de_manager { get; set; }

        public int? company_id { get; set; }
    }
}