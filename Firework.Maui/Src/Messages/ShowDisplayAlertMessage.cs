using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Firework.Maui.Messages;

public class ShowDisplayAlertMessage : RequestMessage<bool>
{
    public string Message { get; set; }
    public string Title { get; set; }
    public string Ok { get; set; }
    public string Cancel { get; set; }
}