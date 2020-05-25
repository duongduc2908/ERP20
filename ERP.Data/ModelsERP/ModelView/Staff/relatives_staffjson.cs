using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Staff
{
    public class relatives_staffjson
    {
        [Key]
        public string rels_id { get; set; }

        public string rels_fullname { get; set; }

        [StringLength(10)]
        public string rels_relatives { get; set; }

        [StringLength(10)]
        public string rels_phone { get; set; }

        public string rels_address { get; set; }

        public int? staff_id { get; set; }
    }
}