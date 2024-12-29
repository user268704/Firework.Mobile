using System.Collections.ObjectModel;
using Firework.Mobile.Models.Models.ConnectionsHistory;

namespace Firework.Mobile.Abstraction.Services;

public interface IConnectionHistoryService
{
    public ObservableCollection<HistoryItem> HistoryItems { get; }
}