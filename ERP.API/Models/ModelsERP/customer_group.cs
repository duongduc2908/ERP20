namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class customer_group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cg_id { get; set; }

        [StringLength(50)]
        public string cg_name { get; set; }

        [StringLength(120)]
        public string cg_thumbnail { get; set; }

        [StringLength(500)]
        public string cg_description { get; set; }

        public DateTime? cg_created_date { get; set; }

        public int? staff_id { get; set; }
    }
}
