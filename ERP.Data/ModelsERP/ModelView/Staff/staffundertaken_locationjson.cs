using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Data.ModelsERP.ModelView.Staff
{
    public class staffundertaken_locationjson
    {
        public string unl_id { get; set; }

        [StringLength(50)]
        public string unl_ward { get; set; }

        [StringLength(50)]
        public string unl_district { get; set; }

        [StringLength(50)]
        public string unl_province { get; set; }

        [StringLength(50)]
        public string unl_detail { get; set; }

        [StringLength(50)]
        public string unl_note { get; set; }

        [StringLength(50)]
        public string unl_geocoding { get; set; }

        public int? staff_id { get; set; }

        public byte? unl_flag_center { get; set; }
    }
}