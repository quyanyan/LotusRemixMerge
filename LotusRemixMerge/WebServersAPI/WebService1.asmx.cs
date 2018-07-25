using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Services;

namespace WebServersAPI
{
    ///
    /// Service1 的摘要说明
    ///
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public bool CreateFile(string fileName)
        {
            bool isCreate = true;
            try
            {
                fileName = Path.Combine(Server.MapPath(""), Path.GetFileName(fileName));
                //首先设置上传服务器文件的路径  然后发布web服务 发布的时候要自己建一个自己知道的文件夹 "C:\NMGIS_Video" "C:\NMGIS_Video"                fileName = Path.Combine(Server.MapPath("") + @"\Video" + Path.GetFileName(fileName));  
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Close();
            }
            catch
            {
                isCreate = false;
            }
            return isCreate;
        }
        [WebMethod]
        public bool Append(string fileName, byte[] buffer)
        {
            bool isAppend = true;
            try
            {
                //fileName = Path.Combine(@"C:\NMGIS_Video" + Path.GetFileName(fileName));  
                fileName = Path.Combine(Server.MapPath(""), Path.GetFileName(fileName));
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Seek(0, SeekOrigin.End);
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
            }
            catch
            {
                isAppend = false;
            }
            return isAppend;
        }
        [WebMethod]
        public bool Verify(string fileName, string md5)
        {
            bool isVerify = true;
            try
            {
                fileName = Path.Combine(Server.MapPath(""), Path.GetFileName(fileName));

                // fileName = Server.MapPath（"D:\MesWebCR\picture")  + Path.GetFileName(fileName);
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                MD5CryptoServiceProvider p = new MD5CryptoServiceProvider();
                byte[] md5buffer = p.ComputeHash(fs);
                fs.Close();
                string md5Str = "";
                //List strList = new List();
                for (int i = 0; i < md5buffer.Length; i++)
                {
                    md5Str += md5buffer[i].ToString("x2");
                }
                if (md5 != md5Str)
                    isVerify = false;
            }
            catch
            {
                isVerify = false;
            }
            return isVerify;
        }
    }
}