using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class ProductCreateViewModel
    {
        public ProductCreateViewModel() { }

        [StringLength(50)]
        public string pu_code { get; set; }

        public int? pu_quantity { get; set; }

        [StringLength(50)]
        public string pu_buy_price { get; set; }

        [StringLength(50)]
        public string pu_sale_price { get; set; }

        public int? pu_saleoff { get; set; }

        public int? pu_short_description { get; set; }

        public DateTime? pu_create_date { get; set; }

        public DateTime? pu_update_date { get; set; }

        public string pu_description { get; set; }

        public byte? pu_rate { get; set; }

        public byte? pu_unit { get; set; }

        public byte? pu_status { get; set; }

        public byte? pu_size { get; set; }

        public int? product_category_id { get; set; }

        public int? provider_id { get; set; }
    }
}