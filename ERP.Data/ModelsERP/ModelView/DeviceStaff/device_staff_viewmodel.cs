using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.DeviceStaff
{
    public class device_staff_viewmodel
    {
        public int? device_id { get; set; }
        public string device_name { get; set; }

        public int? des_quantity { get; set; }
        public string staff_name { get; set; }
        public int? des_status { get; set; }
        public int? dev_unit { get; set; }
        public string dev_unit_name { get; set; }
        public string des_note { get; set; }
        public DateTime? des_date { get; set; }
    }
}