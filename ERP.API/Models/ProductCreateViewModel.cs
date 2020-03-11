using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class ProductCreateViewModel
    {
        public ProductCreateViewModel() { }

        

        [StringLength(45)]
        public string pu_code { get; set; }

        [StringLength(45)]
        public string pu_name { get; set; }

        public int? pu_quantity { get; set; }

        public int? pu_buy_price { get; set; }

        public int? pu_sale_price { get; set; }

        public int? pu_saleoff { get; set; }

        [StringLength(200)]
        public string pu_short_description { get; set; }

        public DateTime? pu_create_date { get; set; }

        public DateTime? pu_update_date { get; set; }

        [Column(TypeName = "text")]
        public string pu_description { get; set; }

        public byte? pu_unit { get; set; }

        public int? product_category_id { get; set; }

        public int? provider_id { get; set; }

        public int? pu_tax { get; set; }

        public DateTime? pu_expired_date { get; set; }

        public int? pu_weight { get; set; }
        public string pu_thumbnail { get; set; }
    }
}