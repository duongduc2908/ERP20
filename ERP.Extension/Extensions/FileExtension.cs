using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;

namespace ERP.Extension.Extensions
{
    public static class FileExtension
    {
        #region["Mau save file"]
        public static string SaveFileOnDisk(MultipartFileData fileData)
        {
            string fileName = "";
            if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
            {
                fileName = Guid.NewGuid().ToString();
            }
            fileName = fileData.Headers.ContentDisposition.FileName;
            if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
            {
                fileName = fileName.Trim('"');
            }
            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
            {
                fileName = Path.GetFileName(fileName);
            }

            var newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Images", fileName);
            var fileInfo = new FileInfo(newFileName);
            if (fileInfo.Exists)
            {
                fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                fileName = fileName + (new Random().Next(0, 10000)) + fileInfo.Extension;

                newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Images", fileName);
            }

            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }


            File.Move(fileData.LocalFileName, newFileName);

            return "/Uploads/" + fileName;
        }

        #endregion
        #region [File Dinh kem ]
        public static string SaveFileOnDiskStaff(MultipartFileData fileData)
        {
            string fileName = "";
            if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
            {
                fileName = Guid.NewGuid().ToString();
            }
            fileName = fileData.Headers.ContentDisposition.FileName;
            if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
            {
                fileName = fileName.Trim('"');
            }
            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
            {
                fileName = Path.GetFileName(fileName);
            }

            var newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Files/Staffs", fileName);
            var fileInfo = new FileInfo(newFileName);
            if (fileInfo.Exists)
            {
                fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                fileName = fileName + (new Random().Next(0, 10000)) + fileInfo.Extension;

                newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Files/Staffs", fileName);
            }

            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }


            File.Move(fileData.LocalFileName, newFileName);

            return "/Uploads/Files/Staffs/" + fileName;
        }
        #endregion
        public static string SaveFileStaffOnDisk(MultipartFileData fileData, string staff_code)
        {
            string[] chuoi;
            var fileNametest = fileData.Headers.ContentDisposition.FileName;
            chuoi = fileNametest.Split('.');
            string fileName = staff_code+"."+ chuoi[chuoi.Length-1];
            if (fileName.StartsWith("\"") || fileName.EndsWith("\""))
            {
                fileName = fileName.Trim('"');
            }
            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
            {
                fileName = Path.GetFileName(fileName);
            }
            var newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Images/Staffs", fileName);
            var fileInfo = new FileInfo(newFileName);
            if (fileInfo.Exists)
            {
                File.Delete(newFileName);
                newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Images/Staffs", fileName);
            }
            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }
            File.Move(fileData.LocalFileName, newFileName);
            return "/Uploads/Images/Staffs/" + fileName;
        }

        public static string SaveFileProductOnDisk(MultipartFileData fileData, string pu_code)
        {
            string[] chuoi;
            var fileNametest = fileData.Headers.ContentDisposition.FileName;
            chuoi = fileNametest.Split('.');
            string fileName = pu_code + "." + chuoi[chuoi.Length - 1];
            if (fileName.StartsWith("\"") || fileName.EndsWith("\""))
            {
                fileName = fileName.Trim('"');
            }
            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
            {
                fileName = Path.GetFileName(fileName);
            }
            var newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Images/Products", fileName);
            var fileInfo = new FileInfo(newFileName);
            if (fileInfo.Exists)
            {
                File.Delete(newFileName);
                newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Images/Products", fileName);
            }
            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }
            File.Move(fileData.LocalFileName, newFileName);
            return "/Uploads/Images/Products/" + fileName;
        }
        public static string SaveFileServiceOnDisk(MultipartFileData fileData, string sr_code)
        {
            string[] chuoi;
            var fileNametest = fileData.Headers.ContentDisposition.FileName;
            chuoi = fileNametest.Split('.');
            string fileName = sr_code + "." + chuoi[chuoi.Length - 1];
            if (fileName.StartsWith("\"") || fileName.EndsWith("\""))
            {
                fileName = fileName.Trim('"');
            }
            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
            {
                fileName = Path.GetFileName(fileName);
            }
            var newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Images/Services", fileName);
            var fileInfo = new FileInfo(newFileName);
            if (fileInfo.Exists)
            {
                File.Delete(newFileName);
                newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Images/Services", fileName);
            }
            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }
            File.Move(fileData.LocalFileName, newFileName);
            return "/Uploads/Images/Services/" + fileName;
        }
        public static string SaveFileCustomerGroupOnDisk(MultipartFileData fileData, string pu_code)
        {
            string[] chuoi;
            var fileNametest = fileData.Headers.ContentDisposition.FileName;
            chuoi = fileNametest.Split('.');
            string fileName = pu_code + "." + chuoi[chuoi.Length - 1];
            if (fileName.StartsWith("\"") || fileName.EndsWith("\""))
            {
                fileName = fileName.Trim('"');
            }
            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
            {
                fileName = Path.GetFileName(fileName);
            }
            var newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Images/Customer-Group", fileName);
            var fileInfo = new FileInfo(newFileName);
            if (fileInfo.Exists)
            {
                File.Delete(newFileName);
                newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Images/Customer-Group", fileName);
            }
            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }
            File.Move(fileData.LocalFileName, newFileName);
            return "/Uploads/Images/Customer-Group/" + fileName;
        }
        public static string SaveFileCustomerOnDisk(MultipartFileData fileData, string cu_code)
        {
            string[] chuoi;
            var fileNametest = fileData.Headers.ContentDisposition.FileName;
            chuoi = fileNametest.Split('.');
            string fileName = cu_code + "." + chuoi[chuoi.Length - 1];
            if (fileName.StartsWith("\"") || fileName.EndsWith("\""))
            {
                fileName = fileName.Trim('"');
            }
            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
            {
                fileName = Path.GetFileName(fileName);
            }
            var newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Images/Customers", fileName);
            var fileInfo = new FileInfo(newFileName);
            if (fileInfo.Exists)
            {
                File.Delete(newFileName);
                newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Images/Customers", fileName);
            }
            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }
            File.Move(fileData.LocalFileName, newFileName);
            return "/Uploads/Images/Customers/" + fileName;
        }
        public static string SaveFileStaffOnDiskExcel(MultipartFileData fileData,string staff_code, string forder, string time_stamp)
        {
            string[] chuoi;
            var fileNametest = fileData.Headers.ContentDisposition.FileName;
            chuoi = fileNametest.Split('.');
            string fileName = staff_code+ time_stamp + "." + chuoi[chuoi.Length - 1];
            if (fileName.StartsWith("\"") || fileName.EndsWith("\""))
            {
                fileName = fileName.Trim('"');
            }
            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
            {
                fileName = Path.GetFileName(fileName);
            }
            var newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Excels/Staff/"+ forder + "/", fileName);
            var fileInfo = new FileInfo(newFileName);
            if (fileInfo.Exists)
            {
                fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                fileName = fileName + (new Random().Next(0, 10000)) + fileInfo.Extension;

                newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Excels/Staff/" + forder + "/", fileName);
            }

            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }


            File.Move(fileData.LocalFileName, newFileName);

            return "/Uploads/Excels/Staff/" + forder + "/" + fileName;
        }
        public static string SaveFileCustomerOnDiskExcel(MultipartFileData fileData, string staff_code, string forder, string time_stamp)
        {
            string[] chuoi;
            var fileNametest = fileData.Headers.ContentDisposition.FileName;
            chuoi = fileNametest.Split('.');
            string fileName = staff_code + time_stamp + "." + chuoi[chuoi.Length - 1];
            if (fileName.StartsWith("\"") || fileName.EndsWith("\""))
            {
                fileName = fileName.Trim('"');
            }
            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
            {
                fileName = Path.GetFileName(fileName);
            }
            var newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Excels/Customer/" + forder + "/", fileName);
            var fileInfo = new FileInfo(newFileName);
            if (fileInfo.Exists)
            {
                fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                fileName = fileName + (new Random().Next(0, 10000)) + fileInfo.Extension;

                newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Excels/Customer/" + forder + "/", fileName);
            }

            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }


            File.Move(fileData.LocalFileName, newFileName);

            return "/Uploads/Excels/Customer/" + forder + "/" + fileName;
        }
        public static string SaveFileProductOnDiskExcel(MultipartFileData fileData, string staff_code, string forder, string time_stamp)
        {
            string[] chuoi;
            var fileNametest = fileData.Headers.ContentDisposition.FileName;
            chuoi = fileNametest.Split('.');
            string fileName = staff_code + time_stamp + "." + chuoi[chuoi.Length - 1];
            if (fileName.StartsWith("\"") || fileName.EndsWith("\""))
            {
                fileName = fileName.Trim('"');
            }
            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
            {
                fileName = Path.GetFileName(fileName);
            }
            var newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Excels/Product/" + forder + "/", fileName);
            var fileInfo = new FileInfo(newFileName);
            if (fileInfo.Exists)
            {
                fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                fileName = fileName + (new Random().Next(0, 10000)) + fileInfo.Extension;

                newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Excels/Product/" + forder + "/", fileName);
            }

            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }


            File.Move(fileData.LocalFileName, newFileName);

            return "/Uploads/Excels/Product/" + forder + "/" + fileName;
        }
        public static string SaveFileCustomerOrderOnDiskExcel(MultipartFileData fileData, string staff_code, string forder, string time_stamp)
        {
            string[] chuoi;
            var fileNametest = fileData.Headers.ContentDisposition.FileName;
            chuoi = fileNametest.Split('.');
            string fileName = staff_code + time_stamp + "." + chuoi[chuoi.Length - 1];
            if (fileName.StartsWith("\"") || fileName.EndsWith("\""))
            {
                fileName = fileName.Trim('"');
            }
            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
            {
                fileName = Path.GetFileName(fileName);
            }
            var newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Excels/CustomerOrder/" + forder + "/", fileName);
            var fileInfo = new FileInfo(newFileName);
            if (fileInfo.Exists)
            {
                fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                fileName = fileName + (new Random().Next(0, 10000)) + fileInfo.Extension;

                newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Excels/CustomerOrder/" + forder + "/", fileName);
            }

            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }


            File.Move(fileData.LocalFileName, newFileName);

            return "/Uploads/Excels/CustomerOrder/" + forder + "/" + fileName;
        }
    }
}