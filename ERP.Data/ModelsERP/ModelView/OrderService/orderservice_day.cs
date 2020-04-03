using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.OrderService
{
    public class orderservice_day
    {
        public TimeSpan start_time { get; set; }
        public TimeSpan end_time { get; set; }
        public string service_name { get; set; }
    }
}