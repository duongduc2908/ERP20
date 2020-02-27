using ERP.Data.ModelsERP.ModelView.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView
{
    public class customerordermodelview
    {
        public List<productorderviewmodel> list_product { get; set; }
        public customerviewmodel customer { get; set; }
        public int? cuo_total_price { get; set; }

        public byte? cuo_status { get; set; }

        public int? cuo_discount { get; set; }

        public byte? cuo_payment_type { get; set; }

        public byte? cuo_payment_status { get; set; }

        public int? cuo_ship_tax { get; set; }
    }
}