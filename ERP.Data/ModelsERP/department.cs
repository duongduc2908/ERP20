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

        [StringLength(250)]
        public string de_name { get; set; }

        [StringLength(250)]
        public string de_thumbnail { get; set; }

        public string de_description { get; set; }

        [StringLength(250)]
        public string de_manager { get; set; }

        public int? company_id { get; set; }
    }
}
