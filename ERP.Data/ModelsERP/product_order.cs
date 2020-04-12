namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class product_order
    {
        [Key]
        public int po_id { get; set; }

        public int? po_quantity { get; set; }

        public int? product_id { get; set; }

        public int? staff_order_id { get; set; }

        [StringLength(50)]
        public string po_note { get; set; }

        public virtual product product { get; set; }
    }
}
