using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Transaction
{
    public class transactionorderproductviewmodel
    {
        [Key]
        public int op_id { get; set; }

        public int? op_quantity { get; set; }

        public string op_note { get; set; }

        public int? product_id { get; set; }

        public int? customer_order_id { get; set; }

        public int? op_discount { get; set; }
        

        public string pu_name { get; set; }
        public string pu_unit_name { get; set; }
        public string cu_fullname { get; set; }
        public DateTime? cuo_date { get; set; }

        public string cuo_status_name { get; set; }
        public string cuo_address { get; set; }

        public double? op_total_value { get; set; }

    }
}