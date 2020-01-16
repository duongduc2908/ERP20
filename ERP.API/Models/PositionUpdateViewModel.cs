using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class PositionUpdateViewModel
    {
        public PositionUpdateViewModel() { }
        [Key]
        public int pos_id { get; set; }

        [StringLength(250)]
        public string pos_name { get; set; }

        [StringLength(50)]
        public string pos_competence { get; set; }

        [StringLength(150)]
        public string pos_abilty { get; set; }

        [StringLength(50)]
        public string pos_authority { get; set; }

        [StringLength(250)]
        public string pos_responsibility { get; set; }

        [StringLength(250)]
        public string pos_description { get; set; }
    }
}