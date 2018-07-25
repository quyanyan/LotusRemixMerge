using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Cuda;

namespace FaceDetect
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FaceDetect());
            //string filepath = @"C:\Users\Administrator\Desktop\简历整合版本\LotusRemixMerge\FatBear\bin\Debug\Resumes\wordImg\QP\QP1.jpg";
            //int i = new DetectFace().Run(filepath);
        }       
    }
}
