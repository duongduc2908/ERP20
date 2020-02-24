using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class AddressCreateViewModel
    {
        public AddressCreateViewModel()
        {

        }
        

        [StringLength(50)]
        public string add_ward { get; set; }

        [StringLength(50)]
        public string add_district { get; set; }

        [StringLength(50)]
        public string add_province { get; set; }

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