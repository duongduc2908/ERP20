namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("supplier")]
    public partial class supplier
    {
        [Key]
        public int su_id { get; set; }

        [StringLength(50)]
        public string su_name { get; set; }

        [StringLength(50)]
        public string su_logo { get; set; }

        [StringLength(50)]
        public string su_address { get; set; }

        [StringLength(11)]
        public string su_contact_phone { get; set; }

        [StringLength(50)]
        public string su_contact_name { get; set; }

        [StringLength(11)]
        public string su_phone { get; set; }

        public int? supplier_type_id { get; set; }
        public int? company_id { get; set; }
    }
}
