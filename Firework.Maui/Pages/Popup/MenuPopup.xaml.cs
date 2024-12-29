using System.Collections.ObjectModel;
using System.Windows.Input;
using Firework.Mobile.Models.Models.UI;

namespace Firework.Maui.Pages.Popup;

public partial class MenuPopup : CommunityToolkit.Maui.Views.Popup
{
    public ObservableCollection<MenuPopupItem> MenuItems { get; set; }

    public ICommand ItemSelectedCommand { get; set; }

    public MenuPopup(List<MenuPopupItem> menuItems)
    {
        InitializeComponent();  

        MenuCollection.BindingContext = this;

        MenuItems = new ObservableCollection<MenuPopupItem>(menuItems);
        ItemSelectedCommand = new Command<MenuPopupItem>(OnItemSelected);
        
        MenuCollection.ItemsSource = MenuItems;
    }
    
    private void OnItemSelected(MenuPopupItem item)
    {
        Shell.Current.GoToAsync($"//{item.Value}");
        
        Close();
    }
}