namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("field")]
    public partial class field
    {
        [Key]
        public int fie_id { get; set; }

        [StringLength(50)]
        public string fie_name { get; set; }

        public string fie_description { get; set; }

        public byte? fie_type { get; set; }
    }
}
