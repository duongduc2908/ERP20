using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class StaffLoginViewModel
    {
        [StringLength(50)]
        public string sta_username { get; set; }

        public string sta_password { get; set; }
    }
}