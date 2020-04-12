namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class customer_phone
    {
        [Key]
        public int cp_id { get; set; }

        public byte? cp_type { get; set; }

        [StringLength(250)]
        public string cu_fullname { get; set; }

        [StringLength(50)]
        public string cp_phone_number { get; set; }

        public int? customer_id { get; set; }
        public string cp_note { get; set; }
    }
}
