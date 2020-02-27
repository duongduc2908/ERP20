using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView
{
    public class customergroupviewmodel
    {
        [Key]
        public int cg_id { get; set; }

        [StringLength(45)]
        public string cg_name { get; set; }

        [StringLength(120)]
        public string cg_thumbnail { get; set; }

        [Column(TypeName = "text")]
        public string cg_description { get; set; }

        public DateTime cg_created_date { get; set; }

        public string staff_name { get; set; }
    }
}