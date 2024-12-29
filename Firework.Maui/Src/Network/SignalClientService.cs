using System.Net.NetworkInformation;
using System.Net.Sockets;
using Firework.Maui.Settings;
using Firework.Mobile.Abstraction.Network;
using Firework.Mobile.Abstraction.Services;
using Firework.Mobile.Models.Models.ConnectionsHistory;
using Firework.Mobile.Models.Models.Instructions;
using Firework.Mobile.Models.Models.Results;
using Microsoft.AspNetCore.SignalR.Client;

namespace Firework.Maui.Network;

public class SignalClientService : IConnectionService
{
    private readonly IConnectionHistoryService _connectionHistoryService;
    private static HubConnection? _hubConnection;

    private HubConnection Connection
    {
        get => _hubConnection;
        set => _hubConnection = value;
    }

    public SignalClientService(IConnectionHistoryService connectionHistoryService)
    {
        _connectionHistoryService = connectionHistoryService;
    }

    public bool TryStartConnection()
    {
        string host = Preferences.Get(PreferencesDefault.HOST, "");

        string hostUrl = $"http://{host}/signal";

        if (string.IsNullOrEmpty(host) || !Uri.TryCreate(hostUrl, UriKind.Absolute, out _))
            return false;


        var initResult = Initialize(hostUrl);

        if (!initResult)
            return false;

        return true;
    }


    private HubConnection? CreateConnection(string url)
    {
        try
        {
            return new HubConnectionBuilder()
                .WithUrl(url)
                .WithServerTimeout(TimeSpan.FromSeconds(5))
                .WithAutomaticReconnect()
                .Build();
        }
        catch (Exception e)
        {
            return null;
        }
    }

    private void RegisterEndPoints()
    {
        Connection.On("GetInfo", [typeof(Handshake)], parameters =>
        {
            var handshake = (Handshake)parameters.First();

            _connectionHistoryService.HistoryItems.Add(new HistoryItem
            {
                DateLastConnected = DateTime.Now,
                Id = Guid.NewGuid(),
                HostName = handshake.DeviceName,
                EndPoint = handshake.EndPoint,
            });

            return Task.FromResult(new GetInfoResult
            {
                Ip = GetLocalIpAddress(),
                Os = $"{DeviceInfo.Platform.ToString()} ({DeviceInfo.Version})",
                DeviceName = DeviceInfo.Name
            });
        });
    }

    private static string GetLocalIpAddress()
    {
        foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (networkInterface.OperationalStatus == OperationalStatus.Up)
            {
                var ipProperties = networkInterface.GetIPProperties();
                foreach (var address in ipProperties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return address.Address.ToString();
                    }
                }
            }
        }
        return "Неизвестен";
    }

    private bool Initialize(string host)
    {
        try
        {
            var connection = CreateConnection(host);

            if (connection == null)
                return false;

            Connection = connection;

            RegisterEndPoints();
            Connection.StartAsync();

            if (Connection.State is HubConnectionState.Disconnected)
            {
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool IsConnected()
    {
        return Connection?.State == HubConnectionState.Connected;
    }

    public async Task<List<InstructionResult>> SendCommandAsync(List<InstructionInfo> commands)
    {
        var result = await Connection.InvokeAsync<List<InstructionResult>>("Command", commands);

        return result;
    }

    public List<InstructionResult> SendCommand(List<InstructionInfo> commands)
    {
        var result = Connection.InvokeAsync<List<InstructionResult>>("Command", commands).Result;

        return result;
    }

    public void RestartConnection()
    {
        Dispose();

        _hubConnection = null;

        TryStartConnection();

        Connection.StartAsync();
    }

    public void ClearConnection()
    {
        Dispose();

        _hubConnection = null;
    }


    public void Dispose()
    {
        Connection?.DisposeAsync();
    }
}