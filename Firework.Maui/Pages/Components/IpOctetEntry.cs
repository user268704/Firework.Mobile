using Android.Widget;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Color = Microsoft.Maui.Graphics.Color;

namespace Firework.Maui.Pages.Components;

public class IpOctetEntry : Entry
{
    public IpOctetEntry()
    {
        HandlerChanged += OnHandlerChanged;
    }

    private void OnHandlerChanged(object sender, EventArgs e)
    {
        Color color = Colors.Black;

        if (Application.Current.RequestedTheme == AppTheme.Dark)
        {
            Application.Current.Resources.TryGetValue("EntryTintDarkColor", out var colorObj);

            color = (Color)colorObj;
        }
        else if (Application.Current.RequestedTheme == AppTheme.Light)
        {
            Application.Current.Resources.TryGetValue("EntryTintLightColor", out var colorObj);

            color = (Color)colorObj;
        }


        if (Handler?.PlatformView is EditText editText)
        {
            color.ToRgb(out var colorR, out var colorG, out var colorB);

            // Установка цвета подчеркивания
            editText.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Rgb(colorR, colorG, colorB)); ;
        }
    }
}