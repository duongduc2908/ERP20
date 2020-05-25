using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Staff
{
    public class bonus_staffviewmodel
    {
        public int bos_id { get; set; }

        public string bos_content { get; set; }

        public string bos_title { get; set; }

        public string bos_note { get; set; }

        public int? bos_type { get; set; }
        public string bos_type_name { get; set; }

        public string bos_value { get; set; }

        public DateTime? bos_time { get; set; }

        public string bos_reason { get; set; }

        public int? staff_id { get; set; }
    }
}