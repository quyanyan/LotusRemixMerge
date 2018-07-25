using System;
using System.Drawing;
using System.Windows.Forms;

using System.Timers;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Cuda;
using Emgu.CV.UI;

using System.Collections.Generic;
using FaceDetect.Model;
using Emgu.CV.CvEnum;

namespace FaceDetect
{
    public partial class FaceDetect : Form
    {
        //Mat matImg;//摄像头图像
        //Emgu.CV.Capture capture;//摄像头对象
        //KingFaceDetect kfd;
        public FaceDetect()
        {
            InitializeComponent();
            //BindComBox();
            //CvInvoke.UseOpenCL = false;
            //kfd = new KingFaceDetect();
            //try {
            //    capture = new Emgu.CV.Capture();
            //    capture.Start();
            //    capture.ImageGrabbed += frameProcess;
            //} catch (NullReferenceException ex) {
            //    MessageBox.Show(ex.Message);
            //}
            IImage image;

            //Read the files as an 8-bit Bgr image  

            image = new UMat("lena.jpg", ImreadModes.Color); //UMat version
            long detectionTime;
            List<Rectangle> faces = new List<Rectangle>();
            List<Rectangle> eyes = new List<Rectangle>();
            DetectFace.Detect(
           image, "haarcascade_frontalface_default.xml", "haarcascade_eye.xml",
           faces, eyes,
           out detectionTime);

            foreach (Rectangle face in faces)
                CvInvoke.Rectangle(image, face, new Bgr(Color.Red).MCvScalar, 2);
            foreach (Rectangle eye in eyes)
                CvInvoke.Rectangle(image, eye, new Bgr(Color.Blue).MCvScalar, 2);

            //display the image 
            using (InputArray iaImage = image.GetInputArray())
                ImageViewer.Show(image, String.Format(
                   "Completed face and eye detection using {0} in {1} milliseconds",
                   (iaImage.Kind == InputArray.Type.CudaGpuMat && CudaInvoke.HasCuda) ? "CUDA" :
                   (iaImage.IsUMat && CvInvoke.UseOpenCL) ? "OpenCL"
                   : "CPU",
                   detectionTime));
        }
       
        public void BindComBox() {
            List<RecognizerType> infoList = new List<RecognizerType>();
            RecognizerType info1 = new RecognizerType() { Id = "1", Name = "EigenFaceRecognizer" };
            RecognizerType info2 = new RecognizerType() { Id = "2", Name = "FisherFaceRecognizer" };
            RecognizerType info3 = new RecognizerType() { Id = "3", Name = "LBPHFaceRecognizer" };
            infoList.Add(info1);
            infoList.Add(info2);
            infoList.Add(info3);
            recognizerType.DataSource = infoList;
            recognizerType.ValueMember = "Id";
            recognizerType.DisplayMember = "Name";
            recognizerType.SelectedIndex = 0;
        }
    }  
}