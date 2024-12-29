namespace Firework.Maui.Dependency;

public interface IToast
{
    void LongMessage(string message);
    void ShortMessage(string message);
}