namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sm
    {
        [Key]
        public int sms_id { get; set; }

        [StringLength(150)]
        public string sms_api_key { get; set; }

        [StringLength(150)]
        public string sms_secret_key { get; set; }

        [StringLength(50)]
        public string sms_brand_name_code { get; set; }

        [StringLength(50)]
        public string sms_call_back_url { get; set; }

        public int? company_id { get; set; }
    }
}
