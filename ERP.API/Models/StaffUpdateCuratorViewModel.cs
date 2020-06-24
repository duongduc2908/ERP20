using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class StaffUpdateCuratorViewModel
    {
        public int[] list_staff_id { get; set; }
        public int? customer_group_id { get; set; }
        public int? cu_type_id { get; set; }
    }
}