using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class DeviceStaffUpdateViewModel
    {
        public int? device_id { get; set; }

        public int? des_quantity { get; set; }

        public int? des_status { get; set; }

        public string des_note { get; set; }
        public DateTime? des_date { get; set; }
    }
}