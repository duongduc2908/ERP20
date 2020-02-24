namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class order_product
    {
        [Key]
        public int op_id { get; set; }

        public int? op_quantity { get; set; }

        public string op_note { get; set; }

        public int? product_id { get; set; }

        public int? customer_order_id { get; set; }

        public int? op_discount { get; set; }
    }
}
