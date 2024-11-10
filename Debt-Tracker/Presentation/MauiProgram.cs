using Application.Services;
using Core.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Presentation.ViewModels;
using Presentation.Views;

namespace Presentation
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
                });

            builder.Logging.AddDebug();

            // Services
            builder.Services.AddSingleton<IMongoDbService, MongoDbService>();
            builder.Services.AddTransient<IDebtService, DebtService>();

            // Repositories
            builder.Services.AddTransient<IDebtRepository, DebtRepository>();

            // ViewModels
            builder.Services.AddTransient<DebtViewModel>();
            builder.Services.AddTransient<AddDebtViewModel>();
            builder.Services.AddTransient<EditDebtViewModel>();

            // Pages
            builder.Services.AddTransient<DebtPage>();
            builder.Services.AddTransient<AddDebtPage>();

            return builder.Build();
        }
    }
}
