using System.Globalization;
using System.Text.Json;
//using Firework.Abstraction.Instruction;
using Firework.Maui.Network;
using Firework.Maui.Settings;
using Firework.Mobile.Abstraction.Instructions;
using Firework.Mobile.Abstraction.Network;
using Firework.Mobile.Models.Models.Instructions;
using Firework.Mobile.Models.Models.Results;

namespace Firework.Maui.Pages;

public partial class MainPage : ContentPage
{
    private readonly IConnectionService _connectionService;
    private readonly IInstructionService _instructionService;
    private bool _isInfoSet;
    private Timer _timer;

    public MainPage(IConnectionService connectionService, IInstructionService instructionService)
    {
        _connectionService = connectionService;
        _instructionService = instructionService;

        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }
    
    protected override async void OnAppearing()
    {
        SetHistory();

        try
        {
            var instruction = _instructionService.CreateInstruction("os", "username");
            var userName = await _connectionService.SendCommandAsync(new List<InstructionInfo>()
            {
                instruction
            });

            UserNameLabel.Text = UserNameClipping(userName.First().Value);

            var timerCallback = new TimerCallback(InfoUpdateCallback);
            _timer = new Timer(timerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    protected override void OnDisappearing()
    {
        if (_timer != null)
        {
            _timer.Dispose();
        }
    }

    private async void InfoUpdateCallback(object state)
    {
        var instruction = _instructionService.CreateInstruction("os", "getinfo");

        var result = await _connectionService.SendCommandAsync(new List<InstructionInfo>
        {
            instruction
        });
        
        Update(result.First().Value);
    }

    private void HotButtonHandler(object sender, EventArgs e)
    {
    }

    private void FastCommandHandler(object sender, EventArgs eventArgs)
    {
    }

    private void SetHistory()
    {
        if (!Preferences.ContainsKey(PreferencesDefault.SAVED_INSTRUCTIONS))
            return;

        var instructions =
            JsonSerializer.Deserialize<List<InstructionInfo>>(Preferences.Get(PreferencesDefault.SAVED_INSTRUCTIONS,
                String.Empty));

        if (instructions == null)
        {
            HistoryUsingInstructions.Children.Add(new Label
            {
                Text = "Список истории пуст"
            });

            return;
        }


        for (var index = 0; index < (instructions.Count > 5 ? 5 : instructions.Count); index++)
        {
            var instruction = instructions[index];
            var newElem = new Button
            {
                Text = instruction.Title
            };
            newElem.Clicked += FastCommandHandler;
            HistoryUsingInstructions.Children.Add(newElem);
        }
    }
    
    private void Update(string message)
    {
        var info = GetInfo(message);
        
        async void UpdateUi()
        {
            await UpdateProgressBars(info.CpuLoad, info.GpuLoad, info.RamLoad);
        }

        MainThread.BeginInvokeOnMainThread(UpdateUi);
        if (!_isInfoSet)
        {
            AnimatedSetVisibleContainers();
            _isInfoSet = true;
        }
    }

    private string UserNameClipping(string name)
    {
        if (name.Length > 20)
            return name.Substring(0, 18) + "...";

        return name;
    }

    private async Task UpdateProgressBars(float cpu, float gpu, float ram)
    {
        await CpuProgress.ProgressTo(cpu / 100, 1000, Easing.CubicInOut);
        PercentageCpu.Text = Math.Round(cpu, 0) + "%";

        await GpuProgress.ProgressTo(gpu / 100, 1000, Easing.CubicInOut);
        PercentageGpu.Text = Math.Round(gpu, 0) + "%";

        await RamProgress.ProgressTo(ram / 100, 1000, Easing.CubicInOut);
        PercentageRam.Text = Math.Round(ram, 0) + "%";
    }

    private GetLoadResult GetInfo(string updateMessage)
    {
        var message = updateMessage;
        var result = new GetLoadResult();

        var computedLoaded = message
            .Substring(message.IndexOf('(') + 1, message.IndexOf(')') - 1)
            .Split(';', StringSplitOptions.RemoveEmptyEntries);

        // BUG: Если десктоп и мобилка запускаются на устройствах с разной культурой, может быть exception
        result.CpuLoad = Convert.ToSingle(computedLoaded.First().Split(':').Last(), new CultureInfo(CultureInfo.CurrentCulture.Name));
        result.GpuLoad = Convert.ToSingle(computedLoaded[1].Split(':').Last(), new CultureInfo(CultureInfo.CurrentCulture.Name));
        result.RamLoad = Convert.ToSingle(computedLoaded.Last().Split(':').Last(), new CultureInfo(CultureInfo.CurrentCulture.Name));

        return result;
    }

    private void AnimatedSetVisibleContainers()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            MainContainer.IsVisible = true;
            MainContainer.FadeTo(1, 500, Easing.Linear);
        });
    } 
}