namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class bank_branch
    {
        
        [Key]
        public int bbr_id { get; set; }

        [StringLength(250)]
        public string bbr_code { get; set; }

        public string bbr_name { get; set; }

        public int? province_id { get; set; }

        public string bbr_address { get; set; }

        public string bbr_description { get; set; }

        public int? bank_id { get; set; }
    }
}
