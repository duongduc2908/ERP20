using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class ExecutorUpdateViewModel
    {
        public ExecutorUpdateViewModel() { }
        [Key]
        public int exe_id { get; set; }

        public int? customer_order_id { get; set; }

        public int? staff_id { get; set; }

        public int? service_time_id { get; set; }

        public DateTime? work_time { get; set; }
    }


}