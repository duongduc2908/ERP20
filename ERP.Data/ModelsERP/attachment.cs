namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class attachment
    {
        [Key]
        public int ast_id { get; set; }

        public string ast_filename { get; set; }

        public string ast_link { get; set; }

        public int? staff_id { get; set; }

        public string ast_description { get; set; }

        public string ast_note { get; set; }

        public virtual staff staff { get; set; }
    }
}
