using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.OrderService
{
    public class order_service_view
    {
        public order_service_view()
        {
            list_service = new List<orderservice_day>();
        }
        [Column(TypeName = "date")]
        public DateTime work_time { get; set; }
        public List<orderservice_day> list_service { get; set; }
    }
}