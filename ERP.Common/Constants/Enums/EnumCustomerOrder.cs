using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Common.Constants.Enums
{
    public class EnumCustomerOrder
    {
        public static string[] status = new string[] { "Đã báo kho", "Đang vận chuyển", "Đã thanh toán" };
        

        public static string[] cuo_payment_type = new string[] { "Trả tiền mặt", "Chuyển khoản", "Thẻ visa" };
       
        public static string[] cuo_payment_status = new string[] { "Đã thanh toán ", "Chưa thanh toán"};


    }
}