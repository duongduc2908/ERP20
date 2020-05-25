namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("bank")]
    public partial class bank
    {
        

        [Key]
        public int ba_id { get; set; }

        [StringLength(250)]
        public string ba_code { get; set; }

        public string ba_name { get; set; }

        public string ba_description { get; set; }

        public int? bank_category_id { get; set; }
    }
}
