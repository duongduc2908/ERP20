using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class GroupRoleCreateViewModel
    {
        public GroupRoleCreateViewModel() { }
        

        [StringLength(20)]
        public string gr_name { get; set; }

        [StringLength(45)]
        public string gr_thumbnail { get; set; }

        [Column(TypeName = "text")]
        public string gr_description { get; set; }

        public byte? gr_status { get; set; }
    }
}