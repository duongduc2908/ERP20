namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class order_service
    {
        [Key]
        public int os_id { get; set; }

        public byte? os_evaluation { get; set; }

        public int? service_id { get; set; }

        public int? customer_order_id { get; set; }
        public int? se_number { get; set; }

        public byte? os_show_as { get; set; }

        public int? os_reminder { get; set; }

        public int? os_discount { get; set; }
        public int? os_quantity { get; set; }
    }
}
