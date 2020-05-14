namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("department")]
    public partial class department
    {
        

        [Key]
        public int de_id { get; set; }

        [StringLength(50)]
        public string de_name { get; set; }

        [StringLength(45)]
        public string de_thumbnail { get; set; }

        [StringLength(500)]
        public string de_description { get; set; }

        [StringLength(150)]
        public string de_manager { get; set; }

        public int? de_parent_id { get; set; }

        public int? de_flag { get; set; }
        public int? company_id { get; set; }
    }
}
