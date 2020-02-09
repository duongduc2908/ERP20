using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView
{
    public class customerorderviewmodel
    {
       

        public DateTime? cuo_date { get; set; }

        [StringLength(50)]
        public string cuo_code { get; set; }

        public int? cuo_total_price { get; set; }
        [StringLength(50)]
        public string cuo_status { get; set; }

        public int? customer_id { get; set; }

        public int? cuo_discount { get; set; }
        [StringLength(50)]
        public string cuo_payment_type { get; set; }
        [StringLength(50)]
        public string cuo_payment_status { get; set; }

        public int? cuo_ship_tax { get; set; }
        public int? staff_id { get; set; }
    }
}