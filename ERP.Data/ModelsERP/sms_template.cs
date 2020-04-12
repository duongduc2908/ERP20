namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sms_template
    {
        [Key]
        public int smt_id { get; set; }

        [StringLength(50)]
        public string smt_code { get; set; }

        [StringLength(50)]
        public string smt_title { get; set; }

        public DateTime? smt_created_date { get; set; }

        public string smt_content { get; set; }

        public int? staff_id { get; set; }

        public int? smt_modifier { get; set; }

        public DateTime? smt_modify_time { get; set; }

        public virtual staff staff { get; set; }
    }
}
