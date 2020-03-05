using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView
{
    public class productorderviewmodel
    {
        public int op_id { get; set; }
       
        public int? op_quantity { get; set; }

        public string op_note { get; set; }

        public int? product_id { get; set; }
        public string pu_name { get; set; }

        public int? op_discount { get; set; }
        public byte? pu_unit { get; set; }
        public string pu_unit_name { get; set; }
        public int? pu_sale_price { get; set; }

    }
}