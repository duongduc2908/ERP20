﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ERP.Common.Constants
{
    public class CheckNumber
    {
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^[\d\s()+-]{7,11}$").Success;
        }
        public static bool IsNumber(string pValue)
        {
            foreach (Char c in pValue)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }
    }
}