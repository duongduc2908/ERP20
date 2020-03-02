using ERP.Data.ModelsERP.ModelView.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView
{
    public class productviewmodel
    {
        public List<orderproducthistoryviewmodel> list_orp_history { get; set; }
        public int pu_id { get; set; }
        [StringLength(45)]
        public string pu_code { get; set; }

        [StringLength(45)]
        public string pu_name { get; set; }

        public int? pu_quantity { get; set; }

        public int? pu_buy_price { get; set; }

        public int? pu_sale_price { get; set; }

        public string pu_unit_name { get; set; }
        public int pu_unit_id { get; set; }
        public string product_category_name { get; set; }
        public string product_category_id { get; set; }
        public string provider_name { get; set; }
        public string provider_id { get; set; }
        public string pu_tax { get; set; }

        public DateTime? pu_expired_date { get; set; }

        public int? pu_weight { get; set; }

        
        public DateTime? pu_create_date { get; set; }

        public string pu_short_description { get; set; }
        [Column(TypeName = "text")]
        public string pu_description { get; set; }

    }
}