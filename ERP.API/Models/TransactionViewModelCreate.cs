﻿using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class TransactionViewModelCreate
    {
        public customer customer { get; set; }
        [StringLength(50)]
        public string tra_title { get; set; }

        public string tra_content { get; set; }

        public string tra_rate { get; set; }

        public byte? tra_type { get; set; }

        public DateTime? tra_datetime { get; set; }

        [StringLength(50)]
        public string tra_result { get; set; }

        public byte? tra_priority { get; set; }

        public int? staff_id { get; set; }

        public int? customer_id { get; set; }

        public byte? tra_status { get; set; }
    }
}