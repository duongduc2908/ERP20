using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Company
{
    public class packagefunctionviewmodel
    {
        public int pac_id { get; set; }
        [StringLength(45)]
        public string pac_code { get; set; }

        [StringLength(250)]
        public string pac_name { get; set; }
        [Key]
        public int fun_id { get; set; }

        [StringLength(250)]
        public string fun_name { get; set; }

        [StringLength(250)]
        public string fun_code { get; set; }

        public double? fun_price { get; set; }
    }
}