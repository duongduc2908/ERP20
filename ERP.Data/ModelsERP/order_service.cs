namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class order_service
    {
        [Key]
        public int os_id { get; set; }

        [StringLength(50)]
        public string os_code { get; set; }

        [StringLength(250)]
        public string os_before_image { get; set; }

        [StringLength(250)]
        public string os_after_image { get; set; }

        [StringLength(250)]
        public string os_requiment { get; set; }

        public byte? os_evaluation { get; set; }

        public DateTime? os_create_date { get; set; }

        public byte? os_status { get; set; }

        public int? customer_id { get; set; }

        public int? service_id { get; set; }

        public int? staff_id { get; set; }
    }
}
