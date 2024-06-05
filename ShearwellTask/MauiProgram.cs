using Microsoft.Extensions.Logging;

namespace ShearwellTask
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("calibri.ttf", "Calibri");
                });

            builder.Services.AddSingleton<MainPage>();
       

            builder.Services.AddTransient<GroupSummaryPage>();
            builder.Services.AddTransient<GroupSummaryViewModel>();

            builder.Services.AddTransient<GroupDetailsPage>();
            builder.Services.AddTransient<GroupDetailsViewModel>();

            builder.Services.AddTransient<AddPage>();
            builder.Services.AddTransient<AddViewModel>();

            string db_path = Path.Combine(FileSystem.AppDataDirectory, "Animals.db3");
            builder.Services.AddSingleton<AnimalsDatabase>(s => ActivatorUtilities.CreateInstance<AnimalsDatabase>(s, db_path));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
