namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class customer_order
    {
        [Key]
        public int cuo_id { get; set; }

        public DateTime? cuo_date { get; set; }

        [StringLength(50)]
        public string cuo_code { get; set; }

        public int? cuo_total_price { get; set; }

        public byte? cuo_status { get; set; }

        public int? customer_id { get; set; }

        public int? cuo_discount { get; set; }

        public byte? cuo_payment_type { get; set; }

        public byte? cuo_payment_status { get; set; }

        public int? cuo_ship_tax { get; set; }

        public int? staff_id { get; set; }

        [StringLength(500)]
        public string cuo_note { get; set; }

        public int? cuo_who_support { get; set; }
        [StringLength(250)]
        public string cuo_address { get; set; }
        public byte? cuo_evaluation { get; set; }
        [StringLength(500)]
        public string cuo_feedback { get; set; }

    }
}
