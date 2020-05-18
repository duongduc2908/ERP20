namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class staff_brank
    {
        [Key]
        public int stb_id { get; set; }

        [StringLength(250)]
        public string stb_account { get; set; }

        [Required]
        public string stb_fullname { get; set; }

        public int? bank_branch_id { get; set; }

        public string stb_note { get; set; }

        public int? staff_id { get; set; }

        public virtual bank_branch bank_branch { get; set; }

        public virtual staff staff { get; set; }
    }
}
