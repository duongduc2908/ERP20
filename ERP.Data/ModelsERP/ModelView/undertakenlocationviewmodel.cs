using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView
{
    public class undertakenlocationviewmodel
    {
        public int unl_id { get; set; }

        [StringLength(50)]
        public string unl_ward { get; set; }
        public int ward_id { get; set; }

        [StringLength(50)]
        public string unl_district { get; set; }
        public int district_id { get; set; }

        [StringLength(50)]
        public string unl_province { get; set; }
        public int province_id { get; set; }

        [StringLength(50)]
        public string unl_detail { get; set; }

        [StringLength(50)]
        public string unl_note { get; set; }

        [StringLength(50)]
        public string unl_geocoding { get; set; }
    }
}