namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class transaction_evaluate
    {
        [Key]
        public int teval_id { get; set; }

        [StringLength(250)]
        public string teval_name { get; set; }

        public string teval_description { get; set; }

        public int? company_id { get; set; }
    }
}
