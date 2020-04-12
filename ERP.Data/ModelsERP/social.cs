namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("social")]
    public partial class social
    {
        [Key]
        public int soc_id { get; set; }

        [StringLength(200)]
        public string soc_facebook { get; set; }

        [StringLength(200)]
        public string soc_twitter { get; set; }

        [StringLength(200)]
        public string soc_instagram { get; set; }

        [StringLength(200)]
        public string soc_linkedin { get; set; }

        [StringLength(200)]
        public string soc_skype { get; set; }

        [StringLength(200)]
        public string soc_github { get; set; }

        public int? customer_id { get; set; }

        public int? staff_id { get; set; }

        public virtual customer customer { get; set; }

        public virtual staff staff { get; set; }
    }
}
