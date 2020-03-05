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
        public static string SaveFileOnDiskExcel(MultipartFileData fileData, int timestamp)
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

            var newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Excels/"+ timestamp+"/", fileName);
            var fileInfo = new FileInfo(newFileName);
            if (fileInfo.Exists)
            {
                fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                fileName = fileName + (new Random().Next(0, 10000)) + fileInfo.Extension;

                newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Excels/"+ timestamp + "/", fileName);
            }

            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }


            File.Move(fileData.LocalFileName, newFileName);

            return "/Uploads/Excels/" + timestamp + "/" + fileName;
        }
        public static string SaveListFilesOnDisk(MultipartFileData fileData, string productId)
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

            var newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Images/" + productId + "/", fileName);
            var fileInfo = new FileInfo(newFileName);
            if (fileInfo.Exists)
            {
                fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                fileName = fileName + (new Random().Next(0, 10000000)) + fileInfo.Extension;

                newFileName = Path.Combine(HostingEnvironment.MapPath("/") + @"/Uploads/Images/" + productId + "/", fileName);
            }

            if (!Directory.Exists(fileInfo.Directory.FullName))
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }


            File.Move(fileData.LocalFileName, newFileName);

            return @"/Uploads/Images/" + productId + "/" + fileName;
        }
    }
}