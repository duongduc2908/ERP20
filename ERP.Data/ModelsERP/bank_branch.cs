namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class bank_branch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public bank_branch()
        {
            staff_brank = new HashSet<staff_brank>();
        }

        [Key]
        public int bbr_id { get; set; }

        [StringLength(250)]
        public string bbr_code { get; set; }

        public string bbr_name { get; set; }

        public int? province_id { get; set; }

        public string bbr_address { get; set; }

        public string bbr_description { get; set; }

        public int? bank_id { get; set; }

        public virtual bank bank { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<staff_brank> staff_brank { get; set; }
    }
}
