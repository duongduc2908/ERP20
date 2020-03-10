using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Statistics
{
    public class statisticsorderviewmodel
    {
        public int cuo_id { get; set; }
        public string cuo_code { get; set; }
        public byte? cuo_status { get; set; }
        public string cuo_status_name { get; set; }
        public DateTime? cuo_date { get; set; }
        public float? op_total_value { get; set; }
        public int pu_id { get; set; }
        public string pu_name { get; set; }

    }
}