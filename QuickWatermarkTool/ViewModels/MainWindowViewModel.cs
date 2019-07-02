using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
        }

        public ObservableCollection<Photo> Photos { get; set; }

        private string savingPath;
        public string SavingPath
        {
            get => savingPath;
            set => this.RaiseAndSetIfChanged(ref savingPath, value);
        }

        public void ImportImage()
        {
            Photo.SelectPhotoFiles();
        }

        public void SelectSavingFolder()
        {
            Photo.SelectSavingFolder();
        }

    }
}
