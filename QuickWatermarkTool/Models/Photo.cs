using Avalonia.Controls;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.MetaData.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ReactiveUI;
using Image = SixLabors.ImageSharp.Image;

namespace QuickWatermarkTool.Models
{
    public class Photo : ReactiveObject
    {
        public static ObservableCollection<Photo> Photos => Program.MwDataContext.Photos;

        public static string SavingPath
        {
            get => Program.MwDataContext.SavingPath;
            set => Program.MwDataContext.SavingPath = value;
        }

        public static Format SavingFormat => (Format)Enum.Parse(typeof(Format),Program.MwDataContext.SelectedSavingFormat);

        private Image<Rgba32> originImage;
        private Image<Rgba32> watermarkImage;
        public string ImagePath;

        public string FileName { get; }
        private string status;

        public string Status
        {
            get => status;
            set => this.RaiseAndSetIfChanged(ref status, value);
        }

        public int Width => originImage.Width;

        public int Height => originImage.Height;

        private int WmHeight => watermarkImage.Height;
        private int WmWidth => watermarkImage.Width;

        public int FrameCount => originImage.Frames.Count;

        public Photo(string path)
        {
            this.ImagePath = path;
            FileName = Path.GetFileName(path);
            Status = "Loaded";
        }

        public void Watermark()
        {
            Status = "Starting";

            originImage = Image.Load(ImagePath);
            watermarkImage = Image.Load(Config.config.WatermarkFilename);

            if (Width > Config.config.MaxOutputImageWidth || Height > Config.config.MaxOutputImageHeight)
                ResizePic(originImage, Config.config.MaxOutputImageWidth, Config.config.MaxOutputImageHeight);

            int wmPosiW, wmPosiH;
            int maxWmWidth = (int)Math.Floor(Config.config.MaxWatermarkScaleWidth * Width);
            int maxWmHeight = (int)Math.Floor(Config.config.MaxWatermarkScaleHeight * Height);

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

        public void SaveImage()
        {
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(ImagePath);
            string saveName = fileNameWithoutExt + Config.config.OutputSuffix + "." + Enum.GetName(typeof(Format),SavingFormat);
            string filepath = Path.Combine(SavingPath, saveName);
            switch (SavingFormat)
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
            originImage.Dispose();
            watermarkImage.Dispose();
            Status = "Success";
        }

        public static async void SelectPhotoFiles()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Select Photos",
                AllowMultiple = true
            };
            FileDialogFilter imageFilter = new FileDialogFilter();
            imageFilter.Extensions.AddRange(new[] { "jpg", "png", "tif" });
            imageFilter.Name = "Images";
            dialog.Filters.Add(imageFilter);
            string[] files = await dialog.ShowAsync(Program.MainWindow);
            foreach (var file in files)
            {
                if(Photos.Count(i => i.ImagePath == file) == 0)
                    Photos.Add(new Photo(file));
            }
        }

        public static async void SelectSavingFolder()
        {
            OpenFolderDialog ofd = new OpenFolderDialog
            {
                Title = "Select Saving Folder"
            };
            string folder = await ofd.ShowAsync(Program.MainWindow);
            SavingPath = folder;
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
