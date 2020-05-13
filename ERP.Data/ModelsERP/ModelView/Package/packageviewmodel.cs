using ERP.Data.ModelsERP.ModelView.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Package
{
    public class packageviewmodel
    {
        [Key]
        public int pac_id { get; set; }
        [StringLength(45)]
        public string pac_code { get; set; }

        [StringLength(250)]
        public string pac_name { get; set; }

        [StringLength(250)]
        public string pac_icon { get; set; }

        public double? pac_price { get; set; }

        public byte? pac_status { get; set; }
        public string pac_status_name { get; set; }
        public List<function> list_function { get; set; }
    }
}