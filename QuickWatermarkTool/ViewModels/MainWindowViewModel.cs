using System;
using System.Collections.Generic;
using System.Text;
using QuickWatermarkTool.Models;

namespace QuickWatermarkTool.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";
        public List<Photo> Photos => Photo.PhotoList;
    }
}
