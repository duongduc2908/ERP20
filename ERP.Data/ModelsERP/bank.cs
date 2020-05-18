namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("bank")]
    public partial class bank
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public bank()
        {
            bank_branch = new HashSet<bank_branch>();
        }

        [Key]
        public int ba_id { get; set; }

        [StringLength(250)]
        public string ba_code { get; set; }

        public string ba_name { get; set; }

        public string ba_description { get; set; }

        public int? bank_category_id { get; set; }

        public virtual bank_category bank_category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bank_branch> bank_branch { get; set; }
    }
}
