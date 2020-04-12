namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class email_strategy
    {
        [Key]
        public int ems_id { get; set; }

        [StringLength(50)]
        public string ems_code { get; set; }

        [StringLength(250)]
        public string ems_name { get; set; }

        public double? ems_cost { get; set; }

        public DateTime? ems_send_date { get; set; }

        public int? ems_send_count { get; set; }

        public int? ems_click_count { get; set; }

        public int? ems_recevied_count { get; set; }

        public int? ems_open_count { get; set; }

        public byte? ems_type { get; set; }

        public DateTime? ems_create_date { get; set; }

        public int? email_id { get; set; }

        public int? email_template_id { get; set; }

        public int? customer_group_id { get; set; }

        public virtual customer_group customer_group { get; set; }

        public virtual email email { get; set; }

        public virtual email_template email_template { get; set; }
    }
}
