namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class training_staff
    {
        [Key]
        public int ts_id { get; set; }

        public int? staff_id { get; set; }

        public int? training_id { get; set; }

        public int? ts_evaluate { get; set; }
        public byte? achieved { get; set; }
        [StringLength(50)]
        public string comment { get; set; }
    }
}
