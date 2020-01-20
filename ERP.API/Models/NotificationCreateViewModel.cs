using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class NotificationCreateViewModel
    {
        public NotificationCreateViewModel() { }
        

        [StringLength(50)]
        public string ntf_title { get; set; }

        public string ntf_description { get; set; }

        public DateTime? ntf_datetime { get; set; }

        public DateTime? ntf_confim_datetime { get; set; }

        public int? staff_id { get; set; }
    }
}