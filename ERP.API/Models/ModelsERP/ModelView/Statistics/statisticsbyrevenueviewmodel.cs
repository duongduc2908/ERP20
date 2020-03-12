using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Statistics
{
    public class statisticsbyrevenueviewmodel
    {
        public int? totalRevenueByDay { get; set; }
        public int? totalRevenueByMonth { get; set; }
        public int? totalRevenueByWeek { get; set; }
        public int? totalRevenue { get; set; }
    }
}