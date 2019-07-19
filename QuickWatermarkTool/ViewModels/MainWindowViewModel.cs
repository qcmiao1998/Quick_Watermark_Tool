using QuickWatermarkTool.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using QuickWatermarkTool.Views;

namespace QuickWatermarkTool.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Photos = new ObservableCollection<Photo>();
            SelectedSavingFormat = Config.config.DefaultOutputFormat.ToString().ToUpper();
        }

        public ObservableCollection<Photo> Photos { get; set; }

        private string savingPath;
        public string SavingPath
        {
            get => savingPath;
            set => this.RaiseAndSetIfChanged(ref savingPath, value);
        }

        public string[] SavingFormats => Enum.GetNames(typeof(Photo.Format)).Select(i => i.ToUpper()).ToArray();
        public string SelectedSavingFormat { get; set; }

        public void ImportImage()
        {
            _ = Photo.SelectPhotoFiles();
        }

        public void SelectSavingFolder()
        {
            _ = Photo.SelectSavingFolder();
        }

        public void LoadSettingWindow()
        {
            Window settingWindow = new SettingWindow();
            settingWindow.DataContext = new SettingWindowViewModel(settingWindow);
            settingWindow.Show();
        }

        public void Start()
        {
            Thread processThread = new Thread(() =>
            {
                Parallel.ForEach(Photos, photo =>
                {
                    try
                    {
                        if (photo.Status != "Success")
                        {
                            photo.Watermark();
                            photo.AddCopyright();
                            photo.SaveImage();
                        }
                    }
                    catch (Exception e)
                    {
                        photo.Status = e.Message;
#if DEBUG
                        throw e.InnerException;
#endif
                    }
                });
            });
            processThread.Start();
        }

        public void Clear()
        {
            Photos.Clear();
        }

    }
}
