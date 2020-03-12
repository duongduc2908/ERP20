namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class field_template
    {
        [Key]
        public int fit_id { get; set; }

        public int? sms_template_id { get; set; }

        public int? field_id { get; set; }

        public int? email_template_id { get; set; }
    }
}
