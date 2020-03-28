﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class OrderServiceCreateViewModel
    {
        public OrderServiceCreateViewModel() { }

        public byte? os_evaluation { get; set; }

        public int? service_id { get; set; }

        public int? customer_order_id { get; set; }

        public byte? os_show_as { get; set; }

        public int? os_reminder { get; set; }
    }
}