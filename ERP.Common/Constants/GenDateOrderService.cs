using ERP.Common.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Common.Constants
{
    public class GenDateOrderService
    {
        public static List<DateTime> Gen(DateTime datetime_start,DateTime datetime_end, byte st_repeat_type,int st_repeat_every, bool st_sun_flag = false, bool st_mon_flag = false, bool st_tue_flag = false, bool st_wed_flag = false, bool st_thu_flag = false, bool st_fri_flag = false, bool st_sat_flag = false, bool st_on_day_flag=false,int st_on_day=0, bool st_on_the_flag = false, int st_on_the = 0)
        {
            List<DateTime> res = new List<DateTime>();
            if(EnumRepeatType.st_repeat_type[st_repeat_type-1].Contains("Day"))
            {
                do
                {
                    res.Add(datetime_start);
                    datetime_start = datetime_start.AddDays(st_repeat_every);
                } while (datetime_start <= datetime_end);
            }
            if(EnumRepeatType.st_repeat_type[st_repeat_type-1].Contains("Week"))
            {
                int resultCompare = 0;
                DateTime start = datetime_start;
                #region["Add date week once"]
                if (st_mon_flag == true)
                {
                    resultCompare = Utilis.CompareDate((int)datetime_start.DayOfWeek, "M");

                    if (resultCompare >= 0)
                    {
                        start = datetime_start.AddDays(resultCompare);
                    }
                    else
                    {
                        start = datetime_start.AddDays(resultCompare + st_repeat_every * 7);
                    }
                    if (start < datetime_end)
                        res.Add(start);
                }
                if (st_tue_flag == true)
                {
                    resultCompare = Utilis.CompareDate((int)datetime_start.DayOfWeek, "T3");

                    if (resultCompare >= 0)
                    {
                        start = datetime_start.AddDays(resultCompare);
                    }
                    else
                    {
                        start = datetime_start.AddDays(resultCompare + st_repeat_every * 7);
                    }
                    if (start < datetime_end)
                        res.Add(start);
                }
                if (st_wed_flag == true)
                {
                    resultCompare = Utilis.CompareDate((int)datetime_start.DayOfWeek, "W");

                    if (resultCompare >= 0)
                    {
                        start = datetime_start.AddDays(resultCompare);
                    }
                    else
                    {
                        start = datetime_start.AddDays(resultCompare + st_repeat_every * 7);
                    }
                    if (start < datetime_end)
                        res.Add(start);
                }
                if (st_thu_flag == true)
                {
                    resultCompare = Utilis.CompareDate((int)datetime_start.DayOfWeek, "T5");

                    if (resultCompare >= 0)
                    {
                        start = datetime_start.AddDays(resultCompare);
                    }
                    else
                    {
                        start = datetime_start.AddDays(resultCompare + st_repeat_every * 7);
                    }
                    if (start < datetime_end)
                        res.Add(start);
                }
                if (st_fri_flag == true)
                {
                    resultCompare = Utilis.CompareDate((int)datetime_start.DayOfWeek, "F");

                    if (resultCompare >= 0)
                    {
                        start = datetime_start.AddDays(resultCompare);
                    }
                    else
                    {
                        start = datetime_start.AddDays(resultCompare + st_repeat_every * 7);
                    }
                    if (start < datetime_end)
                        res.Add(start);
                }
                if (st_sat_flag == true)
                {
                    resultCompare = Utilis.CompareDate((int)datetime_start.DayOfWeek, "S7");

                    if (resultCompare >= 0)
                    {
                        start = datetime_start.AddDays(resultCompare);
                    }
                    else
                    {
                        start = datetime_start.AddDays(resultCompare + st_repeat_every * 7);
                    }
                    if (start < datetime_end)
                        res.Add(start);
                }
                if (st_sun_flag == true)
                {
                    resultCompare = Utilis.CompareDate((int)datetime_start.DayOfWeek, "S8");

                    if (resultCompare >= 0)
                    {
                        start = datetime_start.AddDays(resultCompare);
                    }
                    else
                    {
                        start = datetime_start.AddDays(resultCompare + st_repeat_every * 7);
                    }
                    if (start < datetime_end)
                        res.Add(start);
                }
                #endregion
                List<DateTime> res_for = new List<DateTime>(res);
                foreach (DateTime dt in res_for)
                {
                    DateTime dt1 = dt;
                    do
                    {
                         dt1= dt1.AddDays(st_repeat_every * 7);
                        if (dt1 <= datetime_end)
                            res.Add(dt1);
                    } while (dt1 <= datetime_end);
                }
            }
            if(EnumRepeatType.st_repeat_type[st_repeat_type-1].Contains("Month"))
            {
                if(st_on_day_flag)
                {
                    DateTime dtRepeat = new DateTime(datetime_start.Year, datetime_start.Month, st_on_day);
                    if (dtRepeat < datetime_end)
                        res.Add(dtRepeat);
                    do
                    {
                        dtRepeat = dtRepeat.AddMonths(st_repeat_every);
                        res.Add(dtRepeat);
                    } while (dtRepeat < datetime_end);
                }
                if (st_on_the_flag)
                {
                    DateTime dtTest = datetime_start;
                    int resultCompare;
                    int weekNumber = Utilis.GetWeekNumberOfMonth(dtTest);
                    if(st_on_the>= weekNumber)
                    {
                        dtTest = dtTest.AddDays((st_on_the-weekNumber)*7);
                        DateTime start = dtTest;
                        #region["Add date week once"]
                        if (st_mon_flag == true)
                        {
                            resultCompare = Utilis.CompareDate((int)start.DayOfWeek, "M");

                            if (resultCompare >= 0)
                            {
                                start = start.AddDays(resultCompare);
                            }
                            else
                            {
                                start = start.AddDays(resultCompare + st_repeat_every * 7);
                            }
                            if (start < datetime_end)
                                res.Add(start);
                        }
                        if (st_tue_flag == true)
                        {
                            resultCompare = Utilis.CompareDate((int)start.DayOfWeek, "T3");

                            if (resultCompare >= 0)
                            {
                                start = start.AddDays(resultCompare);
                            }
                            else
                            {
                                start = start.AddDays(resultCompare + st_repeat_every * 7);
                            }
                            if (start < datetime_end)
                                res.Add(start);
                        }
                        if (st_wed_flag == true)
                        {
                            resultCompare = Utilis.CompareDate((int)start.DayOfWeek, "W");

                            if (resultCompare >= 0)
                            {
                                start = start.AddDays(resultCompare);
                            }
                            else
                            {
                                start = start.AddDays(resultCompare + st_repeat_every * 7);
                            }
                            if (start < datetime_end)
                                res.Add(start);
                        }
                        if (st_thu_flag == true)
                        {
                            resultCompare = Utilis.CompareDate((int)start.DayOfWeek, "T5");

                            if (resultCompare >= 0)
                            {
                                start = start.AddDays(resultCompare);
                            }
                            else
                            {
                                start = start.AddDays(resultCompare + st_repeat_every * 7);
                            }
                            if (start < datetime_end)
                                res.Add(start);
                        }
                        if (st_fri_flag == true)
                        {
                            resultCompare = Utilis.CompareDate((int)start.DayOfWeek, "F");

                            if (resultCompare >= 0)
                            {
                                start = start.AddDays(resultCompare);
                            }
                            else
                            {
                                start = start.AddDays(resultCompare + st_repeat_every * 7);
                            }
                            if (start < datetime_end)
                                res.Add(start);
                        }
                        if (st_sat_flag == true)
                        {
                            resultCompare = Utilis.CompareDate((int)start.DayOfWeek, "S7");

                            if (resultCompare >= 0)
                            {
                                start = start.AddDays(resultCompare);
                            }
                            else
                            {
                                start = start.AddDays(resultCompare + st_repeat_every * 7);
                            }
                            if (start < datetime_end)
                                res.Add(start);
                        }
                        if (st_sun_flag == true)
                        {
                            resultCompare = Utilis.CompareDate((int)start.DayOfWeek, "S8");

                            if (resultCompare >= 0)
                            {
                                start = start.AddDays(resultCompare);
                            }
                            else
                            {
                                start = start.AddDays(resultCompare + st_repeat_every * 7);
                            }
                            if (start < datetime_end)
                                res.Add(start);
                        }
                        #endregion

                        List<DateTime> res_for = new List<DateTime>(res);
                        foreach (DateTime dt in res_for)
                        {
                            DateTime dtRepeat = dt;
                            do
                            {
                                dtRepeat = dtRepeat.AddDays(st_on_the * 7);
                                int current_week = Utilis.GetWeekNumberOfMonth(dtRepeat);
                                dtRepeat = dtRepeat.AddDays((st_on_the - current_week) * 7);
                                if(dtRepeat<datetime_end)
                                    res.Add(dtRepeat);
                            } while (dtRepeat < datetime_end);
                        }

                    }
                    else
                    {
                        dtTest = dtTest.AddDays(((st_on_the - weekNumber)+5)*7);
                        DateTime start = dtTest;
                        #region["Add date week once"]
                        if (st_mon_flag == true)
                        {
                            resultCompare = Utilis.CompareDate((int)dtTest.DayOfWeek, "M");

                            if (resultCompare >= 0)
                            {
                                start = start.AddDays(resultCompare);
                            }
                            else
                            {
                                start = start.AddDays(resultCompare + st_repeat_every * 7);
                            }
                            if (start < datetime_end)
                                res.Add(start);
                        }
                        if (st_tue_flag == true)
                        {
                            resultCompare = Utilis.CompareDate((int)start.DayOfWeek, "T3");

                            if (resultCompare >= 0)
                            {
                                start = start.AddDays(resultCompare);
                            }
                            else
                            {
                                start = start.AddDays(resultCompare + st_repeat_every * 7);
                            }
                            if (start < datetime_end)
                                res.Add(start);
                        }
                        if (st_wed_flag == true)
                        {
                            resultCompare = Utilis.CompareDate((int)start.DayOfWeek, "W");

                            if (resultCompare >= 0)
                            {
                                start = start.AddDays(resultCompare);
                            }
                            else
                            {
                                start = start.AddDays(resultCompare + st_repeat_every * 7);
                            }
                            if (start < datetime_end)
                                res.Add(start);
                        }
                        if (st_thu_flag == true)
                        {
                            resultCompare = Utilis.CompareDate((int)start.DayOfWeek, "T5");

                            if (resultCompare >= 0)
                            {
                                start = start.AddDays(resultCompare);
                            }
                            else
                            {
                                start = start.AddDays(resultCompare + st_repeat_every * 7);
                            }
                            if (start < datetime_end)
                                res.Add(start);
                        }
                        if (st_fri_flag == true)
                        {
                            resultCompare = Utilis.CompareDate((int)start.DayOfWeek, "F");

                            if (resultCompare >= 0)
                            {
                                start = start.AddDays(resultCompare);
                            }
                            else
                            {
                                start = start.AddDays(resultCompare + st_repeat_every * 7);
                            }
                            if (start < datetime_end)
                                res.Add(start);
                        }
                        if (st_sat_flag == true)
                        {
                            resultCompare = Utilis.CompareDate((int)start.DayOfWeek, "S7");

                            if (resultCompare >= 0)
                            {
                                start = start.AddDays(resultCompare);
                            }
                            else
                            {
                                start = start.AddDays(resultCompare + st_repeat_every * 7);
                            }
                            if (start < datetime_end)
                                res.Add(start);
                        }
                        if (st_sun_flag == true)
                        {
                            resultCompare = Utilis.CompareDate((int)start.DayOfWeek, "S8");

                            if (resultCompare >= 0)
                            {
                                start = start.AddDays(resultCompare);
                            }
                            else
                            {
                                start = start.AddDays(resultCompare + st_repeat_every * 7);
                            }
                            if (start < datetime_end)
                                res.Add(start);
                        }
                        #endregion

                        List<DateTime> res_for = new List<DateTime>(res);
                        foreach (DateTime dt in res_for)
                        {
                            DateTime dtRepeat = dt;
                            do
                            {
                                dtRepeat = dtRepeat.AddDays(st_on_the * 7);
                                int current_week = Utilis.GetWeekNumberOfMonth(dtRepeat);
                                dtRepeat = dtRepeat.AddDays((st_on_the - current_week) * 7);
                                if (dtRepeat < datetime_end)
                                    res.Add(dtRepeat);
                            } while (dtRepeat < datetime_end);
                        }
                    }
                }
            }
            return res;
        }
    }
}