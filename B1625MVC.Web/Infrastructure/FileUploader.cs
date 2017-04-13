using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace B1625MVC.Web.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class FileUploader
    {
        public static byte[] UploadFile(HttpPostedFileBase file)
        {
            byte[] content = null;
            byte[] buffer = new byte[file.ContentLength];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = file.InputStream.Read(buffer, 0, file.ContentLength)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                content = ms.ToArray();
            }
            return content;
        }
    }
}