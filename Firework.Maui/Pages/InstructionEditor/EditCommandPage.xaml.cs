using System.Text.Json;
using Firework.Maui.Dependency;
using Firework.Mobile.Models.Models.Instructions;

namespace Firework.Maui.Pages.InstructionEditor;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EditCommandPage : ContentPage
{
    private readonly InstructionInfo _instruction;

    public EditCommandPage(InstructionInfo instruction)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        _instruction = instruction;
        SetParts();
    }

    private void SetParts()
    {
        RootLabel.Text += _instruction.ServiceName;
        ServiceLabel.Text += _instruction.ActionInfo.Name;

        if (!_instruction.Parameters.Any())
        {
            ParamsStack.IsVisible = false;

            return;
        }

        List<string> parameters = new();
        foreach (var parameter in _instruction.Parameters) parameters.Add(parameter.Value);

        ParameterLabel.Text += string.Join(", ", parameters);
    }


    private async void Save_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleCommandEntry.Text))
        {
            DependencyService.Get<IToast>().ShortMessage("Имя не задано");
            NameFrame.BorderColor = Color.FromRgb(255, 0, 0);

            return;
        }

        await ContentContainer.FadeTo(0, easing: Easing.Linear);
        ContentContainer.IsVisible = false;
        Indicator.IsVisible = true;
        Indicator.IsRunning = true;

        if (AddToMainPageSwitch.GetMainSwitch.IsToggled)
        {
            // TODO: Добавлять на главную
        }

        await Task.Run(() =>
        {
            var savedInstr = new InstructionInfo
            {
                Title = TitleCommandEntry.Text,
                Description = DescriptionCommandEntry.Text
            };

            var instructions = new List<InstructionInfo>();
            
            
            var savedInstructions = Preferences.Get("savedInstructions", String.Empty);
            
            instructions = JsonSerializer.Deserialize<List<InstructionInfo>>(savedInstructions) ??
                           new List<InstructionInfo>();

            instructions.Add(savedInstr);
            Preferences.Set("savedInstructions", JsonSerializer.Serialize(instructions));
            Task.Delay(1000);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                DependencyService.Get<IToast>().ShortMessage("Сохранено");
                Navigation.PopAsync();
            });
        });
    }

    private void OnParams_Click(object sender, EventArgs e)
    {
        void ParamsChangedEntryUnfocused(object o, FocusEventArgs focusEventArgs)
        {
            ParameterLabel.Text = ParamsChangedEntry.Text;
            ParameterLabel.IsVisible = true;
            ParamsChangedEntry.IsVisible = false;

            ParamsChangedEntry.Unfocused -= ParamsChangedEntryUnfocused;
        }

        ParamsChangedEntry.Unfocused += ParamsChangedEntryUnfocused;
        ParameterLabel.IsVisible = false;
        ParamsChangedEntry.IsVisible = true;
    }
}