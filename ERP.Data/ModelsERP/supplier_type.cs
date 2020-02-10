namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class supplier_type
    {
        [Key]
        public int sut_id { get; set; }

        [StringLength(50)]
        public string sut_name { get; set; }

        [StringLength(50)]
        public string sut_description { get; set; }
    }
}
