using Avalonia;
using Avalonia.Markup.Xaml;

namespace QuickWatermarkTool
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
