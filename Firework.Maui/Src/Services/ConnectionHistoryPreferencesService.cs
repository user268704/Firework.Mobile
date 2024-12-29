using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text.Json;
using Firework.Maui.Settings;
using Firework.Mobile.Abstraction.Services;
using Firework.Mobile.Models.Models.ConnectionsHistory;

namespace Firework.Maui.Services;

public class ConnectionHistoryPreferencesService : IConnectionHistoryService, IDisposable
{
    public ConnectionHistoryPreferencesService()
    {
        HistoryItems = LoadHistoryFromPreferences();

        HistoryItems.CollectionChanged += UpdateHistoryInPreferences;
    }

    public ObservableCollection<HistoryItem> HistoryItems { get; }

    private ObservableCollection<HistoryItem> LoadHistoryFromPreferences()
    {
        var itemsJson = Preferences.Get(PreferencesDefault.HISTORY_CONNECTION, "");

        if (string.IsNullOrEmpty(itemsJson))
            return new();

        var result = JsonSerializer.Deserialize<List<HistoryItem>>(itemsJson);

        return new ObservableCollection<HistoryItem>(result);
    }

    private void UpdateHistoryInPreferences(object? sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
    {
        string jsonHistory = JsonSerializer.Serialize(notifyCollectionChangedEventArgs.NewItems);

        Preferences.Set(PreferencesDefault.HISTORY_CONNECTION, jsonHistory);
    }

    public void Dispose()
    {
        HistoryItems.CollectionChanged -= UpdateHistoryInPreferences;
    }
}