namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class task
    {
        [Key]
        public int tsk_id { get; set; }

        [StringLength(250)]
        public string tsk_title { get; set; }

        public int? project_id { get; set; }

        public int? staff_id { get; set; }

        public DateTime? tsk_start_datetime { get; set; }

        public DateTime? tsk_end_datetime { get; set; }

        public byte? tsk_status { get; set; }

        [StringLength(250)]
        public string tsk_content { get; set; }
    }
}
