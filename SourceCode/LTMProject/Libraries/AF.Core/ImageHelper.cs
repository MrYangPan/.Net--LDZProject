using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Core
{
    public partial class ImageHelper
    {
        /// <summary>
        /// 按比例计算宽度高度
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        /// <returns></returns>
        public static ImageParam ResizeCalculation(Stream stream, int maxWidth, int maxHeight)
        {
            Image image = Image.FromStream(stream);
            var param = new ImageParam { Width = image.Width, Height = image.Height };

            double rateWidth = param.Width / (double)maxWidth;
            double rateHeight = param.Height / (double)maxHeight;
            if (rateWidth >= rateHeight)
            {
                param.Width = maxWidth;
                param.Height = (int)Math.Round(param.Height / rateWidth);
            }
            else
            {
                param.Width = (int)Math.Round(param.Width / rateHeight);
                param.Height = maxHeight;
            }
            return param;
        }

        /// <summary>
        /// 剪切图片
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="cropWidth">Width of the crop.</param>
        /// <param name="cropHeight">Height of the crop.</param>
        /// <returns></returns>
        public static Image CropImage(byte[] content, int x, int y, int cropWidth, int cropHeight)
        {
            using (MemoryStream stream = new MemoryStream(content))
            {
                return CropImage(stream, x, y, cropWidth, cropHeight);
            }
        }

        /// <summary>
        /// 剪切图片
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="cropWidth">Width of the crop.</param>
        /// <param name="cropHeight">Height of the crop.</param>
        /// <returns></returns>
        public static Image CropImage(Stream content, int x, int y, int cropWidth, int cropHeight)
        {
            using (Bitmap sourceBitmap = new Bitmap(content))
            {
                // 将选择好的图片缩放
                Bitmap bitSource = new Bitmap(sourceBitmap, sourceBitmap.Width, sourceBitmap.Height);

                Rectangle cropRect = new Rectangle(x, y, cropWidth, cropHeight);

                Bitmap newBitMap = new Bitmap(cropWidth, cropHeight);
                newBitMap.SetResolution(sourceBitmap.HorizontalResolution, sourceBitmap.VerticalResolution);
                using (Graphics g = Graphics.FromImage(newBitMap))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;

                    g.DrawImage(bitSource, new Rectangle(0, 0, newBitMap.Width, newBitMap.Height), cropRect, GraphicsUnit.Pixel);

                    return newBitMap;
                }
            }
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="resizeWidth">Width of the resize.</param>
        /// <param name="resizeHeight">Height of the resize.</param>
        /// <returns></returns>
        public static Image ResizeImage(Stream content, int resizeWidth, int resizeHeight)
        {
            using (Bitmap sourceBitmap = new Bitmap(content))
            {
                // 将选择好的图片缩放
                Bitmap bitSource = new Bitmap(sourceBitmap, resizeWidth, resizeHeight);
                bitSource.SetResolution(sourceBitmap.HorizontalResolution, sourceBitmap.VerticalResolution);
                return bitSource;
            }

        }

        /// <summary>
        /// 将图片转换成内存流
        /// </summary>
        /// <param name="image">待转换的图片</param>
        /// <param name="format">转换的格式</param>
        /// <returns></returns>
        public static MemoryStream ToMemoryStream(Image image, ImageFormat format = null)
        {
            format = format ?? ImageFormat.Jpeg;
            MemoryStream ms = new MemoryStream();
            image.Save(ms, format);
            return ms;
        }

        /// <summary>
        /// Images to bytes.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image, ImageFormat format = null)
        {
            format = format ?? ImageFormat.Jpeg;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                byte[] buffer = new byte[ms.Length];
                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        /// <summary>
        /// Byteses to image.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            Image image = System.Drawing.Image.FromStream(ms);
            return image;
        }

        public class ImageParam
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}
