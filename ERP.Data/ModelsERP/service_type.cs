namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class service_type
    {
        [Key]
        public int styp_id { get; set; }

        [StringLength(250)]
        public string styp_name { get; set; }

        public string styp_description { get; set; }

        public int? company_id { get; set; }
    }
}
