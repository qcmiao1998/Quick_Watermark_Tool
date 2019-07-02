using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.MetaData.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace QuickWatermarkTool.Models
{
    public class Photo
    {
        public static List<Photo> PhotoList;

        private Image<Rgba32> originImage;
        private Image<Rgba32> watermarkImage;

        public string FileName { get; }

        public int Width => originImage.Width;

        public int Height => originImage.Height;

        private int WmHeight => watermarkImage.Height;
        private int WmWidth => watermarkImage.Width;

        public int FrameCount => originImage.Frames.Count;

        public Photo(string path)
        {
            originImage = Image.Load(path);
            watermarkImage = Image.Load(Config.config.WatermarkFilename);
            Path.GetFileNameWithoutExtension(path);
        }

        public void Watermark()
        {
            int wmPosiW, wmPosiH;
            int maxWmWidth = (int)Math.Floor(Config.config.MaxWatermarkScaleWidth * Width);
            int maxWmHeight = (int)Math.Floor(Config.config.MaxWatermarkScaleHeight * Height);

            if (Width > Config.config.MaxOutputImageWidth || Height > Config.config.MaxOutputImageHeight)
                ResizePic(originImage, Config.config.MaxOutputImageWidth, Config.config.MaxOutputImageHeight);
            ResizePic(watermarkImage, maxWmWidth, maxWmHeight);

            int offsetW, offsetH;
            offsetW = Config.config.WatermarkOffsetWidth;
            offsetH = Config.config.WatermarkOffsetHeight;

            switch (Config.config.WatermarkPosition)
            {
                case WatermarkPosition.LeftBottom:
                    wmPosiW = offsetW;
                    wmPosiH = Height - WmHeight - offsetH;
                    break;
                case WatermarkPosition.LeftTop:
                    wmPosiW = offsetW;
                    wmPosiH = offsetH;
                    break;
                case WatermarkPosition.RigthBottom:
                    wmPosiW = Width - WmWidth - offsetW;
                    wmPosiH = Height - WmHeight - offsetH;
                    break;
                case WatermarkPosition.RightTop:
                    wmPosiW = Width - WmWidth - offsetW;
                    wmPosiH = offsetH;
                    break;
                case WatermarkPosition.Center:
                    wmPosiW = (Width - WmWidth) / 2;
                    wmPosiH = (Height - WmHeight) / 2;
                    break;
                case WatermarkPosition.BottomMiddle:
                    wmPosiW = (Width - WmWidth) / 2;
                    wmPosiH = Height - offsetH - WmHeight;
                    break;
                case WatermarkPosition.TopMiddle:
                    wmPosiW = (Width - WmWidth) / 2;
                    wmPosiH = offsetH;
                    break;
                default:
                    throw new NotImplementedException();
            }
            originImage.Mutate(i => { i.DrawImage(watermarkImage, new Point(wmPosiW, wmPosiH), Config.config.WatermarkOpacity); });
        }

        public void AddCopyright()
        {
            var newExifProfile = originImage.MetaData.ExifProfile == null ? new ExifProfile() : new ExifProfile(originImage.MetaData.ExifProfile.ToByteArray());
            if (Config.config.Copyright != "")
            {
                newExifProfile.SetValue(ExifTag.Copyright, Config.config.Copyright);
            }

            if (Config.config.AuthorName != "")
            {
                newExifProfile.SetValue(ExifTag.Artist, Config.config.AuthorName);
            }
            originImage.MetaData.ExifProfile = newExifProfile;
        }

        private static void ResizePic(Image<Rgba32> image, int maxWidth, int maxHeight)
        {
            ResizeOptions resizeOptions = new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(maxWidth, maxHeight)
            };
            image.Mutate(x => x.Resize(resizeOptions));
        }

        public void SaveImage(string savePath, Format saveFormat)
        {
            string saveName = FileName + "." + saveFormat;
            string filepath = Path.Combine(savePath, saveName);
            switch (saveFormat)
            {
                case Format.png:
                    originImage.Save(filepath, new PngEncoder());
                    break;
                case Format.gif:
                    originImage.Save(filepath, new GifEncoder());
                    break;
                case Format.jpg:
                    originImage.Save(filepath, new JpegEncoder
                    {
                        Quality = 80
                    });
                    break;
            }
        }


        public enum Format
        {
            jpg,
            png,
            gif
        }
        public enum WatermarkPosition
        {
            LeftTop,
            LeftBottom,
            RightTop,
            RigthBottom,
            TopMiddle,
            BottomMiddle,
            Center
        }
    }
}
