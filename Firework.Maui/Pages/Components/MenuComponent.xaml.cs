using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using Firework.Maui.Extensions;
using Firework.Maui.Pages.Popup;
using Firework.Mobile.Models.Models.UI;

namespace Firework.Maui.Pages.Components;

public partial class MenuComponent : ContentView
{
    public MenuComponent()
    {
        InitializeComponent();
    }
    
    private void OpenPopupMenu(object sender, EventArgs e)
    {
        var parent = this.GetParentPage();
        
        parent.ShowPopup(new MenuPopup(new List<MenuPopupItem>
        {
            new()
            {
                Title = "Главная страница",
                Alt = null,
                Value = "MainPage"
            },
            new()
            {
                Title = "Настройки",
                Alt = null,
                Value = "Settings"
            },
            new()
            {
                Title = "Эмулятор пользователя",
                Alt = null,
                Value = "Simulator"
            },
            new()
            {
                Title = "Редактор комманд",
                Alt = null,
                Value = "CommandEditor"
            }
        }));
        
        //_popupService.ShowPopup<>();   
    }

}