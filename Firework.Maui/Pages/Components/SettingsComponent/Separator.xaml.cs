namespace Firework.Maui.Pages.Components.SettingsComponent;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Separator : ContentView
{
    private Color _color;
    private string _fontFamily;
    private string _title;

    public Separator()
    {
        InitializeComponent();
    }

    public string Title
    {
        set
        {
            TitleLabel.Text = value;
            _title = value;
        }
    }

    public string FontFamily
    {
        set
        {
            TitleLabel.FontFamily = value;
            _fontFamily = value;
        }
    }

    public Color Color
    {
        set
        {
            SeparatorBox.Color = value;
            _color = value;
        }
    }
}