namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class staff_work_time
    {
        [Key]
        public int sw_id { get; set; }

        public TimeSpan? sw_time_start { get; set; }

        public TimeSpan? sw_time_end { get; set; }

        public int staff_id { get; set; }

        [StringLength(3)]
        public string sw_day_flag { get; set; }
    }
}
