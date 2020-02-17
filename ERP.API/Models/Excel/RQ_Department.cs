using ERP.Common.Excel.Model;
using ERP.Data.ModelsERP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.API.Models.Excel
{
    public class RQ_Department: WARQBase
    {
        public string Rt_Cols_Mst_Department { get; set; }
        public department mst_department { get; set; }
    }
}