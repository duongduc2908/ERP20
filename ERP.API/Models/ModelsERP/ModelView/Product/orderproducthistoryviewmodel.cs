using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Product
{
    public class orderproducthistoryviewmodel
    {
        [Key]
        public int op_id { get; set; }

        public int? op_quantity { get; set; }

        public string op_note { get; set; }

        public int? product_id { get; set; }
        public string cuo_code { get; set; }
        public string pu_unit_name { get; set; }
        
        public int? customer_order_id { get; set; }
        public DateTime? cuo_date { get; set; }
        public string cu_fullname { get; set; }
        public string sta_fullname { get; set; }

        public int? op_discount { get; set; }
    }
}