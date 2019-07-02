using System;
using System.Collections.Generic;
using System.Text;
using QuickWatermarkTool.Models;
using ReactiveUI;

namespace QuickWatermarkTool.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IReactiveObject
    {
        public string Greeting => "Welcome to Avalonia!";
        private List<Photo> photos = Photo.PhotoList;
        public List<Photo> Photos
        {
            get => photos;
            set => this.RaiseAndSetIfChanged(ref photos,value);
        }

    }
}
