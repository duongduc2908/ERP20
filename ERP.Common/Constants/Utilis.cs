using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ERP.Common.Constants
{
    public class Utilis
    {
        public static string GetFileFormat(string filename)
        {
            string res = "";
            string[] arrListStr = filename.Split('.');
            int a = arrListStr.Length;
            res = arrListStr[a-1];
            res = res.Replace("\"","");
            return res;
        }
        public static string CreateCode(string code, int id, int lenght)
        {
            string res = "";
            int temp_id = id;
            int id_lenght = 0;
            while (id >= 10)
            {
                id /= 10; // hay n = n /10;
                id_lenght++;
            }
            int code_lenght = code.Length;
            int add_lenght = lenght - code_lenght - id_lenght;
            if(add_lenght > 0)
            {
                string add = "";
                for(int i = 1; i< add_lenght; i++)
                {
                    add = String.Concat(add, '0');
                }
                res = String.Concat(code, add, temp_id);
            }

            return res;
        }
        public static string MakeRandomPassword(int length)
        {
            string UpperCase = "QWERTYUIOPASDFGHJKLZXCVBNM";
            string LowerCase = "qwertyuiopasdfghjklzxcvbnm";
            string Digits = "1234567890";
            string ktdb = "~!@#$%^&*()_+";
            string allCharacters = UpperCase + LowerCase + Digits + ktdb;
            //Random will give random charactors for given length  
            Random r = new Random();
            String password = "";
            for (int i = 0; i < length; i++)
            {
                double rand = r.NextDouble();
                if (i == 0)
                {
                    password += UpperCase.ToCharArray()[(int)Math.Floor(rand * UpperCase.Length)];
                }
                else
                {
                    password += allCharacters.ToCharArray()[(int)Math.Floor(rand * allCharacters.Length)];
                }
            }
            return password;
        }
        /// <summary>
        /// Lấy ra ngày đầu tiên trong tháng có chứa 
        /// 1 ngày bất kỳ được truyền vào
        /// </summary>
        /// <param name="dtDate">Ngày nhập vào</param>
        /// <returns>Ngày đầu tiên trong tháng</returns>
        public static DateTime GetFirstDayOfMonth(DateTime dtInput)
        {
            DateTime dtResult = dtInput;
            dtResult = dtResult.AddDays((-dtResult.Day) + 1);
            return dtResult;
        }
        /// <summary>
        /// Lấy ra ngày đầu tiên trong tháng được truyền vào 
        /// là 1 số nguyên từ 1 đến 12
        /// </summary>
        /// <param name="iMonth">Thứ tự của tháng trong năm</param>
        /// <returns>Ngày đầu tiên trong tháng</returns>
        public static DateTime GetFirstDayOfMonth(int iMonth)
        {
            DateTime dtResult = new DateTime(DateTime.Now.Year, iMonth, 1);
            dtResult = dtResult.AddDays((-dtResult.Day) + 1);
            return dtResult;
        }
        ///<summary>
        /// Lấy ra ngày cuối cùng trong tháng có chứa 
        /// 1 ngày bất kỳ được truyền vào
        /// </summary>
        /// <param name="dtInput">Ngày nhập vào</param>
        /// <returns>Ngày cuối cùng trong tháng</returns>
        public static DateTime GetLastDayOfMonth(DateTime dtInput)
        {
            DateTime dtResult = dtInput;
            dtResult = dtResult.AddMonths(1);
            dtResult = dtResult.AddDays(-(dtResult.Day));
            return dtResult;
        }
        /// <summary>
        /// Lấy ra ngày cuối cùng trong tháng được truyền vào
        /// là 1 số nguyên từ 1 đến 12
        /// </summary>
        /// <param name="iMonth"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(int iMonth)
        {
            DateTime dtResult = new DateTime(DateTime.Now.Year, iMonth, 1);
            dtResult = dtResult.AddMonths(1);
            dtResult = dtResult.AddDays(-(dtResult.Day));
            return dtResult;
        }
        /// <summary>
        /// Lấy ra ngày đầu tiên trong tuần của ngày nhập vào 
        /// với Culture mặc định là Culture hiện tại
        /// </summary>
        /// <param name="dayInWeek">Ngày nhập vào</param>
        /// <returns>Ngày đầu tiên trong tuần</returns>
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            return GetFirstDayOfWeek(dayInWeek, defaultCultureInfo);
        }
        /// <summary>
        /// Lấy ra ngày đầu tiên trong tuần của ngày nhập vào
        /// với một Culture cụ thể được truyền vào
        /// </summary>
        /// <param name="dayInWeek">Ngày nhập vào</param>
        /// <param name="cultureInfo">CultureInfo quy định các thông tin về Culture 
        /// ( định dạng ngày tháng, ngày bắt đầu trong tuần , ... )
        /// </param>
        /// <returns></returns>
        private static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
            {
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            }

            return firstDayInWeek;
        }
        /// <summary>
        /// Lấy ra ngày đầu tiên trong tuần của ngày nhập vào
        /// với 1 giá trị cụ thể của enum DayOfWeek chỉ định 
        /// ngày bắt đầu tuần là thứ mấy
        /// </summary>
        /// <param name="dayInWeek">Ngày nhập vào</param>
        /// <param name="dayOfWeek">enum chỉ định thứ bắt đầu tuần</param>
        /// <returns></returns>
        private static DateTime GetFirstDayOfWeek(DateTime dayInWeek, DayOfWeek dayOfWeek)
        {
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != dayOfWeek)
            {
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            }
            return firstDayInWeek;
        }
    }
}