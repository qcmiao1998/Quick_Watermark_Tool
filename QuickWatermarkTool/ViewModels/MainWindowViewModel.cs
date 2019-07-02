using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using QuickWatermarkTool.Models;
using ReactiveUI;
using ReactiveUI.Legacy;

namespace QuickWatermarkTool.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IReactiveObject
    {
        public MainWindowViewModel()
        {
            Photos = new ObservableCollection<Photo>();
            SelectedSavingFormat = Config.config.DefaultOutputformat.ToString();
        }

        public ObservableCollection<Photo> Photos { get; set; }

        private string savingPath;
        public string SavingPath
        {
            get => savingPath;
            set => this.RaiseAndSetIfChanged(ref savingPath, value);
        }

        public string[] SavingFormats => Enum.GetNames(typeof(Photo.Format));
        public string SelectedSavingFormat { get; set; }

        public void ImportImage()
        {
            Photo.SelectPhotoFiles();
        }

        public void SelectSavingFolder()
        {
            Photo.SelectSavingFolder();
        }

        public void Start()
        {
            Parallel.ForEach(Photos, photo =>
            {
                try
                {
                    photo.Watermark();
                    photo.AddCopyright();
                    photo.SaveImage();
                }
                catch (Exception e)
                {
                    photo.Status = e.Message;
                }
            });
        }

    }
}
