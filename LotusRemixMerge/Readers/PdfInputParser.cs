using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Baidu.Aip.Face;
using Model;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp.text.pdf.parser;
using Dotnet = System.Drawing.Image;
using Readers.ServiceReference1;
using Newtonsoft.Json.Linq;

namespace Readers.Pdf
{
    public class PdfInputReader : InputReaderBase
    {
        protected override bool CanHandle(string location)
        {
            return location.EndsWith("pdf");
        }
        private static string clientId = "19YqVLf7f00ZMbZj4jGXWl42";
        // 百度云中开通对应服务应用的 Secret Key
        private static string clientSecret = "KbXq6fS7IkdAqMaNiB9WFLUwDYGu4aOi";
        protected override IList<string> Handle(string location)
        {
            var lines = new List<string>();

            if (!File.Exists(location))
            {
                throw new ArgumentException("File not exists: " + location);
            }
            using (var pdfReader = new PdfReader(location))
            {
                int pic = 0;
                for (var page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    var strategy = new SimpleTextExtractionStrategy();
                    var currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                    currentText = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    lines.AddRange(currentText.Split(new[] { "\n" }, StringSplitOptions.None));

                    #region Extract and save image from pdf
                    PdfDictionary pg = pdfReader.GetPageN(page);
                    PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
                    PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));//得到XOBJECT图片对象
                    try
                    {
                        if (xobj != null)
                        {
                            foreach (PdfName name in xobj.Keys)
                            {
                                PdfObject obj = xobj.Get(name);
                                if (obj.IsIndirect())
                                {
                                    PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
                                    //ExtractImg(location);  
                                    pic++;
                                    string fileName = System.IO.Path.GetFileNameWithoutExtension(location);
                                    FaceDetectSoapClient detectClient = new FaceDetectSoapClient();
                                    string savePath = detectClient.CreateDir("pdfImg");
                                    #region
                                    ImageRenderInfo imgRI = ImageRenderInfo.CreateForXObject(new GraphicsState(), (PRIndirectReference)obj, tg);
                                    PdfImageObject pdfImage = imgRI.GetImage();
                                    using (Dotnet dotnetImg = pdfImage.GetDrawingImage())
                                    {
                                        if (dotnetImg != null)
                                        {
                                            using (MemoryStream ms = new MemoryStream())
                                            {
                                                dotnetImg.Save(ms, ImageFormat.Jpeg);
                                                Bitmap bmp = new Bitmap(dotnetImg);
                                                string imagePath = System.IO.Path.Combine(savePath, fileName + pic + ".jpg");
                                                bmp.Save(imagePath, ImageFormat.Jpeg);
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
                                                        //lines.Add(strbaser64);
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                    #endregion
                }
            }

            return lines;
        }
    }
}
