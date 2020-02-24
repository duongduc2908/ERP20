using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView
{
    public class addressviewmodel
    {
        [Key]
        public int add_id { get; set; }

        [StringLength(50)]
        public string add_ward { get; set; }
        public int ward_id { get; set; }

        [StringLength(50)]
        public string add_district { get; set; }
        public int district_id { get; set; }

        [StringLength(50)]
        public string add_province { get; set; }
        public int province_id { get; set; }

        [StringLength(50)]
        public string add_detail { get; set; }

        [StringLength(50)]
        public string add_note { get; set; }

        [StringLength(50)]
        public string add_geocoding { get; set; }

        public int? customer_id { get; set; }

        public int? staff_id { get; set; }
    }
}