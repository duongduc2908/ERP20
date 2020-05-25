using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Staff
{
    public class relatives_staffviewmodel
    {
        public int rels_id { get; set; }

        public string rels_fullname { get; set; }

        public string rels_relatives { get; set; }

        public string rels_phone { get; set; }

        public string rels_address { get; set; }

        public int? staff_id { get; set; }
    }
}