using Firework.Maui.Network;
using Firework.Maui.Pages;
using Firework.Maui.Settings;
using Firework.Maui.ViewModels;
using Firework.Mobile.Abstraction.Network;

namespace Firework.Maui;

public partial class App : Application
{
    private readonly IConnectionService _connectionService;
    private readonly IServiceProvider _serviceProvider;

    public App(IConnectionService connectionService, IServiceProvider serviceProvider)
    {
        _connectionService = connectionService;
        _serviceProvider = serviceProvider;
        InitializeComponent();
        
        MainPage = GetMainPage();
    }


    protected override void OnStart()
    {
        var theme = Preferences.Get(PreferencesDefault.THEME, "light");

        Current.UserAppTheme = Enum.Parse<AppTheme>(theme, true);


        Preferences.Set(PreferencesDefault.DEFAULT_PORT, 5062);
        //Preferences.Set(PreferencesDefault.DEFAULT_TIMEOUT, 10);
        
        
        base.OnStart();
    }

    private Page GetMainPage()
    {
        // TODO: Подумать о том что бы убрать это, loadingpage как будто вообще лишнее
        if (!string.IsNullOrEmpty(Preferences.Get(PreferencesDefault.HOST, String.Empty)) 
            && _connectionService.TryStartConnection())
        {
            var loadViewModel = _serviceProvider.GetRequiredService<LoadingPageViewModel>();

            return new LoadingPage(loadViewModel);
        }

        var connectViewModel = _serviceProvider.GetRequiredService<ConnectionPageViewModel>();

        return new ConnectionPage(_connectionService, connectViewModel);
    }
}