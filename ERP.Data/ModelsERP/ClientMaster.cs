namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClientMaster")]
    public partial class ClientMaster
    {
        [Key]
        public int ClientKeyId { get; set; }

        [StringLength(500)]
        public string ClientID { get; set; }

        [StringLength(500)]
        public string ClientSecret { get; set; }

        [StringLength(100)]
        public string ClientName { get; set; }

        public bool Active { get; set; }

        public int? RefreshTokenLifeTime { get; set; }

        [StringLength(500)]
        public string AllowedOrigin { get; set; }
    }
}
