using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Common.Constants.Enums
{
    public class EnumCustomer
    {
        public static string[] cu_type = new string[2] { "Khách sỉ" , "Khách lẻ"};
        public static string[] cu_status = new string[2] {  "Khóa","Kích hoạt"};
        public static string[] cu_flag_order = new string[3] {  "Cần đặt","Đã đặt","Chưa đặt"};
        public static string[] cu_flag_used = new string[2] {  "Cần sử dụng","Chưa sử dụng"};
    }
}