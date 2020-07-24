namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class device_staff
    {
        [Key]
        public int des_id { get; set; }

        public int? device_id { get; set; }

        public int? staff_id { get; set; }

        public int? des_quantity { get; set; }

        public int? des_status { get; set; }

        public string des_note { get; set; }

        public DateTime? des_date { get; set; }
    }
}
