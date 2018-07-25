using System;
using System.IO;
using System.Net;
using System.Text;

namespace AIProject
{
    public class FaceDetect
    {
        // 人脸探测
        private static string clientId = "19YqVLf7f00ZMbZj4jGXWl42";
        private static string clientSecret = "KbXq6fS7IkdAqMaNiB9WFLUwDYGu4aOi";
        public static string detect(string filePath)
        {
            string token = AccessToken.getAccessToken(clientId, clientSecret);
            string host = "https://aip.baidubce.com/rest/2.0/face/v2/detect?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            // 图片的base64编码
            string base64 = FileUtils.imageChangetoBase64(filePath);
            string str = "max_face_num=" + 5 + "&face_fields=" + "age,beauty,expression,faceshape,gender,glasses,landmark,race,qualities" + "&image=" + System.Web.HttpUtility.UrlEncode(base64);
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            Console.WriteLine("人脸探测:");
            Console.WriteLine(result);
            return result;
        }
    }
}
