using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace QuickWatermarkTool.Models
{
    class Photo
    {
        private Image<Rgba32> originImage;
        private Image<Rgba32> watermarkImage;

        public string FileName { get; }

        public int Width => originImage.Width;

        public int Height => originImage.Height;

        public int FrameCount => originImage.Frames.Count;

        public Photo(string path, string watermarkpath)
        {
            originImage = Image.Load(path);
            watermarkImage = Image.Load(watermarkpath);
            Path.GetFileNameWithoutExtension(path);
        }

        public void Watermark(WatermarkPosition position = WatermarkPosition.LeftBottom, int width = 50, int height = 50, float opacity = 1f)
        {
            int wmPosiX, wmPosiY;

            int maxWmNew = 450;
            width = width * Width / 2000;
            height = height * Height / 2000;
            maxWmNew = Width * maxWmNew / 2000;
            ResizePic(watermarkImage, maxWmNew, maxWmNew);

            switch (position)
            {
                case WatermarkPosition.LeftBottom:
                    wmPosiX = width;
                    wmPosiY = originImage.Height - watermarkImage.Height - height;
                    break;
                default:
                    throw new NotImplementedException();
            }
            originImage.Mutate(i => { i.DrawImage(watermarkImage, opacity, new Point(wmPosiX, wmPosiY)); });
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
