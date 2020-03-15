using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Sms
{
    public class smstemplatestrategyviewmodel
    {
        public int smt_id { get; set; } 
        [StringLength(50)]
        public string smt_title { get; set; }
        public DateTime? smt_created_date { get; set; }
        public int? staff_id { get; set; }
        public string staff_name { get; set; }
        public DateTime? smt_modify_time { get; set; }
        public int? smt_modifier { get; set; }
        public string smt_modify_name { get; set; }
    }
}