using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Device
{
    public class deviceviewmodel
    {
        public int dev_id { get; set; }
        public string dev_name { get; set; }

        public int? dev_unit { get; set; }
        public string dev_unit_name { get; set; }

        public int? dev_number { get; set; }

        public DateTime? dev_create_date { get; set; }

        public string dev_note { get; set; }

        public string dev_code { get; set; }
    }
}