namespace Firework.Maui.Settings;

public class SettingsHandler
{
    public void Start(string settingsName, bool newValue)
    {
        switch (settingsName)
        {
            case "theme":
                ThemeChanged(newValue);
                return;
            case "autoConnect":
                AutoConnect(newValue);
                return;
        }
    }

    /// <param name="e">
    ///     true - темная тема
    ///     false - светлая тема
    /// </param>
    private void ThemeChanged(bool newValue)
    {
        if (newValue)
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
            Preferences.Set("Theme", "dark");
        }
        else
        {
            Application.Current.UserAppTheme = AppTheme.Light;
            Preferences.Set("Theme", "light");
        }
    }

    private void AutoConnect(bool newValue)
    {
        Preferences.Set("AutoConnect", newValue);
    }
}