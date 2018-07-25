using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Drawing;
using Readers.ServiceReference1;
using Baidu.Aip.Face;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Readers
{
    public static class CommonHelper
    {
        private static string clientId = "19YqVLf7f00ZMbZj4jGXWl42";
        // 百度云中开通对应服务应用的 Secret Key
        private static string clientSecret = "KbXq6fS7IkdAqMaNiB9WFLUwDYGu4aOi";
        public static string GetHvtImgUrls(string sHtmlText, string location)
        {
            SuperHero.LogWriter log = new SuperHero.LogWriter();
            string strbaser64 = string.Empty;
            Regex m_hvtRegImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            MatchCollection matches = m_hvtRegImg.Matches(sHtmlText);
            // 取得匹配项列表 
            FaceDetectSoapClient detectClient = new FaceDetectSoapClient();
            string savePath = detectClient.CreateDir("htmlImg");
            log.Write("FaceDetectSoapClient:" + savePath);
            int pic = 0;
            string fileName = Path.GetFileNameWithoutExtension(location);
            foreach (Match match in matches)
            {
                #region 图片
                var img = match.Groups["imgUrl"].Value;
                pic++;
                WebClient client = new WebClient();
                byte[] bytes = client.DownloadData(new Uri(img));
                MemoryStream ms = new MemoryStream(bytes);
                Bitmap bmp = new Bitmap(ms);
                string imagePath = Path.Combine(savePath, fileName + pic + ".jpg");
                bmp.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                log.Write("bmp.Save:" + imagePath);
                try
                {
                    #region baiduapi
                    var baiduCiient = new Face(clientId, clientSecret);
                    var image = File.ReadAllBytes(imagePath);
                    var options = new Dictionary<string, object>
                {
                    { "max_face_num","3"},//设置最多识别的脸数
                    {"face_fields", "beauty,age,gender,glasses,race"}
                };
                    var resJson = baiduCiient.FaceDetect(image, options);
                    JObject newObj1 = new JObject(resJson);
                    int resutNum = int.Parse(newObj1["result_num"].ToString());
                    #endregion
                    //#region Emgu.CV
                    //int resutNum = new FaceDetect.DetectFace().Run(imagePath);
                    //#endregion
                    if (resutNum != 1)
                    {
                        File.Delete(imagePath);
                    }
                    else
                    {
                        strbaser64 = AIProject.FileUtils.imageChangetoBase64(imagePath);
                    }
                }
                catch (Exception)
                {
                }
                #endregion
            }
            return strbaser64;
        }
    }
}
