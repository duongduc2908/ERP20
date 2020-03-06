namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("notification")]
    public partial class notification
    {
        [Key]
        public int ntf_id { get; set; }

        [StringLength(50)]
        public string ntf_title { get; set; }

        public string ntf_description { get; set; }

        public DateTime? ntf_datetime { get; set; }

        public DateTime? ntf_confim_datetime { get; set; }

        public int? staff_id { get; set; }
        public int? ntf_who_sent { get; set; }
        public byte? ntf_status { get; set; }
        public byte? ntf_level { get; set; }
    }
}
