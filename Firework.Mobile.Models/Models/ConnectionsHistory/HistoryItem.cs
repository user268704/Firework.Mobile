namespace Firework.Mobile.Models.Models.ConnectionsHistory;

public class HistoryItem
{
    public Guid Id { get; set; }
    public string HostName { get; set; }
    public DateTime DateLastConnected { get; set; }
    public string EndPoint { get; set; }
}