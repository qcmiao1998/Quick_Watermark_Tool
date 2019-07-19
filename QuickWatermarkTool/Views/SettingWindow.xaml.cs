using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace QuickWatermarkTool.Views
{
    public class SettingWindow : Window
    {
        public SettingWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
