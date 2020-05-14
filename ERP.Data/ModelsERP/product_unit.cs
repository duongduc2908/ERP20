namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class product_unit
    {
        [Key]
        public int puni_id { get; set; }

        [StringLength(250)]
        public string puni_name { get; set; }

        public string punit_description { get; set; }

        public int? company_id { get; set; }
    }
}
