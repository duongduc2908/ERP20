namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class device_unit
    {
        [Key]
        public int deuni_id { get; set; }

        [StringLength(100)]
        public string deuni_name { get; set; }

        public string deuni_description { get; set; }

        public int? company_id { get; set; }
    }
}
