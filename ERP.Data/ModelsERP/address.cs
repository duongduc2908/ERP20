namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("address")]
    public partial class address
    {
        [Key]
        public int add_id { get; set; }

        [StringLength(50)]
        public string add_ward { get; set; }

        [StringLength(50)]
        public string add_district { get; set; }

        [StringLength(50)]
        public string add_city { get; set; }

        [StringLength(50)]
        public string add_detail { get; set; }

        [StringLength(50)]
        public string add_note { get; set; }

        [StringLength(50)]
        public string add_geocoding { get; set; }

        public int? customer_id { get; set; }

        public int? staff_id { get; set; }
    }
}
