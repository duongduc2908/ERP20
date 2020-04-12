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

        public bool? st_sun_flag { get; set; }

        public bool? st_mon_flag { get; set; }

        public bool? st_tue_flag { get; set; }

        public bool? st_wed_flag { get; set; }

        public bool? st_thu_flag { get; set; }

        public bool? st_fri_flag { get; set; }

        public bool? st_sat_flag { get; set; }

        public int? staff_id { get; set; }
    }
}
