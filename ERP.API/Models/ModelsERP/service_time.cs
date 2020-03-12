namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class service_time
    {
        [Key]
        public int st_id { get; set; }

        public int? service_order_id { get; set; }

        public byte? st_type { get; set; }

        public DateTime? st_start_time { get; set; }

        public DateTime? st_end_time { get; set; }

        public byte? st_sun_flag { get; set; }

        public byte? st_mon_flag { get; set; }

        public byte? st_tue_flag { get; set; }

        public byte? st_wed_flag { get; set; }

        public byte? st_thu_flag { get; set; }

        public byte? st_fri_flag { get; set; }

        public byte? st_sat_flag { get; set; }

        public byte? st_repeat { get; set; }
    }
}
