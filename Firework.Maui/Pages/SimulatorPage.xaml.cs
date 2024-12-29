namespace Firework.Maui.Pages;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SimulatorPage : ContentPage
{
    private double x, y;

    public SimulatorPage()
    {
        InitializeComponent();

        x = Stick.X;
        y = Stick.Y;
    }

    private void PanUpdate(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Running:
                Pan(e.TotalX, e.TotalY);
                break;

            case GestureStatus.Completed:
                break;
        }
    }

    private void Pan(double totalX, double totalY)
    {
        Stick.TranslationX = totalX;
        Stick.TranslationY = totalY;
    }
}