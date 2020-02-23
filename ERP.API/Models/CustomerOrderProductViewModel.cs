using ERP.Data.ModelsERP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class CustomerOrderProductViewModel
    {
        public List<product> list { get; set; }
        public department depart_ment{get;set ;}
    }
}