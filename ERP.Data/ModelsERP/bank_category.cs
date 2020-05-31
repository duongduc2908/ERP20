namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class bank_category
    {

        [Key]
        public int bac_id { get; set; }

        public string bac_name { get; set; }

        public string bac_description { get; set; }
    }
}
