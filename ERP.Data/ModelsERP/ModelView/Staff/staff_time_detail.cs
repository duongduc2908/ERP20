using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Staff
{
    public class staff_time_detail
    {
        public int? staff_id { get; set; }
        public List<staff_time> list_time { get; set; }
        public staff_time_detail()
        {
            list_time = new List<staff_time>();
        }
    }
}