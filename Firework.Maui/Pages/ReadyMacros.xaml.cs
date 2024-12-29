using System.Collections.ObjectModel;
using System.Text.Json;
using Firework.Mobile.Models.Models.Instructions;

namespace Firework.Maui.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ReadyMacros : ContentPage
{
    public ReadyMacros()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        Instructions = new ObservableCollection<InstructionInfo>();
        foreach (var macro in GetMacros()) Instructions.Add(macro);

        if (!Instructions.Any())
        {
            ListInstructions.IsVisible = false;
            ErrorLabel.Text = "Пусто";
        }
    }

    private ObservableCollection<InstructionInfo> Instructions { get; }

    private List<InstructionInfo> GetMacros()
    {
        var instructions = new List<InstructionInfo>();
        var instrs = Preferences.Get("savedInstructions", String.Empty);

        if (string.IsNullOrEmpty(instrs))
            instructions = JsonSerializer.Deserialize<List<InstructionInfo>>(instrs) ??
                           new List<InstructionInfo>();
        return instructions;
    }

    private void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedValue = e.CurrentSelection;
    }
}