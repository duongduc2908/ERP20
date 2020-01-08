using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ERP.Extension.Extensions
{
    public static class DateExtension
    {
        public static DateTime GetCurrentTime()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            DateTime dt =
                DateTime.ParseExact(currentDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            return dt;
        }

        public static DateTime ConvertStringToDateTime(string text)
        {
            DateTime dateTime = DateTime.ParseExact(text, "dd/MM/yyyy", null);
            return dateTime;
        }
    }
}