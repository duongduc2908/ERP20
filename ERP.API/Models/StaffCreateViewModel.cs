using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class StaffCreateViewModel
    {
        public StaffCreateViewModel()
        {
        }

        [StringLength(12)]
        public string sta_code { get; set; }

        [StringLength(50)]
        public string sta_thumbnai { get; set; }

        [StringLength(50)]
        public string sta_fullname { get; set; }

        [StringLength(50)]
        public string sta_username { get; set; }

        public string sta_password { get; set; }

        public byte? sta_sex { get; set; }

        public DateTime? sta_birthday { get; set; }
    }
}