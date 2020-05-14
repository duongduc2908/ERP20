namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("source")]
    public partial class source
    {
        [Key]
        public int src_id { get; set; }
        [StringLength(50)]
        public string src_name { get; set; }
        public string src_description { get; set; }
        public int? company_id { get; set; }
    }
}
