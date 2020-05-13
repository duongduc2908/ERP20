namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("package")]
    public partial class package
    {
        [Key]
        public int pac_id { get; set; }
        [StringLength(45)]
        public string pac_code { get; set; }

        [StringLength(250)]
        public string pac_name { get; set; }

        [StringLength(250)]
        public string pac_icon { get; set; }

        public double? pac_price { get; set; }

        public byte? pac_status { get; set; }
    }
}
