using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServiceReference1.AnalyzeResumeSoapClient client = new ServiceReference1.AnalyzeResumeSoapClient();
            //上传服务器后的文件名  一般不修改文件名称
            int start = textBox1.Text.LastIndexOf("\\");
            int length = textBox1.Text.Length;
            //string serverfile = textBox1.Text.Substring(start + 1, length - textBox1.Text.LastIndexOf("."))
            //        + DateTime.Now.ToString("-yyyy-mm-dd-hh-mm-ss")
            //        + textBox1.Text.Substring(textBox1.Text.LastIndexOf("."), textBox1.Text.Length - textBox1.Text.LastIndexOf("."));

            string serverfile = textBox1.Text.Substring(textBox1.Text.LastIndexOf("."), textBox1.Text.Length - textBox1.Text.LastIndexOf(".")); ;
            string start1 = textBox1.Text;
            client.CreateFile(start1);
            //要上传文件的路径
            string sourceFile = textBox1.Text;
            string md5 = GetMD5(sourceFile);

            FileStream fs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            int size = (int)fs.Length;
            int bufferSize = 1024 * 512;
            int count = (int)Math.Ceiling((double)size / (double)bufferSize);
            var serverFilePath = "";
            for (int i = 0; i < count; i++)
            {
                int readSize = bufferSize;
                if (i == count - 1)
                    readSize = size - bufferSize * i;
                byte[] buffer = new byte[readSize];
                fs.Read(buffer, 0, readSize);

                int index = start1.LastIndexOf(@"\");
                string locationPath = start1.Substring(0, index);
                var outputFolder = Path.Combine(locationPath + @"\Output\");

                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
                serverFilePath =client.Append(start1, buffer);
               string[] output= client.ExtractResume(serverFilePath);

                var j = 0; var outputFileName = "";
                foreach (var item in output)
                {
                    using (var writer = new StreamWriter(Path.Combine(outputFolder, outputFileName + j + ".txt")))
                    {
                        writer.Write(item);
                    }
                    j++;
                }
            }

            bool isVerify = client.Verify(start1, md5);

            if (isVerify)
                MessageBox.Show("解析成功");
            else
                MessageBox.Show("解析失败");

















            //string filePath=textBox1.Text;
            //FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //BinaryReader br = new BinaryReader(fs);
            //byte[] bytes = br.ReadBytes((int)fs.Length);
            //fs.Flush();
            //fs.Close();
            ////定义并实例化一个内存流，以存放提交上来的字节数组。
            ////var uploadFolder = Path.Combine(@"http://192.168.88.122:8081/uploadfile/" + @"\UploadFolder\");
            //// Directory.CreateDirectory(uploadFolder);

            //var fileName = @"http://192.168.88.122:8081/uploadfile/";
            //MemoryStream m = new MemoryStream(bytes);
            //FileStream f = new FileStream(@"D:\\uploadfile\\", FileMode.Create);
            ////把内内存里的数据写入物理文件
            //m.WriteTo(f);
            //m.Close();
            ////定义实际文件对象，保存上载的文件。
            
            //ServiceReference1.AnalyzeResumeSoapClient client = new ServiceReference1.AnalyzeResumeSoapClient();
            ////WebServersAPI.AnalyzeResume client = new WebServersAPI.AnalyzeResume();
            //client.ExtractResume(fileName.ToString());
           
        }

        private string GetMD5(string fileName)
        {
            string md5Str = "";

            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            MD5CryptoServiceProvider p = new MD5CryptoServiceProvider();
            byte[] md5buffer = p.ComputeHash(fs);
            fs.Close();

            // List<string> strList = new List<string>();
            for (int i = 0; i < md5buffer.Length; i++)
            {
                md5Str += md5buffer[i].ToString("x2");
            }
            return md5Str;
        }
    }
}
