using CommunityToolkit.Maui;
using Firework.Maui.Network;
using Firework.Maui.Pages;
using Firework.Maui.Pages.InstructionEditor;
using Firework.Maui.Services;
using Firework.Maui.ViewModels;
using Firework.Mobile.Abstraction.Instructions;
using Firework.Mobile.Abstraction.Network;
using Firework.Mobile.Abstraction.Services;
using Microsoft.Extensions.Logging;
using UraniumUI;

namespace Firework.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseUraniumUI()
            .UseUraniumUIMaterial()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        //builder.Services.AddSingleton<IConnectionService, HttpConnectionService>();
        builder.Services.AddSingleton<IConnectionService, SignalClientService>();
        builder.Services.AddScoped<IInstructionService, InstructionService>();
        builder.Services.AddSingleton<IConnectionHistoryService, ConnectionHistoryPreferencesService>();

        //builder.Services.AddSingleton<IInstructionService, InstructionService>();

        builder.Services.AddSingleton<CommandPageViewModel>();
        builder.Services.AddSingleton<ConnectionPageViewModel>();
        builder.Services.AddSingleton<LoadingPageViewModel>();
        builder.Services.AddSingleton<ParametersEditPageViewModel>();

        builder.Services.AddTransient<LoadingPage>();  
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<CreateCommandPage>();
        builder.Services.AddTransient<ParametersEditPage>();

        return builder.Build();
    }
}