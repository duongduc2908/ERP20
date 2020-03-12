namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("staff")]
    public partial class staff
    {
        [Key]
        public int sta_id { get; set; }

        [StringLength(50)]
        public string sta_code { get; set; }

        [StringLength(120)]
        public string sta_thumbnai { get; set; }

        [StringLength(45)]
        public string sta_fullname { get; set; }

        [StringLength(45)]
        public string sta_username { get; set; }

        [StringLength(120)]
        public string sta_password { get; set; }

        public byte? sta_sex { get; set; }

        public DateTime? sta_birthday { get; set; }

        [StringLength(30)]
        public string sta_email { get; set; }

        public byte? sta_status { get; set; }

        [StringLength(500)]
        public string sta_aboutme { get; set; }

        [StringLength(11)]
        public string sta_mobile { get; set; }

        [StringLength(20)]
        public string sta_identity_card { get; set; }

        public DateTime? sta_identity_card_date { get; set; }

        [StringLength(120)]
        public string sta_address { get; set; }

        public DateTime? sta_created_date { get; set; }

        public int? department_id { get; set; }

        public int? group_role_id { get; set; }

        public int? social_id { get; set; }

        [StringLength(120)]
        public string sta_hometown { get; set; }

        public int? position_id { get; set; }

        public byte? sta_leader_flag { get; set; }

        public DateTime? sta_end_work_date { get; set; }

        public DateTime? sta_start_work_date { get; set; }

        [StringLength(255)]
        public string sta_reason_to_end_work { get; set; }

        [Column(TypeName = "text")]
        public string sta_note { get; set; }

        public bool? sta_login { get; set; }
    }
}
