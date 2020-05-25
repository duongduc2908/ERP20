using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Staff
{
    public class staff_bankjson
    {
        [Key]
        public string stb_id { get; set; }

        [StringLength(250)]
        public string stb_account { get; set; }

        [Required]
        public string stb_fullname { get; set; }

        public int? bank_branch_id { get; set; }

        public string stb_note { get; set; }

        public int? staff_id { get; set; }
    }
}