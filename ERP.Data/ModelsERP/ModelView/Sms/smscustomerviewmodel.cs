using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Sms
{
    public class smscustomerviewmodel
    {
        public int cu_id { get; set; }
        [StringLength(45)]
        public string cu_fullname { get; set; }
        public string cu_mobile { get; set; }

        [StringLength(40)]
        public string cu_email { get; set; }
        public string cu_type_name { get; set; }
        public int cu_type { get; set; }

        public string customer_group_name { get; set; }
        public int customer_group_id { get; set; }
        public string source_name { get; set; }
        public int source_id { get; set; }
       
    }
}