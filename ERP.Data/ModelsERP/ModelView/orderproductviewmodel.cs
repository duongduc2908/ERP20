using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView
{
    public class orderproductviewmodel
    {
        public List<shipaddressviewmodel> list_address { get; set; }
        [StringLength(50)]

        public int? op_quantity { get; set; }

        public byte? op_status { get; set; }
        public byte? pu_unit { get; set; }

        public string pu_code { get; set; }
        public string pu_name { get; set; }

        public int? customer_order_id { get; set; }
        public int? pu_buy_price { get; set; }
    }
}