using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Staff
{
    public class staff_time
    {
        public TimeSpan? sw_time_start { get; set; }

        public TimeSpan? sw_time_end { get; set; }

        [StringLength(3)]
        public string sw_day_flag { get; set; }
    }
}