using SuperHero;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Security.Cryptography;
using System.Web.Script.Services;

namespace WebServersAPI
{
    /// <summary>
    /// Summary description for AnalyzeResume
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AnalyzeResume : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void ExtractResume(string filePath)
        {
            var outputFolder = Path.Combine(Server.MapPath("") + @"\Output\");

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            Processor.Processor processor = new Processor.Processor(new JsonOutputFormatter());

            var files = Directory.GetFiles(filePath).Select(Path.GetFullPath);
            List<string> output = new List<string>();
            Context.Response.Write("[");
            foreach (var file in files)
            {
                output = processor.Process(file);
                Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                Context.Response.Charset = "gb2312";
                for (int i = 0; i < output.Count(); i++)
                {
                    string str = "";
                    if (i != (output.Count() - 1))
                    {
                        str += output[i] + ",";
                    }
                    else
                    {
                        str += output[i];
                    }
                    Context.Response.Write(str);
                }
            }
            Context.Response.Write("]");
        }

        [WebMethod]
        public bool CreateFile(string fileName)
        {
            bool isCreate = true;
            try
            {
                var path = Path.Combine(Server.MapPath("") + @"\UploadFile\");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //首先设置上传服务器文件的路径  然后发布web服务 发布的时候要自己建一个自己知道的文件夹 "C:\NMGIS_Video\" "C:\NMGIS_Video\"         
                fileName = Path.Combine(Server.MapPath("") + @"\UploadFile\" + Path.GetFileName(fileName));
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
        public string Append(string fileName, byte[] buffer)
        {
            string filePath = "";
            try
            {
                filePath = Path.Combine(Server.MapPath("") + @"\UploadFile\");
                fileName = Path.Combine(Server.MapPath("") + @"\UploadFile\" + Path.GetFileName(fileName));
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Seek(0, SeekOrigin.End);
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
            }
            catch
            {

            }
            return filePath;
        }

        [WebMethod]
        public bool Verify(string fileName, string md5)
        {
            bool isVerify = true;
            try
            {

                fileName = Path.Combine(Server.MapPath("") + @"\UploadFile\" + Path.GetFileName(fileName));
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                MD5CryptoServiceProvider p = new MD5CryptoServiceProvider();
                byte[] md5buffer = p.ComputeHash(fs);
                fs.Close();
                string md5Str = "";
                List<string> strList = new List<string>();
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
