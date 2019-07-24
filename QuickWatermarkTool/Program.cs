using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging.Serilog;
using QuickWatermarkTool.Models;
using QuickWatermarkTool.ViewModels;
using QuickWatermarkTool.Views;
using System.IO;
using System.Reflection;
using CommandLine;
using System.Linq;
using Serilog;

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
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Console.WriteLine("Do not close this window!");
 
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location));

            if (args.Length != 0)
            {
                Parser.Default.ParseArguments<CliArgs>(args).WithParsed(arg =>
                {
                    Config.config = !string.IsNullOrEmpty(arg.ConfigFilename) ? new Config(arg.ConfigFilename) : new Config();
                    MwDataContext = new MainWindowViewModel();
                    foreach (var file in arg.PhotoFiles)
                    {
                        if (Photo.Photos.Count(i => i.ImagePath == file) == 0)
                            Photo.Photos.Add(new Photo(file));
                    }
                    if (!string.IsNullOrEmpty(arg.SavingFolder))
                    {
                        Photo.SavingPath = arg.SavingFolder;
                    }
                    MwDataContext.Start();
                });
                return;
            }

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

        class CliArgs
        {
            [Value(0,Required = true,HelpText = "Photo files")]
            public IEnumerable<string> PhotoFiles { get; set; }

            [Option('o',"out",Required = false,HelpText = "Saving folder")]
            public string SavingFolder { get; set; }

            [Option('c', "config", Required = false, HelpText = "Config file")]
            public string ConfigFilename { get; set; }
        }

    }
}
