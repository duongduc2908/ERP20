using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ERP.API.Controllers.Dashboard
{
    public class BaseController : ApiController
    {
        public string GwUserCode = "";
        public string GwPassword = "";
        public string FuncType = "";
        public string Ft_RecordStart = "0";
        public string Ft_RecordStartExportExcel = "0";
        public string Ft_RecordCount = "123456000";
        public string Ft_WhereClause = "";
        public static string current_id ;
        public string Hethong = "";
        public string FolderUploadTest = "";
        public static string SubPath = "";
        public string TenantIdName = "idocNet";
        public int PageSizeConfig = 10;
        public int RowsWorksheets = 1048570; // If you are working on Excel 2007 or any of the latest versions - there are 1048576 rows and 16384 columns. 
        public string Uploads = "Uploads";
        public string TempFiles = "TempFiles";
        //public double FileUploadSize = 10485760; // 10MB
        //public double FileImageSize = 6164480; // 5MB
        public double FileUploadSize = 26214400; // 25MB
        public double FileImageSize = 26214400; // 25MB
        public string FlagActive = "1";
        public string FlagInActive = "0";
        private int inext = 0;
        public BaseController() {
            var lst_claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            if(lst_claims.Count() == 4)
            {
                current_id = lst_claims[3].Value;
            }
        }
        public string GetNextTId()
        {
            //string fileId = Guid.NewGuid().ToString("D");
            var strDateTimeNow = DateTime.Now.ToString("yyyyMMdd.HHmmss.ffffff").Trim();
            var strNextTId = string.Format("{0}.{1}", strDateTimeNow, inext++);
            //var strGetNextTId = strNextTId + fileId;
            return strNextTId;
        }

        public string GenExcelExportFilePath(string prefix, ref string virualPath)
        {
            String subpath = string.Format("/TempFiles/{0}", DateTime.Now.ToString("yyyy-MM-dd"));

            string filename = string.Format("{0}_{1}", prefix, DateTime.Now.ToString("yyMMddHHmmss"));

            
            string subpathPhys = System.Web.HttpContext.Current.Server.MapPath(subpath); 

            if (!Directory.Exists(subpathPhys))
            {
                Directory.CreateDirectory(subpathPhys);
            }

            virualPath = string.Format("{0}/{1}.xlsx", subpath, filename);
            string filePath = System.Web.HttpContext.Current.Server.MapPath(virualPath);

            int i = 0;
            while (System.IO.File.Exists(filePath))
            {

                virualPath = string.Format("{0}/{1}_{2}.xlsx", subpath, filename, ++i);
                filePath = System.Web.HttpContext.Current.Server.MapPath(virualPath);
            }

            return filePath;
        }

    }
}