namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("device")]
    public partial class device
    {
        [Key]
        public int dev_id { get; set; }

        [StringLength(100)]
        public string dev_name { get; set; }

        public int? dev_unit { get; set; }

        public int? dev_number { get; set; }

        public DateTime? dev_create_date { get; set; }

        public string dev_note { get; set; }

        [StringLength(50)]
        public string dev_code { get; set; }
        public int? company_id { get; set; }
    }
}
