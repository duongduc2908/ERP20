using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.CustomerOrder
{
    public class work_time_view
    {
        [Column(TypeName = "date")]
        public DateTime work_time { get; set; }

        public TimeSpan start_time { get; set; }

        public int[] list_staff_id { get; set; }
        
        public int customer_order_id { get; set; }
        public TimeSpan end_time { get; set; }

    }
}