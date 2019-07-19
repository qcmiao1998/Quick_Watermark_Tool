using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging.Serilog;
using QuickWatermarkTool.Models;
using QuickWatermarkTool.ViewModels;
using QuickWatermarkTool.Views;

namespace QuickWatermarkTool
{
    class Program
    {
        public static Window MainWindow;

        public static MainWindowViewModel MwDataContext;
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) => BuildAvaloniaApp().Start(AppMain, args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseDataGrid()
                .LogToDebug()
                .UseReactiveUI();

        // Your application's entry point. Here you can initialize your MVVM framework, DI
        // container, etc.
        private static void AppMain(Application app, string[] args)
        {
            Config.config = new Config();
            MwDataContext = new MainWindowViewModel();
            MainWindow = new MainWindow
            {
                DataContext = MwDataContext,
            };
            if (Config.config.OpenFileDialogOnStartup)
            {
                _ = Photo.SelectPhotoFiles();
                _ = Photo.SelectSavingFolder();
            }
            app.Run(MainWindow);

        }


    }
}
