namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class group_role
    {

        [Key]
        public int gr_id { get; set; }

        [StringLength(20)]
        public string gr_name { get; set; }

        [StringLength(50)]
        public string gr_thumbnail { get; set; }

        [StringLength(50)]
        public string gr_description { get; set; }

        public byte? gr_status { get; set; }
        public int? company_id { get; set; }
    }
}
