namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("executor")]
    public partial class executor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int exe_id { get; set; }

        public int? customer_order_id { get; set; }

        public int? staff_id { get; set; }

        public int? service_time_id { get; set; }

        public DateTime? start_datetime { get; set; }

        public DateTime? end_datetime { get; set; }
    }
}
