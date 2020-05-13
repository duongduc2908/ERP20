using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;

namespace ERP.API.Controllers.Dashboard
{
    public class BaseController : ApiController
    {
        #region["Parameters"]
        public string GwUserCode = "";
        public string GwPassword = "";
        public string FuncType = "";
        public string Ft_RecordStart = "0";
        public string Ft_RecordStartExportExcel = "0";
        public string Ft_RecordCount = "123456000";
        public string Ft_WhereClause = "";
        public static int current_id;
        public string Hethong = "";
        public string FolderUploadTest = "";
        public static string SubPath = "";
        public string TenantIdName = "idocNet";
        public int PageSizeConfig = 10;
        public int RowsWorksheets = 1048570; // If you are working on Excel 2007 or any of the latest versions - there are 1048576 rows and 16384 columns. 
        public string Uploads = "Uploads";
        public string TempFiles = "TempFiles";
        public double FileUploadSize = 26214400; // 25MB
        public double FileImageSize = 26214400; // 25MB
        public string FlagActive = "1";
        public string FlagInActive = "0";
        private int inext = 0;
        public static Client socket;
        public static String ip= "localhost";
        public static String user;
        public static int PORT_NUMBER = 9999;
        public const int BUFFER_SIZE = 1024;
        public static ASCIIEncoding encoding = new ASCIIEncoding();
        #endregion
        //var clientInfo = new Object();
        //    clientInfo.customId         = data.customId;
        //    clientInfo.clientId     = socket.id;
        //    clients.push(clientInfo);
        public BaseController() {
            
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
        public static void send_mail(string body, string to_MailAddress, string title)
        {
            MailMessage message = new MailMessage();
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
            //Add Image
            LinkedResource theEmailImageBG = new LinkedResource("C:/inetpub/wwwroot/coerp/Uploads/Images/default/header-bg-1.png", "image/png");
            theEmailImageBG.ContentId = "idBG";
            //Add the Image to the Alternate view
            htmlView.LinkedResources.Add(theEmailImageBG);

            LinkedResource theEmailImageFB = new LinkedResource("C:/inetpub/wwwroot/coerp/Uploads/Images/default/fb-6.png", "image/png");
            theEmailImageFB.ContentId = "idFB";
            //Add the Image to the Alternate view
            htmlView.LinkedResources.Add(theEmailImageFB);

            LinkedResource theEmailImageTW = new LinkedResource("C:/inetpub/wwwroot/coerp/Uploads/Images/default/tw-6.png", "image/png");
            theEmailImageTW.ContentId = "idTW";
            //Add the Image to the Alternate view
            htmlView.LinkedResources.Add(theEmailImageTW);

            LinkedResource theEmailImageGG = new LinkedResource("C:/inetpub/wwwroot/coerp/Uploads/Images/default/gg-6.png", "image/png");
            theEmailImageGG.ContentId = "idGG";
            //Add the Image to the Alternate view
            htmlView.LinkedResources.Add(theEmailImageGG);

            LinkedResource theEmailImageBH = new LinkedResource("C:/inetpub/wwwroot/coerp/Uploads/Images/default/bh-6.png", "image/png");
            theEmailImageBH.ContentId = "idBH";
            //Add the Image to the Alternate view
            htmlView.LinkedResources.Add(theEmailImageBH);

            LinkedResource theEmailImageIN = new LinkedResource("C:/inetpub/wwwroot/coerp/Uploads/Images/default/in-6.png", "image/png");
            theEmailImageIN.ContentId = "idIN";
            //Add the Image to the Alternate view
            htmlView.LinkedResources.Add(theEmailImageIN);

            LinkedResource theEmailImageDB = new LinkedResource("C:/inetpub/wwwroot/coerp/Uploads/Images/default/db-6.png", "image/png");
            theEmailImageDB.ContentId = "idDB";
            //Add the Image to the Alternate view
            htmlView.LinkedResources.Add(theEmailImageDB);

            //Add view to the Email Message
            message.AlternateViews.Add(htmlView);

            SmtpClient smtp = new SmtpClient();
            var email_send = ConfigurationManager.AppSettings["emailsend"];
            var emailsendpass = ConfigurationManager.AppSettings["emailsendpass"];
            message.From = new MailAddress(email_send, "Coerp quản trị doanh nghiệp tinh gọn", System.Text.Encoding.UTF8);
            message.To.Add(new MailAddress(to_MailAddress));
            message.Subject = title;
            message.IsBodyHtml = true; //to make message body as html  
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential(email_send, emailsendpass);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }
        public static int get_id_current()
        {
            var lst_claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            if (lst_claims.Count() == 4)
            {
                current_id = int.Parse( lst_claims[3].Value);
            }
            return current_id;
        }
        public static string get_timestamp()
        {
            return int.Parse(DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()).ToString();
        }
        public static string folder()
        {
            string date = DateTime.Now.Day.ToString();
            string month = Convert.ToInt32(DateTime.Today.Month).ToString();

            return month+date;
        }
        public static string get_infor_current(bool email = false, bool role = false , bool fullname = false)
        {
            if(email == true)
            {
                return ClaimsPrincipal.Current.Identities.First().Claims.ToList()[2].Value.ToString();
            }
            if (role == true)
            {
                return ClaimsPrincipal.Current.Identities.First().Claims.ToList()[0].Value.ToString();
            }
            else return ClaimsPrincipal.Current.Identities.First().Claims.ToList()[1].Value.ToString();

        }
    }
}