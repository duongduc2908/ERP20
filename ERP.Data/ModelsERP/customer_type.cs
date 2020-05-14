namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class customer_type
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cut_id { get; set; }

        [StringLength(250)]
        public string cut_name { get; set; }

        public string cut_description { get; set; }

        public int? company_id { get; set; }
    }
}
