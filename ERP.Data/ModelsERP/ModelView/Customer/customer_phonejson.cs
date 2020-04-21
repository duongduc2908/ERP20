using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Customer
{
    public class customer_phonejson
    {
        [Key]
        public string cp_id { get; set; }

        public byte? cp_type { get; set; }

        [StringLength(50)]
        public string cp_phone_number { get; set; }
        public string cp_note { get; set; }
    }
}