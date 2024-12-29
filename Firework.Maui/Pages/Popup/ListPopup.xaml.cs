using MauiPopup;
using MauiPopup.Views;

namespace Firework.Maui.Pages.Popup;

public partial class ListPopup : BasePopupPage
{
    public ListPopup(List<string> list)
    {
        InitializeComponent();
        
        ItemsSource = new List<string>(list);
        
        MainList.ItemsSource = ItemsSource;
    }
    
    public List<string> ItemsSource { get; }

    /*
    protected override void OnDisappearing()
    {
    }
    */

    protected override bool OnBackButtonPressed()
    {
        PopupAction.ClosePopup();
        
        return false;
    }
}