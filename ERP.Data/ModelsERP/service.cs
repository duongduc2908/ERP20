namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("service")]
    public partial class service
    {

        [Key]
        public int se_id { get; set; }

        [StringLength(10)]
        public string se_code { get; set; }

        public byte? se_type { get; set; }

        [StringLength(100)]
        public string se_name { get; set; }

        [Column(TypeName = "text")]
        public string se_description { get; set; }

        [StringLength(45)]
        public string se_thumbnai { get; set; }

        public int? se_price { get; set; }

        public int? se_saleoff { get; set; }

        public int? service_category_id { get; set; }

        [StringLength(200)]
        public string se_note { get; set; }
    }
}
