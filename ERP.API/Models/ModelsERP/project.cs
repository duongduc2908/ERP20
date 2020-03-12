namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("project")]
    public partial class project
    {
        [Key]
        public int pro_id { get; set; }

        [StringLength(250)]
        public string pro_name { get; set; }

        public byte? pro_status { get; set; }
    }
}
