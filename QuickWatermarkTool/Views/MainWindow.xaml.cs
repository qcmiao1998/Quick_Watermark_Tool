using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using QuickWatermarkTool.Models;

namespace QuickWatermarkTool.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            if (Config.config.OpenFiledialogOnStartup)
            {
                SelectPhotoFiles();
            }
        }

        public async void SelectPhotoFiles()
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
            string[] files = await dialog.ShowAsync(this);
        }

        public async void SelectSavingFolder()
        {
            OpenFolderDialog ofd = new OpenFolderDialog
            {
                Title = "Select Saving Folder"
            };
            string folder = await ofd.ShowAsync(this);

        }
    }
}
