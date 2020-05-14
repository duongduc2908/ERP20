namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class transaction_priority
    {
        [Key]
        public int tpro_id { get; set; }

        [StringLength(250)]
        public string tpro_name { get; set; }

        public string tpro_description { get; set; }

        public int? company_id { get; set; }
    }
}
