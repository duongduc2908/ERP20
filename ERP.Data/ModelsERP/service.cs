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

        [StringLength(50)]
        public string se_type { get; set; }

        [StringLength(250)]
        public string se_name { get; set; }

        public string se_description { get; set; }

        [StringLength(250)]
        public string se_thumbnai { get; set; }

        public double? se_price { get; set; }

        public int? service_category_id { get; set; }
    }
}
