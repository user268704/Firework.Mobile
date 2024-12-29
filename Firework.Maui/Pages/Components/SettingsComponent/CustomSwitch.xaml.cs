namespace Firework.Maui.Pages.Components.SettingsComponent;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CustomSwitch : ContentView
{
    private bool _isToggled;
    private Color _onColor;
    private Color _thumbColor;
    private string _title;

    public CustomSwitch()
    {
        InitializeComponent();

        GetMainSwitch = MainSwitch;
    }

    public string ElementName { get; set; }

    public bool IsToggled
    {
        get => _isToggled;
        set
        {
            MainSwitch.IsToggled = value;
            _isToggled = value;
        }
    }

    public Switch GetMainSwitch { get; set; }

    public string Title
    {
        get => _title;
        set
        {
            MainLabel.Text = value;
            _title = value;
        }
    }

    public Color ThumbColor
    {
        get => _thumbColor;
        set
        {
            MainSwitch.ThumbColor = value;
            _thumbColor = value;
        }
    }

    public Color OnColor
    {
        get => _onColor;
        set
        {
            MainSwitch.OnColor = value;
            _onColor = value;
        }
    }

    public event EventHandler<ToggledEventArgs> CheckChanged;

    private void MainCheckBox_OnCheckedChanged(object sender, ToggledEventArgs e)
    {
        if (CheckChanged != null) CheckChanged.Invoke(this, e);
    }

    private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
    {
        MainSwitch.IsToggled = !MainSwitch.IsToggled;
    }
}