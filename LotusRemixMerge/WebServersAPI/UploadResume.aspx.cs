using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebServersAPI
{
    public partial class UploadResume : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private string base64Str;
        protected void Button1_Click(object sender, EventArgs e)
        {

            string path = txtUpload.Text;  //界面上第一个文件路径  
            string tempPath = txtUpload.Text; //界面上第二个文件路径  
            FileStream filestream = new FileStream(path, FileMode.Open);

            byte[] bt = new byte[filestream.Length];

            //调用read读取方法  
            filestream.Read(bt, 0, bt.Length);
            this.base64Str = Convert.ToBase64String(bt);
            filestream.Close();

            //将Base64串写入临时文本文件  
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
            FileStream fs = new FileStream(tempPath, FileMode.Create);
            byte[] data = System.Text.Encoding.Default.GetBytes(base64Str);
            //开始写入  
            fs.Write(data, 0, data.Length);

            //清空缓冲区、关闭流  
            fs.Flush();
            fs.Close();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string outPath = txtUpload1.Text;  //界面上第三个文件路径  

            var contents = Convert.FromBase64String(this.base64Str);
            using (var fs = new FileStream(outPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(contents, 0, contents.Length);
                fs.Flush();
            }
        }




        private const int MAX_UPLOAD_SIZE = 2;

        /// <summary>
        /// 上传文件
        /// fileSelector是前台代码中文件控件的name
        /// </summary>
        /// <returns></returns>
        public string UploadFile(HttpRequest req, HttpResponse res)
        {
            if (req.Files["fileSelector"].ContentLength > MAX_UPLOAD_SIZE * 1024 * 1024)
            {
                return String.Format("请上传{0}M以内的文件。", MAX_UPLOAD_SIZE);
            }

            string uploadFileName = req.Files["fileSelector"].FileName;
            string path = HttpContext.Current.Server.MapPath(uploadFileName);
            req.Files["fileSelector"].SaveAs(path);
            return "";
        }


        protected void FileUploadButton_Click(object sender, EventArgs e)
        {
            if (MyFileUpload.HasFile)
            {
                string filePath = Server.MapPath("~/UploadFiles/");
                string fileName = MyFileUpload.PostedFile.FileName;
                MyFileUpload.SaveAs(filePath + fileName);
                Response.Write("<p >上传成功!</p>");
            }
            else
            {
                Response.Write("<p >请选择要上传的文件!</p>");
            }
        }
        protected void InputFileUploadButton_Click(object sender, EventArgs e)
        {
            HttpFileCollection files = Request.Files;
            string filePath = Server.MapPath("~/UploadFiles/");
            if (files.Count != 0)
            {
                string fileName = files[0].FileName;
                files[0].SaveAs(Path.Combine(filePath, fileName));
                Response.Write("<p>上传成功</p>");
            }
            else
            {
                Response.Write("<p>未获取到Files:" + files.Count.ToString() + "</p>");
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string filePath = Server.MapPath("~/UploadFiles/");
            AnalyzeResume ar = new WebServersAPI.AnalyzeResume();
           
            ar.ExtractResume(filePath + MyFileUpload.FileName);
        }
    }
}