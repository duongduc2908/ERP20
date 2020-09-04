namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class staff_type
    {
        [Key]
        public int sttp_id { get; set; }

        public string sttp_name { get; set; }

        public string sttp_description { get; set; }

        public int? sttp_order { get; set; }

        public int? company_id { get; set; }
    }
}
