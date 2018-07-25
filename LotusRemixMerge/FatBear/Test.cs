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
using Microsoft.Office.Interop.Word;

namespace FatBear
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //    //localhost.WebService1 client = new localhost.WebService1();  
            //    //WindowsFormsApp.ServiceReference1.Service1SoapClient client = new WindowsFormsApp.ServiceReference1.Service1SoapClient();
            //    WindowsFormsApp.ServiceReference1.Service1SoapClient client = new WindowsFormsApp.ServiceReference1.Service1SoapClient();
            //    //WindowsFormsApp.ServiceReference1.WebService1SoapClient client = new WindowsFormsApp.ServiceReference1.WebService1SoapClient();
            //    //上传服务器后的文件名  一般不修改文件名称  

            //    string serverfile = textBox1.Text.Substring(start + 1, length - textBox1.Text.LastIndexOf("."))
            //            + DateTime.Now.ToString("-yyyy-mm-dd-hh-mm-ss")
            //            + textBox1.Text.Substring(textBox1.Text.LastIndexOf("."), textBox1.Text.Length - textBox1.Text.LastIndexOf("."));
            //    client.CreateFile(serverfile);
            //    //要上传文件的路径  
            //    string sourceFile = textBox1.Text;
            //    string md5 = GetMD5(sourceFile);
            //    FileStream fs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //    int size = (int)fs.Length;
            //    int bufferSize = 1024 * 512;
            //    int count = (int)Math.Ceiling((double)size / (double)bufferSize);
            //    for (int i = 0; i < count; i++)
            //    {
            //        int readSize = bufferSize;
            //        if (i == count - 1)
            //            readSize = size - bufferSize * i;
            //        byte[] buffer = new byte[readSize];
            //        fs.Read(buffer, 0, readSize);
            //        client.Append(serverfile, buffer);
            //    }
            //    bool isVerify = client.Verify(serverfile, md5);
            //    if (isVerify)
            //        MessageBox.Show("上传成功");
            //    else
            //        MessageBox.Show("上传失败");
            //}
            //private string GetMD5(string fileName)
            //{
            //    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            //    MD5CryptoServiceProvider p = new MD5CryptoServiceProvider();
            //    byte[] md5buffer = p.ComputeHash(fs);
            //    fs.Close();
            //    string md5Str = "";
            //    // List strList = new List();
            //    for (int i = 0; i < md5buffer.Length; i++)
            //    {
            //        md5Str += md5buffer[i].ToString("x2");
            //    }
            //    return md5Str;
            //}
            //private void button2_Click(object sender, EventArgs e)
            //{
            //    OpenFileDialog openDialog = new OpenFileDialog();
            //    //openDialog.Filter = "视频文件(*.avi,*.wmv,*.mp4)|*.avi;*.wmv;*.mp4";
            //    if (openDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        textBox1.Text = openDialog.FileName;
            //    }
        }
    }
}
