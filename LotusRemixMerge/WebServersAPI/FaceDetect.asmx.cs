using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebServersAPI
{
    /// <summary>
    /// Summary description for FaceDetect
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class FaceDetect : WebService
    {
        [WebMethod]
        //location 子目录名称
        public string CreateDir(string location)
        {
            string savePath = Server.MapPath("~/ImageFile/");
            //检查服务器上是否存在这个物理路径，如果不存在则创建 
            savePath = Path.Combine(savePath, location);
            if (!Directory.Exists(savePath))
            {
                //需要注意的是，需要对这个物理路径有足够的权限，否则会报错 
                //另外，这个路径应该是在网站之下，而将网站部署在C盘却把上传文件保存在D盘
                Directory.CreateDirectory(savePath);
            }
            return savePath;
        }
    }
}
