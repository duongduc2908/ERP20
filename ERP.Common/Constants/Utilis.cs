using System;
using System.Collections.Generic;
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
    }
}