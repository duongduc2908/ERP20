using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Transaction
{
    public class transactionorderserviceviewmodel
    {
        [Key]
        public int se_id { get; set; }

        [StringLength(10)]
        public string se_code { get; set; }

        public int se_type { get; set; }
        public string se_type_name { get; set; }

        [StringLength(100)]
        public string se_name { get; set; }

        public string se_description { get; set; }

        [StringLength(45)]
        public string se_thumbnai { get; set; }

        public int? se_price { get; set; }

        public int? se_saleoff { get; set; }

        public int? service_category_id { get; set; }
        public string service_category_name { get; set; }

        [StringLength(200)]
        public string se_note { get; set; }
        public DateTime? cuo_date { get; set; }
    }
}