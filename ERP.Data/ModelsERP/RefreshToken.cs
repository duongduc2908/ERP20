namespace ERP.Data.ModelsERP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RefreshToken")]
    public partial class RefreshToken
    {
        [StringLength(50)]
        public string ID { get; set; }

        [StringLength(500)]
        public string UserName { get; set; }

        [StringLength(500)]
        public string ClientID { get; set; }

        public DateTime? IssuedTime { get; set; }

        public DateTime? ExpiredTime { get; set; }

        public string ProtectedTicket { get; set; }
    }
}
