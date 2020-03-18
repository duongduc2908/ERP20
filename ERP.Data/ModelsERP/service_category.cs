namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class service_category
    {
        [Key]
        public int sc_id { get; set; }

        [StringLength(50)]
        public string sc_name { get; set; }

        [StringLength(500)]
        public string sc_description { get; set; }
    }
}
