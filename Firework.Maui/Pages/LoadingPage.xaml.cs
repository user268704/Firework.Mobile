using CommunityToolkit.Mvvm.Messaging;
using Firework.Maui.Messages;
using Firework.Maui.ViewModels;

namespace Firework.Maui.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class LoadingPage : ContentPage
{
    private readonly LoadingPageViewModel ViewModel;

    public LoadingPage(LoadingPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;

        Task.Run(ViewModel.StartConnection);

        WeakReferenceMessenger.Default.Unregister<ShowDisplayAlertMessage>(this);
        WeakReferenceMessenger.Default.Register<ShowDisplayAlertMessage>(this, (r, m) =>
        {
            bool responseUser = false;

            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                responseUser = await DisplayAlert("Ошибка", "Не удалось подключиться к хосту", "Повторить",
                    "Сменить хост");
            }).Wait();

            m.Reply(responseUser);
        });
    }
}