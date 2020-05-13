namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class company_funtion
    {
        [Key]
        public int cop_id { get; set; }

        public int? fun_id { get; set; }

        public int? company_id { get; set; }
    }
}
