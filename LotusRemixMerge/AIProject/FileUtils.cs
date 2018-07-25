using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIProject
{
    public static class FileUtils
    {
        public static string imageChangetoBase64(string filePath)
        {
            // 图片的base64编码
            Bitmap bmp = new Bitmap(filePath);
            //this.pictureBox1.Image = bmp;
            FileStream fs = new FileStream(filePath + ".txt", FileMode.Create);
            //StreamWriter sw = new StreamWriter(fs);

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Jpeg);
            byte[] arr = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(arr, 0, (int)ms.Length);
            ms.Close();
            String strbaser64 = Convert.ToBase64String(arr);
            fs.Close();
            try
            {
                File.Delete(filePath + ".txt");
            }
            catch (Exception)
            {
            }
            return strbaser64;
        }
    }
}
