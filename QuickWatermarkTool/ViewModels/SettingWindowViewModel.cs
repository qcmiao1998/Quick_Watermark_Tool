using QuickWatermarkTool.Models;
using ReactiveUI;
using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace QuickWatermarkTool.ViewModels
{
    class SettingWindowViewModel : ViewModelBase
    {
        private Config config;
        private Window window;

        public SettingWindowViewModel(Window window)
        {
            config = Config.config;
            this.window = window;
            _watermarkFilename = config.WatermarkFilename;
            _watermarkPosition = config.WatermarkPosition.ToString();
            _defaultOutputFormat = config.DefaultOutputFormat.ToString().ToUpper();
        }
        public int MaxOutputImageWidth
        {
            get => config.MaxOutputImageWidth;
            set => config.MaxOutputImageWidth = value;
        }

        public int MaxOutputImageHeight
        {
            get => config.MaxOutputImageHeight;
            set => config.MaxOutputImageHeight = value;
        }

        public void ChooseWatermarkFile()
        {
            _ = ChooseWatermarkFileAsync();
        }

        private async Task ChooseWatermarkFileAsync()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Select Watermark Photo",
                AllowMultiple = false
            };
            FileDialogFilter imageFilter = new FileDialogFilter();
            imageFilter.Extensions.AddRange(new[] { "jpg", "jpeg", "png", "gif" });
            imageFilter.Name = "Image";
            ofd.Filters.Add(imageFilter);
            string result = (await ofd.ShowAsync(window)).First();
            if (!string.IsNullOrEmpty(result))
                WatermarkFilename = result;
        }

        private string _watermarkFilename;
        public string WatermarkFilename
        {
            get => _watermarkFilename;
            set
            {
                config.WatermarkFilename = value;
                this.RaiseAndSetIfChanged(ref _watermarkFilename, value);
            }
        }

        public float MaxWatermarkScaleWidth
        {
            get => config.MaxWatermarkScaleWidth;
            set => config.MaxWatermarkScaleWidth = value;
        }

        public float MaxWatermarkScaleHeight
        {
            get => config.MaxWatermarkScaleHeight;
            set => config.MaxWatermarkScaleHeight = value;
        }

        public float WatermarkOpacity
        {
            get => config.WatermarkOpacity;
            set => config.WatermarkOpacity = value;
        }

        public int WatermarkOffsetWidth
        {
            get => config.WatermarkOffsetWidth;
            set => config.WatermarkOffsetWidth = value;
        }

        public int WatermarkOffsetHeight
        {
            get => config.WatermarkOffsetHeight;
            set => config.WatermarkOffsetHeight = value;
        }

        public string[] SavingFormats => Enum.GetNames(typeof(Photo.Format)).Select(i => i.ToUpper()).ToArray();
        private string _defaultOutputFormat;
        public string DefaultOutputFormat
        {
            get => _defaultOutputFormat;
            set
            {
                this.RaiseAndSetIfChanged(ref _defaultOutputFormat, value);
                config.DefaultOutputFormat = Enum.Parse<Photo.Format>(value, true);
            }
        }

        public bool OpenFileDialogOnStartup
        {
            get => config.OpenFileDialogOnStartup;
            set => config.OpenFileDialogOnStartup = value;
        }

        public string AuthorName
        {
            get => config.AuthorName;
            set => config.AuthorName = value;
        }

        public string Copyright
        {
            get => config.Copyright;
            set => config.Copyright = value;
        }

        public string[] WatermarkPositions => Enum.GetNames(typeof(Photo.WatermarkPosition)).ToArray();
        private string _watermarkPosition;
        public string WatermarkPosition
        {
            get => _watermarkPosition;
            set
            {
                this.RaiseAndSetIfChanged(ref _watermarkPosition, value);
                config.WatermarkPosition = Enum.Parse<Photo.WatermarkPosition>(value, true);
            }
        }

        public string OutputSuffix
        {
            get => config.OutputSuffix;
            set => config.OutputSuffix = value;
        }
    }
}
