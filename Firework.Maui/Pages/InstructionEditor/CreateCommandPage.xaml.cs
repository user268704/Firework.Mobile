using Firework.Maui.ViewModels;
using Firework.Mobile.Models.Models.Instructions;

namespace Firework.Maui.Pages.InstructionEditor;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CreateCommandPage : ContentPage
{
    public CommandPageViewModel ViewModel { get; set; }

    public CreateCommandPage(CommandPageViewModel viewModel)
    {
        ViewModel = viewModel;

        NavigationPage.SetHasNavigationBar(this, false);

        BindingContext = ViewModel;
        InitializeComponent();
    }
    
    protected override void OnAppearing()
    {
        var services = ViewModel.GetServices();
        ViewModel.SetServices(services);

        base.OnAppearing();
    }

    private async void ServicePickChange(object? sender, EventArgs e)
    {
        if (ServicePick.SelectedItem == null)
        {
            return;
        }

        Arrow1.IsVisible = true;
        await Arrow1.FadeTo(100, 500, Easing.CubicInOut);

        ActionPick.IsVisible = true;
        ActionPickFrame.IsVisible = true;
        await ActionPick.FadeTo(100, 500, Easing.CubicInOut);
        await ActionPickFrame.FadeTo(100, 500, Easing.CubicInOut);

        await Progress.ProgressTo(0.33, 250, Easing.CubicInOut);

        var serviceInfo = ViewModel
            .Services
            .FirstOrDefault(x => x.Name == ((ServiceInfo)((Picker)sender).SelectedItem)?.Name);

        var actions = serviceInfo.ActionInfo;

        ViewModel.SetActions(actions);

        ActionPick.SelectedItem = null;
        ActionPick.SelectedIndex = -1;
    }

    private async void ActionPickChanged(object? sender, EventArgs e)
    {
        if (!ViewModel.SelectedAction.Parameters.Any())
        {
            return;
        }

        Arrow2.IsVisible = true;
        await Arrow2.FadeTo(100, 500, Easing.CubicInOut);
        
        SelectParametersButton.IsVisible = true;
        await SelectParametersButton.FadeTo(100, 500, Easing.CubicInOut);

        await Progress.ProgressTo(0.66, 250, Easing.CubicInOut);
    }
}