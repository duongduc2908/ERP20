using ERP.Data.ModelsERP.ModelView.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Customer
{
    public class customerorderservicehistoryviewmodel
    {
        public int cuo_id { get; set; }
        public DateTime? cuo_date { get; set; }

        [StringLength(50)]
        public string cuo_code { get; set; }

        public int? staff_id { get; set; }
        public string staff_name { get; set; }
        public List<serviceviewmodel> list_service_history { get; set; }
    }
}