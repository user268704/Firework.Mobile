using Firework.Maui.Pages.Components.SettingsComponent;
using Firework.Maui.Settings;

namespace Firework.Maui.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        SetSwitches();
        SetEntries();
    }

    private void SetEntries()
    {
        var endPoint = Preferences.Get("Host", "null");
        EndPoint.SetText(endPoint);
    }

    private void SettingsChanged(object sender, ToggledEventArgs e)
    {
        var handler = new SettingsHandler();
        handler.Start(((CustomSwitch) sender).ElementName, e.Value);
    }

    private void SetSwitches()
    {
        AutoConnect.GetMainSwitch.IsToggled = Preferences.Get("AutoConnect", false);
        if (Preferences.Get("Theme", "dark") == "dark") Theme.GetMainSwitch.IsToggled = true;
    }
}