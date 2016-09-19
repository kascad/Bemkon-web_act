using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Data.Linq;
using System.Drawing.Imaging;

namespace Shared
{
    public class ImageHelper
    {
        public static Image GetImage(Binary photo)
        {
            var memSream = new MemoryStream(photo.Length);
            var buffer = photo.ToArray();
            memSream.Write(buffer, 0, photo.Length);
            return Image.FromStream(memSream);
        }

        public static Binary GetBinary(Image image)
        {
            var memSream = new MemoryStream();
            image.Save(memSream, image.RawFormat);
            var bytes = memSream.ToArray();
            return new System.Data.Linq.Binary(bytes);
        }

        public static string GetMineImageFormat(System.Drawing.Image image)
        {
            if (image.RawFormat.Equals(ImageFormat.Jpeg))
                return "image/jpeg";
            else if (image.RawFormat.Equals(ImageFormat.Bmp))
                return "image/bmp";
            else if (image.RawFormat.Equals(ImageFormat.Emf))
                return "image/emf";
            else if (image.RawFormat.Equals(ImageFormat.Exif))
                return "image/exif";
            else if (image.RawFormat.Equals(ImageFormat.Gif))
                return "image/gif";
            else if (image.RawFormat.Equals(ImageFormat.Icon))
                return "image/icon";
            else if (image.RawFormat.Equals(ImageFormat.Png))
                return "image/png";
            else if (image.RawFormat.Equals(ImageFormat.Tiff))
                return "image/tiff";
            else if (image.RawFormat.Equals(ImageFormat.Wmf))
                return "image/wmf";
            return "";
        }
    }
}
