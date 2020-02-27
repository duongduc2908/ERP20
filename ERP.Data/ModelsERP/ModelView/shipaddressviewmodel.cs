using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView
{
    public class shipaddressviewmodel
    {
        public int sha_id { get; set; }

        [StringLength(50)]
        public string sha_ward { get; set; }
        public int ward_id { get; set; }

        [StringLength(50)]
        public string sha_district { get; set; }
        public int district_id { get; set; }

        [StringLength(50)]
        public string sha_province { get; set; }
        public int province_id { get; set; }

        [StringLength(50)]
        public string sha_detail { get; set; }

        [StringLength(50)]
        public string sha_note { get; set; }

        [StringLength(50)]
        public string sha_geocoding { get; set; }
    }
}