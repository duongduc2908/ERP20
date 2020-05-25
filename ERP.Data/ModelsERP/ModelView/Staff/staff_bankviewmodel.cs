using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Staff
{
    public class staff_bankviewmodel
    {
        public int stb_id { get; set; }

        public string stb_account { get; set; }

        public string stb_fullname { get; set; }

        public int? bank_branch_id { get; set; }
        public string bank_branch_name { get; set; }

        public string stb_note { get; set; }

        public int? staff_id { get; set; }

        public int? bank_id { get; set; }
        public string bank_name { get; set; }
        public int? bank_category_id { get; set; }
        public string bank_category_name { get; set; }
    }
}