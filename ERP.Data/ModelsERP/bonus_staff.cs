namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class bonus_staff
    {
        [Key]
        public int bos_id { get; set; }

        public string bos_content { get; set; }

        public string bos_title { get; set; }

        public string bos_note { get; set; }

        public int? bos_type { get; set; }

        [StringLength(50)]
        public string bos_value { get; set; }

        public DateTime? bos_time { get; set; }

        public string bos_reason { get; set; }

        public int? staff_id { get; set; }

    }
}
