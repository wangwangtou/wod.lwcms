﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace wod.imageTool.imageLib
{
    public class common
    {
        private static readonly List<string> ImageExtensions = new List<string>()
        {
        //public static ImageFormat Bmp { get; }
        //public static ImageFormat Emf { get; }
        //public static ImageFormat Exif { get; }
        //public static ImageFormat Gif { get; }
        //public static ImageFormat Icon { get; }
        //public static ImageFormat Jpeg { get; }
        //public static ImageFormat MemoryBmp { get; }
        //public static ImageFormat Png { get; }
        //public static ImageFormat Tiff { get; }
        //public static ImageFormat Wmf { get; }
            "BMP",
            "EMF",
            "EXIF",
            "GIF",
            "ICON",
            "JPEG",
            "JPG",
            "PNG",
        };

        public static bool IsImage(FileInfo file)
        {
            return ImageExtensions.IndexOf(file.Extension.ToUpper().Substring(1)) > -1;
        }

        public static bool IsImage(string file)
        {
            return IsImage(new FileInfo(file));
        }

        public static void ResizeImages(bool ratioScale, string targetFolder, List<string> sourceImage, double? maxW, double? maxH)
        {
            foreach (string imgFile in sourceImage)
            {
                using (Bitmap img = new Bitmap(imgFile))
                {
                    try
                    {
                        var rw = img.Width;
                        var rh = img.Height;

                        GetNewSize(ratioScale, maxW, maxH, ref rw, ref rh);

                        var nImg = new Bitmap((int)rw, (int)rh);
                        Graphics g = Graphics.FromImage(nImg);
                        g.Clear(Color.Transparent);
                        g.DrawImage(img, 0, 0, (int)rw, (int)rh);

                        nImg.Save(Path.Combine(targetFolder, new FileInfo(imgFile).Name), img.RawFormat);
                        nImg.Dispose();
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
        }

        private static void GetNewSize(bool ratioScale, double? maxW, double? maxH, ref int rw, ref int rh)
        {
            if (ratioScale)
            {
                if (maxH == null)
                {
                    var w = maxW ?? rw;
                    rh = (int)Math.Round((double)rh / rw * w);
                    rw = (int)w;
                }
                else if (maxW == null)
                {
                    var h = maxH ?? rh;
                    rw = (int)Math.Round((double)rw / rh * h);
                    rh = (int)h;
                }
                else
                {
                    if (maxW / maxH <= rw / rh)
                    {
                        var w = maxW ?? rw;
                        rh = (int)Math.Round((double)rh / rw * w);
                        rw = (int)w;
                    }
                    else
                    {
                        var h = maxH ?? rh;
                        rw = (int)Math.Round((double)rw / rh * h);
                        rh = (int)h;
                    }
                }
            }
            else
            {
                rw = (int)(maxW ?? rw);
                rh = (int)(maxH ?? rh);
            }
        }
    }
}
