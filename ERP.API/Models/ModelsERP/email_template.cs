namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class email_template
    {
        [Key]
        public int emt_id { get; set; }

        [StringLength(50)]
        public string emt_code { get; set; }

        [StringLength(50)]
        public string emt_name { get; set; }

        public string emt_content { get; set; }

        public DateTime? emt_create_date { get; set; }

        public int? staff_id { get; set; }
    }
}
