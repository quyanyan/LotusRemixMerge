using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceDetect.Model
{
    /// <summary>
    /// 存放所有人脸样本的
    /// </summary>
    public class TrainedFileList
    {
        public List<Image<Gray, byte>> trainedImages = new List<Image<Gray, byte>>();
        public List<int> trainedLabelOrder = new List<int>();
        public List<string> trainedFileName = new List<string>();      
        public TrainedFileList SetSampleFaceList() {
            TrainedFileList tfList = new TrainedFileList();
            //获取指定目录下所有的样本文件，并添加到TrainedFileList类的集合中
            DirectoryInfo dri = new DirectoryInfo("");//facesamplePath
            int i = 0;
            foreach (FileInfo fi in dri.GetFiles()) {
                tfList.trainedImages.Add(new Image < Gray, byte >(fi.FullName));
                tfList.trainedLabelOrder.Add(i);
                tfList.trainedFileName.Add(fi.Name);//.Split('_')[0]
                i++;
            }
            return tfList;
        }
    }
    /// <summary>
    /// 存放训练好的人脸识别器
    /// </summary>
    public class TrainedFaceRecognizer
    {
        public Emgu.CV.Face.FaceRecognizer faceRecognizer;
        public TrainedFileList trainedFileList;
        public TrainedFaceRecognizer SetTrainedFaceRecognizer(FaceRecognizerType faceRecType)
        {
            TrainedFaceRecognizer tfr = new TrainedFaceRecognizer();
            tfr.trainedFileList = new TrainedFileList().SetSampleFaceList();
            //根据人脸识别器的类型进行不同的参数设定，并进行训练
            switch (faceRecType)
            {
                case FaceRecognizerType.EigenFaceRecognizer:
                    tfr.faceRecognizer = new Emgu.CV.Face.EigenFaceRecognizer(80, double.PositiveInfinity);
                    break;
                case FaceRecognizerType.FisherFaceRecognizer:
                    tfr.faceRecognizer = new Emgu.CV.Face.FisherFaceRecognizer(80, 3500);
                    break;
                case FaceRecognizerType.LBPHFaceRecognizer:
                    tfr.faceRecognizer = new Emgu.CV.Face.LBPHFaceRecognizer(1, 8, 8, 8, 100);
                    break;
            }
            tfr.faceRecognizer.Train(tfr.trainedFileList.trainedImages.ToArray(),tfr.trainedFileList.trainedLabelOrder.ToArray());
            return tfr;
        }
    }
    /// <summary>
    /// 存放识别后相关信息
    /// </summary>
    public class FaceDetectedObj {
        public Mat originalImg;//原始图片
        public List<Rectangle> facesRectangle;//识别后的人脸矩形框，包括坐标和大小
        public List<string> names = new List<string>();//识别出来的人的姓名
        //将图片中的人脸识别出来
        public FaceDetectedObj GetFaceRectangle(Mat emguImage)
        {
            FaceDetectedObj faceDetectObj = new FaceDetectedObj();
            faceDetectObj.originalImg = emguImage;
            List<Rectangle> faces = new List<Rectangle>();
            try
            {
                using (UMat ugray = new UMat())
                {
                    CvInvoke.CvtColor(emguImage, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);//灰度化图片
                    CvInvoke.EqualizeHist(ugray, ugray);//均衡灰度化图片
                    CascadeClassifier faceClassifier = new CascadeClassifier();//检测人脸
                    Rectangle[] faceDetected = faceClassifier.DetectMultiScale(ugray, 1.1, 10, new Size(20, 20));
                    faces.AddRange(faceDetected);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            faceDetectObj.facesRectangle = faces;
            return faceDetectObj;
        }
        public FaceDetectedObj GetFaceRecognize(Mat emguImage)
        {
            FaceDetectedObj fdo = GetFaceRectangle(emguImage);
            Image<Gray, byte> tempImg = fdo.originalImg.ToImage<Gray, byte>();
            #region
            try
            {
                using (Graphics g = Graphics.FromImage(fdo.originalImg.Bitmap))
                {
                    foreach (Rectangle face in fdo.facesRectangle)
                    {
                        g.DrawRectangle(new Pen(Color.Red,2),face);//画出人脸框
                        Image<Gray, byte> GrayFace = tempImg.Copy(face).Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);
                        GrayFace._EqualizeHist();//得到均衡化人脸的灰化图片
                        #region 得到匹配姓名并画出
                        TrainedFaceRecognizer tfr= new TrainedFaceRecognizer();
                        //关键的人脸识别程序：Emgu.CV.Face.FaceRecognizer.PredictionResult
                        Emgu.CV.Face.FaceRecognizer.PredictionResult pr = tfr.faceRecognizer.Predict(GrayFace);
                        string recognizeName = tfr.trainedFileList.trainedFileName[pr.Label].ToString();
                        string name = tfr.trainedFileList.trainedFileName[pr.Label].ToString();
                        Font font = new Font("宋体", 16, GraphicsUnit.Pixel);
                        SolidBrush fontLine = new SolidBrush(Color.Yellow);
                        float xpos = face.X + (face.Width / 2 - (name.Length * 14) / 2);
                        float ypos = face.Y - 21;
                        g.DrawString(name,font,fontLine,xpos,ypos);
                        #endregion
                        fdo.names.Add(name);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            #endregion
            return fdo;
        }
    }
    public enum FaceRecognizerType {
        EigenFaceRecognizer=0,
        FisherFaceRecognizer=1,
        LBPHFaceRecognizer=2
    }
    public class RecognizerType
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
