using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Common.Constants
{
    public class GenDateOrderService
    {
        public static List<DateTime> Gen(DateTime datetimenow,bool IsDate=false, bool IsWeek= false, bool IsMonth= false)
        {
            List<DateTime> res = new List<DateTime>();
            int i=1;
            if(IsDate!=false)
            {
                do
                {
                    res.Add(datetimenow.AddDays(i));
                    i++;
                } while (i <= 365);
                
            }
            if (IsWeek != false)
            {
                do
                {
                    res.Add(datetimenow.AddDays(i));
                    i++;
                } while (i <= 7);

            }
            if (IsMonth != false)
            {
                do
                {
                    res.Add(datetimenow.AddDays(i));
                    i++;
                } while (i <= 30);

            }
            return res;
        }
    }
}