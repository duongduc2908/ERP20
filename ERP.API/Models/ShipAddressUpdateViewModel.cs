using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class ShipAddressUpdateViewModel
    {
        public int sha_id { get; set; }

        [StringLength(50)]
        public string sha_ward { get; set; }

        [StringLength(50)]
        public string sha_district { get; set; }

        [StringLength(50)]
        public string sha_province { get; set; }

        [StringLength(50)]
        public string sha_detail { get; set; }

        [StringLength(50)]
        public string sha_note { get; set; }

        [StringLength(50)]
        public string sha_geocoding { get; set; }
        public int? customer_id { get; set; }
    }
}