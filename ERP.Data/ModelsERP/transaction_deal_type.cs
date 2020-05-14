namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class transaction_deal_type
    {
        [Key]
        public int trand_id { get; set; }

        public string trand_name { get; set; }

        public string trand_description { get; set; }

        public int? company_id { get; set; }
    }
}
