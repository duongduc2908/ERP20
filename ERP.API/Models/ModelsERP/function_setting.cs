namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class function_setting
    {
        [Key]
        public int fs_id { get; set; }

        public DateTime? fs_create_date { get; set; }

        public int? function_id { get; set; }

        public int? department_id { get; set; }
    }
}
