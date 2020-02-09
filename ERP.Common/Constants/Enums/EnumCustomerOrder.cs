using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Common.Constants.Enums
{
    public class EnumCustomerOrder
    {
        public static string status_0 = "Đã báo kho";
        public static string status_1 = "Đang vận chuyển";
        public static string status_2 = "Đã thanh toán";

        public static string cuo_payment_type_1 = "Trả tiền mặt";
        public static string cuo_payment_type_2 = "Chuyển khoản";
        public static string cuo_payment_type_3 = "Thẻ visa";

        public static string cuo_payment_status_1 = "Đã thanh toán ";
        public static string cuo_payment_status_2 = "Chưa thanh toán";


    }
}