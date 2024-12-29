namespace Firework.Maui.Pages.Components.SettingsComponent;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SettingsEntry : ContentView, IDisposable
{
    private string _description;
    private bool _isReadOnly;

    public SettingsEntry()
    {
        InitializeComponent();
        if (OnTextChanged != null) MainEntry.TextChanged += OnTextChanged.Invoke;
        if (OnEntryClick != null) TapGestureRecognizerEntry.Tapped += OnEntryClick.Invoke;
    }

    public string EntryText { get; set; }

    public bool IsReadOnly
    {
        get => _isReadOnly;
        set
        {
            MainEntry.IsEnabled = value;
            _isReadOnly = value;
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            DescriptionLabel.Text = value;
            _description = value;
        }
    }

    public void Dispose()
    {
        if (OnTextChanged != null) MainEntry.TextChanged -= OnTextChanged.Invoke;
        if (OnEntryClick != null) TapGestureRecognizerEntry.Tapped -= OnEntryClick.Invoke;
    }

    public event EventHandler OnEntryClick;
    public event EventHandler<TextChangedEventArgs> OnTextChanged;

    public void SetDescription(string message)
    {
        DescriptionLabel.Text = message;
    }

    public void SetText(string message)
    {
        MainEntry.Text = message;
    }

    public void SetEntryEnabled(bool isEnable)
    {
        MainEntry.IsEnabled = isEnable;
        if (!isEnable)
            if (OnEntryClick != null)
                TapGestureRecognizerEntry.Tapped -= OnEntryClick.Invoke;
    }
}