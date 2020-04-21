using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Common.Constants.Enums
{
    public class EnumDateWork
    {
        public static Dictionary<string, string> date_work = new Dictionary<string, string>()
            {
                 {"Monday","TH2"},
                 {"Tuesday" ,"TH3"},
                 {"Wednesday","TH4"},
                 {"Thursday","TH5"},
                 {"Friday","TH6"},
                 {"Saturday","TH7"},
                 {"Sunday","TH8"}
            };
        public static string get_str(string key)
        {
            string val="";
            foreach (var prop in date_work.Keys)
            {
                if(prop.Contains(key))
                    val = date_work[prop];
            }
            return val;
        }
}
}