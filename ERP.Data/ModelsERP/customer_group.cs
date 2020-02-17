namespace ERP.Data.ModelsERP
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("customer_group")]
    public partial class customer_group
    {
        [Key]
        public int cg_id { get; set; }

        [StringLength(45)]
        public string cg_name { get; set; }

        [StringLength(120)]
        public string cg_thumbnail { get; set; }

        [Column(TypeName = "text")]
        public string cg_description { get; set; }

        public DateTime cg_created_date { get; set; }

        public int staff_id { get; set; }
    }
}
