namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class product_category
    {
        [Key]
        public int pc_id { get; set; }

        [StringLength(50)]
        public string pc_name { get; set; }

        [StringLength(50)]
        public string pc_thumbnail { get; set; }

        [StringLength(50)]
        public string pc_description { get; set; }

        public byte? pc_published { get; set; }

        public DateTime? pc_created_date { get; set; }
    }
}
