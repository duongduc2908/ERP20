using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Common.Constants.Enums
{
    public class EnumTransaction
    {
        public static string[] tra_type = new string[4] { "Gặp mặt trực tiếp", " Gọi điện thoại ", "Gọi tổng đài","Tiếp nhận đơn" };
        public static string[] tra_priority = new string[4] { "Cao nhất","Cao","Bình thường","Thấp" };
        public static string[] tra_status = new string[3] { "Chưa thực hiện","Đang thực hiện","Đã hoàn thành"};
        public static string[] tra_rate = new string[4] { "Chưa tốt","Hài lòng","Tốt","Rất tốt"};
    }
}