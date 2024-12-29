using System.Collections.ObjectModel;
using System.Net;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firework.Maui.Settings;
using Firework.Mobile.Abstraction.Network;
using Firework.Mobile.Abstraction.Services;
using Firework.Mobile.Models.Models;
using Firework.Mobile.Models.Models.ConnectionsHistory;

namespace Firework.Maui.ViewModels;

public partial class ConnectionPageViewModel : ObservableObject
{
    private readonly IConnectionHistoryService _historyService;
    private readonly IConnectionService _connectionService;
    public IP Ip { get; set; }

    public ObservableCollection<HistoryItem> HistoryItems { get; set; }

    private bool _doneSync;

    public ConnectionPageViewModel(IConnectionHistoryService historyService, IConnectionService connectionService)
    {
        _historyService = historyService;
        _connectionService = connectionService;

        Ip = new IP();
        HistoryItems = _historyService.HistoryItems;
    }

    [RelayCommand]
    private void RemoveHistoryItem(Guid id)
    {
        HistoryItems.Remove(HistoryItems.Single(h => h.Id == id));
    }

    public void SelectHistoryItem(Guid id)
    {
        var item = HistoryItems.Single(h => h.Id == id);

        Done(IPEndPoint.Parse(item.EndPoint));
    }

    [RelayCommand]
    private void Done()
    {
        int port = Preferences.Get(PreferencesDefault.DEFAULT_PORT, 0);

        var ipEndPoint = IPEndPoint.Parse(Ip.ToString());
        ipEndPoint.Port = port;

        Done(ipEndPoint);
    }

    private void Done(IPEndPoint host)
    {
        Task.Factory.StartNew(() =>
        {
            try
            {

                if (_doneSync)
                    return;

                _doneSync = true;

                /*
                if (host.Split(':', StringSplitOptions.RemoveEmptyEntries).Length == 1)
                    host += ":" + Preferences.Get(PreferencesDefault.DEFAULT_PORT, 5062);
                    */

                var h = host.ToString();

                Preferences.Set(PreferencesDefault.HOST, host.ToString());
                _connectionService.ClearConnection();

                if (_connectionService.TryStartConnection())
                {
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Preferences.Set(PreferencesDefault.HOST, host.ToString());
                        Application.Current.MainPage = new AppShell();
                    });

                    return;
                }

                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    var darkThemeColor = (Color)Application.Current.Resources["DarkSecondColor"];
                    var lightThemeColor = (Color)Application.Current.Resources["LightSecondColor"];

                    var darkThemeTextColor = (Color)Application.Current.Resources["DarkMainTextColor"];
                    var lightThemeTextColor = (Color)Application.Current.Resources["LightMainTextColor"];

                    Snackbar.Make("Не удалось подключиться, введите или выберите другой хост",
                        visualOptions: new SnackbarOptions()
                        {
                            BackgroundColor = Application.Current.UserAppTheme == AppTheme.Light
                                ? lightThemeColor
                                : darkThemeColor,
                            TextColor = Application.Current.UserAppTheme == AppTheme.Light
                                ? lightThemeTextColor
                                : darkThemeTextColor
                        }).Show();
                });
            }
            finally
            {
                _doneSync = false;
            }
        });
    }


    /*
    private void StartScanNetwork()
    {
        int port = Preferences.Get(PreferencesDefault.DEFAULT_PORT, 0);

        TimerCallback timerCallback = async _ =>
        {
            if (_doneSync)
                return;

            var networks = await _networkService.GetKnownNetworks(port);

            if (!networks.Except(ListAvailableHostsCollection).Any())
                return;

            ListAvailableHostsCollection.Clear();
            foreach (string network in networks)
                ListAvailableHostsCollection.Add(network);
        };

        _timer = new Timer(timerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
    }
    */


}