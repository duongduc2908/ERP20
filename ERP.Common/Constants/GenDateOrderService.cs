using ERP.Common.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Common.Constants
{
    public class GenDateOrderService
    {
        public static List<DateTime> Gen(DateTime datetime_start,DateTime datetime_end, byte st_repeat_type, byte st_on_the, bool st_sun_flag = false, bool st_mon_flag = false, bool st_tue_flag = false, bool st_wed_flag = false, bool st_thu_flag = false, bool st_fri_flag = false, bool st_sat_flag = false, bool st_repeat_every = false, bool st_on_day_flag=false,int st_on_day=0,int customer_order_id=0)
        {
            List<DateTime> res = new List<DateTime>();
            if(EnumRepeatType.st_repeat_type[st_repeat_type].Contains("day"))
            {

            }
            return res;
        }
    }
}