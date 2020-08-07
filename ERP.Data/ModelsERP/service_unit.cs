namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class service_unit
    {
        [Key]
        public int suni_id { get; set; }

        [StringLength(250)]
        public string suni_name { get; set; }

        public string suni_description { get; set; }

        public int? company_id { get; set; }
    }
}
