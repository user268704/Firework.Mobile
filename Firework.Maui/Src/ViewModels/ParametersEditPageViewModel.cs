using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.Input;
using Firework.Maui.Settings;
using Firework.Mobile.Models.Models.Instructions;

namespace Firework.Maui.ViewModels;

public sealed partial class ParametersEditPageViewModel
    : ViewModelWithReturnBase<List<ActionParameterInfo>>
{
    private ObservableCollection<ActionParameterInfo> _parameters;
    private ActionInfo _actionInfo;


    public ObservableCollection<ActionParameterInfo> Parameters
    {
        get => _parameters;
        set
        {
            if (Equals(value, _parameters))
                return;
            _parameters = value;

            OnPropertyChanged();
        }
    }

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var actionInfo = query["SelectedAction"] as ActionInfo;

        Parameters = new ObservableCollection<ActionParameterInfo>(actionInfo.Parameters);

        base.ApplyQueryAttributes(query);
    }

    [RelayCommand]
    private async Task Done()
    {
        await GoBackAsync(Parameters.ToList(), "//CommandEditor");
    }
}