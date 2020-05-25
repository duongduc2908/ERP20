namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class relatives_staff
    {
        [Key]
        public int rels_id { get; set; }

        public string rels_fullname { get; set; }

        [StringLength(10)]
        public string rels_relatives { get; set; }

        [StringLength(10)]
        public string rels_phone { get; set; }

        public string rels_address { get; set; }

        public int? staff_id { get; set; }
    }
}
