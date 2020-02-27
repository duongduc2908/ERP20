namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ship_address
    {
        [Key]
        public int sha_id { get; set; }

        [StringLength(50)]
        public string sha_ward { get; set; }

        [StringLength(50)]
        public string sha_district { get; set; }

        [StringLength(50)]
        public string sha_province { get; set; }

        [StringLength(50)]
        public string sha_detail { get; set; }

        [StringLength(50)]
        public string sha_note { get; set; }

        [StringLength(50)]
        public string sha_geocoding { get; set; }

        public int? customer_id { get; set; }
    }
}
