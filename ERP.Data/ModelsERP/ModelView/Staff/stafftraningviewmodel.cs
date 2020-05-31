using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Staff
{
    public class stafftraningviewmodel
    {
        [Key]
        public int tn_id { get; set; }

        [StringLength(50)]
        public string tn_code { get; set; }

        [StringLength(250)]
        public string tn_name { get; set; }

        public string tn_content { get; set; }

        public DateTime? tn_start_date { get; set; }

        public DateTime? tn_end_date { get; set; }

        public string tn_purpose { get; set; }
        public int? ts_evaluate { get; set; }
        public string ts_evaluate_name { get; set; }
    }
}