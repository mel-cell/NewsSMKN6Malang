using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Photino.Blazor;

namespace NewsSMKN6Malang
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);

            // register root component and selector
            appBuilder.RootComponents.Add<App>("app");

            // Load configuration
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            
            appBuilder.Services.AddSingleton<IConfiguration>(config);

            appBuilder.Services
                .AddLogging();

            // Register services
            appBuilder.Services.AddSingleton<NewsSMKN6Malang.Data.NewsService>();

            var app = appBuilder.Build();

            // customize window
            app.MainWindow
                .SetTitle("News SMKN 6 Malang")
                .SetUseOsDefaultSize(false)
                .SetSize(1000, 800)
                .SetIconFile("favicon.ico")
                .Load("index.html"); 

            AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
            {
                Console.Error.WriteLine($"Fatal exception: {error.ExceptionObject}");
                app.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString());
            };

            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CRITICAL ERROR: {ex}");
                Console.Error.WriteLine($"CRITICAL ERROR: {ex}");
                app.MainWindow.ShowMessage("Fatal exception", ex.ToString());
            }
        }
    }
}