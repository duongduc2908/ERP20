using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class EmailStrategyUpdateViewModel
    {
        public EmailStrategyUpdateViewModel() { }
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
    }
}