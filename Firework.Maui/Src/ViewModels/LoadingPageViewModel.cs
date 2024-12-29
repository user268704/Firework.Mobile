using System.Net.NetworkInformation;
using CommunityToolkit.Mvvm.Messaging;
using Firework.Maui.Messages;
using Firework.Maui.Pages;
using Firework.Mobile.Abstraction.Network;

namespace Firework.Maui.ViewModels;

public class LoadingPageViewModel
{
    private readonly IConnectionService _connectionService;
    private readonly IServiceProvider _serviceProvider;


    public LoadingPageViewModel(IConnectionService connectionService, IServiceProvider serviceProvider)
    {

        _connectionService = connectionService;
        _serviceProvider = serviceProvider;

        NetworkChange.NetworkAvailabilityChanged += NetworkChangeHandler;
    }

    public void StartConnection()
    {
        bool connectResult;

        if (!CheckNetworkAvailable())
        {
            var result = WeakReferenceMessenger.Default.Send(new ShowDisplayAlertMessage()
            {
                Title = "Ошибка",
                Message = "Отсутсвует подключение к сети",
                Ok = "Повторить",
                Cancel = "Отмена"
            });

            if (result.Response)
            {
                StartConnection();
                return;
            }

            connectResult = false;
        }
        else
        {
            connectResult = _connectionService.TryStartConnection();
        }

        if (connectResult)
            ToMainPage();
        else
        {
            var result = WeakReferenceMessenger.Default.Send(new ShowDisplayAlertMessage
            {
                Title = "Ошибка",
                Message = "Не удалось подключиться к хосту",
                Ok = "Повторить",
                Cancel = "Сменить хост"
            });

            if (result.Response)
            {
                StartConnection();
                return;
            }

            ToConnectionPage();
        }
    }

    private bool CheckNetworkAvailable()
    {
        return NetworkInterface.GetIsNetworkAvailable();
    }

    private void NetworkChangeHandler(object sender, NetworkAvailabilityEventArgs e)
    {
        CheckNetworkAvailable();
    }

    private void ToMainPage()
    {
        Dispose();

        MainThread.InvokeOnMainThreadAsync(() =>
        {
            Application.Current.MainPage = new AppShell();
        });
    }

    private void ToConnectionPage()
    {
        Dispose();

        MainThread.InvokeOnMainThreadAsync(() =>
        {
            var connectionPage = _serviceProvider.GetRequiredService<ConnectionPage>();

            Application.Current.MainPage = connectionPage;
        });
    }

    public void Dispose()
    {
        NetworkChange.NetworkAvailabilityChanged -= NetworkChangeHandler;
    }


}