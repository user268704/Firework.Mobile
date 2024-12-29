using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firework.Mobile.Abstraction.Instructions;
using Firework.Mobile.Abstraction.Network;
using Firework.Mobile.Models.Models.Instructions;

namespace Firework.Maui.ViewModels;

public partial class CommandPageViewModel : ObservableObject
{
    private readonly IInstructionService _instructionService;
    private readonly IConnectionService _connectionService;
    public ObservableCollection<ServiceInfo> Services { get; set; }
    public ObservableCollection<ActionInfo> Actions { get; set; }
    public ObservableCollection<ActionParameterInfo> Parameters { get; set; }

    public ServiceInfo SelectedService { get; set; }
    public ActionInfo SelectedAction { get; set; }

    public CommandPageViewModel(IInstructionService instructionService, IConnectionService connectionService)
    {
        _instructionService = instructionService;
        _connectionService = connectionService;

        Services = new ObservableCollection<ServiceInfo>();
        Actions = new ObservableCollection<ActionInfo>();
        Parameters = new ObservableCollection<ActionParameterInfo>();
    }

    private void SetSelected()
    {
        /*
        var selectedServiceJson = Preferences.Get(PreferencesDefault.SELECTED_SERVICE_INFO, "");

        if (!string.IsNullOrEmpty(selectedServiceJson))
        {
            var service = JsonSerializer.Deserialize<ServiceInfo>(selectedServiceJson);
            SelectedService = service;
        }

        var selectedActionJson = Preferences.Get(PreferencesDefault.SELECTED_ACTION_INFO, "");

        if (string.IsNullOrEmpty(selectedActionJson))
        {
            var action = JsonSerializer.Deserialize<ActionInfo>(selectedActionJson);
            SelectedAction = action;
        }
    */
    }

    public List<ServiceInfo> GetServices()
    {
        var instruction = _instructionService.CreateInstruction("app>getservices()");

        var resultJson = _connectionService.SendCommand(new() {instruction});
        var info = JsonSerializer.Deserialize<List<ServiceInfo>>(resultJson.First().Value);

        return info;
    }


    [RelayCommand]
    private async Task SelectParameter()
    {
        await ParametersEditPageViewModel.GoToAndReturn(this, response =>
        {
            foreach (ActionParameterInfo parameterInfo in response)
            {
                Parameters.Add(parameterInfo);
            }
        }, "//ParametersEditor", new Dictionary<string, object>
        {
            { "SelectedAction", SelectedAction }
        });
    }


    [RelayCommand]
    private void Done()
    {
        var instruction = _instructionService.CreateInstruction(SelectedService.Name,
            SelectedAction.Name, Parameters.ToList());

        _connectionService.SendCommand(new() {instruction});
    }

    public void SetServices(List<ServiceInfo> services)
    {
        Services.Clear();
        services.ForEach(service => Services.Add(service));

        SetSelected();
    }

    public void SetActions(List<ActionInfo> actions)
    {
        Actions.Clear();
        actions.ForEach(action => Actions.Add(action));
    }
}