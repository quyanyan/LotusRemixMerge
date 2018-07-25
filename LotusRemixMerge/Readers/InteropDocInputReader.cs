using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using System.Drawing;
using System.IO;
using Baidu.Aip.Face;
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using SuperHero;
using Readers.ServiceReference1;
using DocumentFormat.OpenXml.Bibliography;

namespace Readers.Doc
{
    public class InteropDocInputReader : InputReaderBase
    {
        LogWriter log = new LogWriter();
        protected override bool CanHandle(string location)
        {
            //log.Write("canhandle/");
            return location.EndsWith("doc");
        }
        private static string clientId = "19YqVLf7f00ZMbZj4jGXWl42";
        // 百度云中开通对应服务应用的 Secret Key
        private static string clientSecret = "KbXq6fS7IkdAqMaNiB9WFLUwDYGu4aOi";
        public List<string> lstlines = null;
        protected override IList<string> Handle(string location)
        {
            Debugger.Launch();
            lstlines = new List<string>();
            Thread th_wordprocess = new Thread(new ParameterizedThreadStart(WordHandle));
            th_wordprocess.SetApartmentState(ApartmentState.STA);
            th_wordprocess.Start(location);
            th_wordprocess.Join();
            return lstlines;
        }

        protected void WordHandle(Object location)
        {
            var word = new Microsoft.Office.Interop.Word.Application();
            object miss = System.Reflection.Missing.Value;
            object path = location;
            object readOnly = true;
            var doc = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss,
                ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);

            lstlines = doc.Paragraphs.Cast<object>().Select((t, i) => doc.Paragraphs[i + 1].Range.Text).ToList();

            lstlines = lstlines.Select(x => x.Replace("\r\a", "").Trim())
                    .Where(x => x.Length > 0).ToList();
            #region 保存个人简历照片
            try
            {
                int pic = 0;
                foreach (Paragraph item in doc.Paragraphs)
                {
                    if (item != null)
                    {
                        if (item.Range.Text.Trim() != "")
                        {
                            //判断该范围内是否存在图片  
                            if (item.Range.InlineShapes.Count != 0)
                            {
                                foreach (InlineShape shape in item.Range.InlineShapes)
                                {
                                    pic++;
                                    var stype = shape.Type;
                                    if (stype == WdInlineShapeType.wdInlineShapePicture || stype == WdInlineShapeType.wdInlineShapeLinkedPicture)
                                    {
                                        FaceDetectSoapClient detectClient = new FaceDetectSoapClient();
                                        string savePath = detectClient.CreateDir("wordImg");
                                        string fileName = Path.GetFileNameWithoutExtension((string)location);
                                        #region EnhMetaFileBits
                                        byte[] img = (byte[])shape.Range.EnhMetaFileBits;
                                        Bitmap bmp = new Bitmap(new MemoryStream(img));
                                        string imagePath = Path.Combine(savePath, fileName + pic + ".jpg");
                                        bmp.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        #endregion
                                        #region use Clipboard save data
                                        //shape.Select(); //选定当前图片  
                                        //Selection sel = word.Selection;
                                        //sel.CopyAsPicture();
                                        //log.Write("wordhandle/e  Selection");
                                        //string imagePath = Path.Combine(savePath, fileName + pic + ".jpg");
                                        //if (Clipboard.ContainsImage())
                                        //{
                                        //    log.Write("wordhandle/e  ContainsImage");
                                        //    Image clipImage = Clipboard.GetImage();
                                        //    Bitmap bmp = new Bitmap(clipImage);
                                        //    bmp.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        //    Clipboard.Clear();
                                        //    sel.SetRange(sel.Start, sel.Start);
                                        //    log.Write("wordhandle/CopyAsPicture  1");
                                        //}
                                        #endregion
                                        try
                                        {

                                            #region baiduapi
                                            var baiduClient = new Face(clientId, clientSecret);
                                            var image = File.ReadAllBytes(imagePath);
                                            var options = new Dictionary<string, object>
                                        {
                                            { "max_face_num","3"},//设置最多识别的脸数
                                            {"face_fields", "beauty,age,gender,glasses,race"}
                                        };
                                            var resJson = baiduClient.FaceDetect(image, options);
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
                                                string strbaser64 = AIProject.FileUtils.imageChangetoBase64(imagePath);
                                                log.Write("wordhandle/imageChangetoBase64  2");
                                            }
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //  throw new Exception(ex.Message);
            }
            #endregion

            try
            {
                //word.NormalTemplate.Saved = false;
                //Object saveChanges = WdSaveOptions.wdPromptToSaveChanges;
                //Object originalFormat = Type.Missing;
                //Object routeDocument = Type.Missing;
                //word.DisplayAlerts = WdAlertLevel.wdAlertsMessageBox;
                //word.Documents.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                //log.Write("wordhandle/6");
                //object missing = Type.Missing;
                //object saveOption = WdSaveOptions.wdDoNotSaveChanges;
                //word.Quit(ref saveOption, ref missing, ref missing);
                //log.Write("wordhandle/7");

                doc.Close();
                word.Quit();
            }
            catch (Exception)
            {
                doc.Close();
                word.Quit();
            }
        }
    }
}
