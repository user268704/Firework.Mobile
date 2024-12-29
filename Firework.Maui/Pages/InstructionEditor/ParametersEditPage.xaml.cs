using Firework.Maui.ViewModels;

namespace Firework.Maui.Pages.InstructionEditor;

public partial class ParametersEditPage : ContentPage
{
    public ParametersEditPageViewModel ViewModel { get; set; }

    public ParametersEditPage(ParametersEditPageViewModel viewModel)
    {
        InitializeComponent();

        ViewModel = viewModel;
        BindingContext = ViewModel;
    }

    protected override bool OnBackButtonPressed()
    {
        Navigation.PopAsync();

        return base.OnBackButtonPressed();
    }
}