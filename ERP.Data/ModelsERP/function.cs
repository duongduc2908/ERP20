namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("function")]
    public partial class function
    {

        [Key]
        public int fun_id { get; set; }

        [StringLength(250)]
        public string fun_name { get; set; }
    }
}
