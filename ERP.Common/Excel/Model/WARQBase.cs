using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Common.Excel.Model
{
    public class WARQBase
    {
		public string Tid { get; set; }

		public string UtcOffset { get; set; }

		public string GwUserCode { get; set; }

		public string GwPassword { get; set; }

		public string WAUserCode { get; set; }

		public string WAUserPassword { get; set; }

		public string FlagIsDelete { get; set; }

		public string FuncType { get; set; }

		public string Ft_RecordStart { get; set; }

		public string Ft_RecordCount { get; set; }

		public string Ft_WhereClause { get; set; }

		public string Ft_Cols_Upd { get; set; }
	}
}