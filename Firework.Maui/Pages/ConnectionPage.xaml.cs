using Firework.Maui.ViewModels;
using Firework.Mobile.Abstraction.Network;
using Firework.Mobile.Models.Models.ConnectionsHistory;

namespace Firework.Maui.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ConnectionPage : ContentPage
{
    private readonly List<Entry> _entries;
    private bool _doneSync;

    public ConnectionPageViewModel ViewModel { get; set; }

    public ConnectionPage(IConnectionService connectionService, ConnectionPageViewModel viewModel)
    {
        InitializeComponent();

        NavigationPage.SetHasNavigationBar(this, false);

        ViewModel = viewModel;
        BindingContext = ViewModel;

        _entries =
        [
            FirstOctet,
            SecondOctet,
            ThirdOctet,
            FourthOctet
        ];
    }

    private void Octets_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (int.TryParse(e.NewTextValue, out var numberInOctet) && numberInOctet > 255)
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
            return;
        }

        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            var entry = _entries.FindIndex(x => x.Id == ((Entry)sender).Id);
            if (entry - 1 >= 0 && entry - 1 < _entries.Count) _entries[entry - 1].Focus();
            
            return;
        }

        if (e.NewTextValue.Length is 3)
        {
            var entry = _entries.FindIndex(x => x.Id == ((Entry)sender).Id);
            if (entry + 1 >= 0 && entry + 1 < _entries.Count)
                _entries[entry + 1].Focus();
        }
    }

    /*
    private void DoneButton_OnClick(object sender, EventArgs e)
    {
        var host = ViewModel.Ip.ToString();
        if (string.IsNullOrEmpty(host))
            return;
        
        ViewModel.Done(host);
    }
    */

    public void CompletedHandler(object? sender, EventArgs e)
    {

    }

    public void SelectHistoryItem(object? sender, SelectionChangedEventArgs e)
    {
        var selectedObject = e.CurrentSelection as List<object>;

        ViewModel.SelectHistoryItem(((HistoryItem)selectedObject.First()).Id);
    }


    private void QRConnection_OnClicked(object? sender, EventArgs e)
    {
            
    }
}