using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using OpenCvSharp.Extensions;

namespace CSharpOpenCVLiberiesConvert
{
    /// <summary>
    /// 使用OpenCv封装库之间的转换
    /// </summary>
    public class OpenCVLibraryConverter
    {
        #region MatOpenCVSharpToEmgu (public : OpenCVSharp库的Mat转EmguCV库的Mat)
        public static Emgu.CV.Mat MatOpenCVSharpToEmgu(OpenCvSharp.Mat opcvsMat)
        {
            #region Obsolte,convert through bytes,可能丢失某些信息
            //var emguMat = new Emgu.CV.Mat();
            //Emgu.CV.CvInvoke.Imdecode(opcvsMat.ToBytes(), Emgu.CV.CvEnum.LoadImageType.AnyColor, emguMat);            
            //return emguMat;
            #endregion
            #region Obsolte,convert through bitmap,可能丢失某些信息
            //System.Drawing.Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(opcvsMat.ToIplImage());
            //var img = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte>(bitmap);
            //return img.Mat;
            #endregion

            #region 正在应用,OpenCvSharp CvPtr指针 Emgu CvArrToMat
            var emptr = Emgu.CV.CvInvoke.CvArrToMat(opcvsMat.CvPtr, true);
            return emptr;
            #endregion
        }
        #endregion

        #region MatEmguToOpenCVSharp (public : EmguCV库的Mat转OpenCVSharp库的Mat)
        public static OpenCvSharp.Mat MatEmguToOpenCVSharp(Emgu.CV.Mat emguMat)
        {
            #region 预留,bitmap转换,可能丢失某些信息
            //var opcvsMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(emguMat.Bitmap);
            //return opcvsMat;
            #endregion

            #region 正在应用,Emgu指针,new OpenCvSharp.Mat(IntPtr)
            var ptrMat = new OpenCvSharp.Mat(emguMat.Ptr);
            return ptrMat;
            #endregion
        }
        #endregion

        #region LoadMatFromFile (public : 从文件读取Mat)
        public static void LoadMatFromFile(String fileName, ref Emgu.CV.Mat mat)
        {
            mat = null;
            mat = new Emgu.CV.Mat(fileName, Emgu.CV.CvEnum.LoadImageType.AnyColor);
        }

        public static void LoadMatFromFile(String fileName, ref OpenCvSharp.Mat mat)
        {
            mat = null;
            mat = new OpenCvSharp.Mat(fileName);
        }
        #endregion

        #region ShowImage (public : 通过CV.ImShow显示一个预览Mat的窗口)
        public static void ShowImage(Emgu.CV.Mat mat, String windowName = "EmguCVMat")
        {
            Emgu.CV.CvInvoke.Imshow(windowName, mat);
        }

        public static void ShowImage(OpenCvSharp.Mat mat, String windowName = "OpenCVSharpMat")
        {
            OpenCvSharp.Cv2.ImShow(windowName, mat);
        }

        public static void ShowImage(String nameSeires, params OpenCvSharp.Mat[] mats)
        {
            for (Int32 i = 0; i < mats.Length; i++)
            {
                OpenCvSharp.Cv2.ImShow(nameSeires + i, mats[i]);
            }
        }

        public static void ShowImage(String nameSeires, params Emgu.CV.Mat[] mats)
        {
            for (Int32 i = 0; i < mats.Length; i++)
            {
                Emgu.CV.CvInvoke.Imshow(nameSeires + i, mats[i]);
            }
        }
        #endregion

        #region CloseAllShowingWindow (public : 销毁所有通过CV.ImShow打开的窗口)
        public static void CloseAllShowingWindow()
        {
            CloseShowingWindow(new OpenCvSharp.Mat());
            CloseShowingWindow(new Emgu.CV.Mat());
        }
        #endregion

        #region CloseShowingWindow (public : 销毁CV.ImShow打开的窗口)
        public static void CloseShowingWindow(OpenCvSharp.Mat m)
        {
            m.Dispose();
            OpenCvSharp.Cv2.DestroyAllWindows();
        }

        public static void CloseShowingWindow(OpenCvSharp.Mat m, String windowName)
        {
            m.Dispose();
            OpenCvSharp.Cv2.DestroyWindow(windowName);
        }

        public static void CloseShowingWindow(Emgu.CV.Mat m)
        {
            m.Dispose();
            Emgu.CV.CvInvoke.DestroyAllWindows();
        }

        public static void CloseShowingWindow(Emgu.CV.Mat m, String windowName)
        {
            m.Dispose();
            Emgu.CV.CvInvoke.DestroyWindow(windowName);
        }
        #endregion

        #region BitmapToEmguMat (public : Bitmap转EmguMat)
        #region locker (private : 防止文件读写出错的多线程锁)
        static object locker = new object();
        #endregion
        public static Emgu.CV.Mat BitmapToEmguMat(System.Drawing.Bitmap bitmap)
        {
            Emgu.CV.Mat mat = null;
            #region 文件读写法
            //lock (locker)
            //{
            //    DateTime time = System.DateTime.Now;
            //    String tempFilePath = String.Format(@"tempBitmapToEmguMat_{0}_{1}.bmp", time.Minute, time.Second);
            //    using (System.IO.FileStream fs = System.IO.File.Create(tempFilePath))
            //    {
            //        bitmap.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
            //        bitmap.Dispose();
            //    }
            //    mat = new Emgu.CV.Mat(tempFilePath, Emgu.CV.CvEnum.LoadImageType.AnyColor);
            //    if (System.IO.File.Exists(tempFilePath))
            //    {
            //        System.IO.File.Delete(tempFilePath);
            //    }
            //}
            using (OpenCvSharp.Mat image = bitmap.ToMat())
            {
                mat = MatOpenCVSharpToEmgu(image);
            }
            #endregion
            return mat;
        }
        #endregion

        #region BitmapToOpenCVSharpMat (public : Bitmap转OpenCVSharpMat)
        public static OpenCvSharp.Mat BitmapToOpenCVSharpMat(System.Drawing.Bitmap bitmap)
        {
            var mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(bitmap);
            return mat;
        }
        #endregion

        #region OpenCVLibraryToBitmap (public : EmguCVMat转Bitmap)
        public static System.Drawing.Bitmap OpenCVLibraryToBitmap(Emgu.CV.Mat mat)
        {
            return mat.Bitmap;
        }
        #endregion

        #region OpenCVLibraryToBitmap (public : OpenCVSharpMat转Bitmap)
        public static System.Drawing.Bitmap OpenCVLibraryToBitmap(OpenCvSharp.Mat mat)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mat.Clone());
            //return OpenCVLibraryToBitmap(MatOpenCVSharpToEmgu(mat));
        }
        #endregion

        #region BitmapSourceToEmguCvMat (public : BitmapSource转EmguCVMat)
        public static Emgu.CV.Mat BitmapSourceToEmguCvMat(System.Windows.Media.Imaging.BitmapSource source)
        {
            if (source.Format == PixelFormats.Bgra32)
            {
                Emgu.CV.Mat result = new Emgu.CV.Mat();
                result.Create(source.PixelHeight, source.PixelWidth, Emgu.CV.CvEnum.DepthType.Cv8U, 4);
                source.CopyPixels(Int32Rect.Empty, result.DataPointer, result.Step * result.Rows, result.Step);
                return result;
            }
            else if (source.Format == PixelFormats.Bgr24)
            {
                Emgu.CV.Mat result = new Emgu.CV.Mat();
                result.Create(source.PixelHeight, source.PixelWidth, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
                source.CopyPixels(Int32Rect.Empty, result.DataPointer, result.Step * result.Rows, result.Step);
                return result;
            }
            else
            {
                throw new Exception(String.Format("Convertion from BitmapSource of format {0} is not supported.", source.Format));
            }
        }
        #endregion

        #region BitmapSourceToOpenCvSharpMat (public : BitmapSource转OpenCvSharpMat)
        public static OpenCvSharp.Mat BitmapSourceToOpenCvSharpMat(System.Windows.Media.Imaging.BitmapSource source)
        {
            OpenCvSharp.Mat mat = source.ToMat();
            source.Freeze();
            source = null;
            return mat;
            //return OpenCvSharp.Extensions.BitmapSourceConverter.ToMat(source);
        }
        #endregion

        #region OpenCVLibraryToBitmapSource (public : EmguCVMat转BitmapSource)
        /// <summary>
        /// Delete a GDI object
        /// </summary>
        /// <param name="o">The poniter to the GDI object to be deleted</param>
        /// <returns></returns>
        [DllImport("gdi32")]
        private static extern Int32 DeleteObject(IntPtr o);
        /// <summary>
        /// Convert an IImage to a WPF BitmapSource. The result can be used in the Set Property of Image.Source
        /// </summary>
        /// <param name="image">The Emgu CV Image</param>
        /// <returns>The equivalent BitmapSource</returns>
        public static System.Windows.Media.Imaging.BitmapSource OpenCVLibraryToBitmapSource(Emgu.CV.IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

                System.Windows.Media.Imaging.BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    System.Windows.Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr); //release the HBitmap
                return bs;
            }
        }
        #endregion

        #region OpenCVLibraryToBitmapSource (public : OpenCvSharpMat转BitmapSource)
        public static System.Windows.Media.Imaging.BitmapSource OpenCVLibraryToBitmapSource(OpenCvSharp.Mat mat)
        {
            System.Windows.Media.Imaging.BitmapSource source = mat.ToWriteableBitmap();
            source.Freeze();
            DeleteObject(mat.CvPtr);
            return source;
            //return OpenCvSharp.Extensions.BitmapSourceConverter.ToBitmapSource(mat);
        }
        #endregion
        #region BitmapToBitmapSource (public : Bitmap转WPF System.Windows.Media.Imaging.BitmapSource)
        public static System.Windows.Media.Imaging.BitmapSource BitmapToBitmapSource(System.Drawing.Bitmap bmp, System.IO.MemoryStream ms)
        {
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bytes = ms.GetBuffer();

            // Init bitmap
            System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new System.IO.MemoryStream(bytes);
            bitmap.EndInit();
            return bitmap;
        }
        #endregion
    }
}
