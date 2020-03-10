using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class OrderProductUpdateViewModel
    {
        public OrderProductUpdateViewModel() { }
        [Key]
        public int op_id { get; set; }

        public int? op_quantity { get; set; }
        public string op_note { get; set; }
        public int? product_id { get; set; }

        public int? customer_order_id { get; set; }
        public int? op_discount { get; set; }
        public float? op_total_value { get; set; }
    }
}