using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class DeviceCreateViewModelJson
    {
        public string dev_name { get; set; }

        public int? dev_unit { get; set; }

        public int? dev_number { get; set; }

        public string dev_note { get; set; }
        public string dev_code { get; set; }
    }
}