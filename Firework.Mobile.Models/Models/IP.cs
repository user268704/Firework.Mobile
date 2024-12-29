namespace Firework.Mobile.Models.Models;

public class IP
{
    public byte? First { get; set; }
    public byte? Second { get; set; }
    public byte? Third { get; set; }
    public byte? Fourth { get; set; }

    public override string ToString()
    {
        return $"{First}.{Second}.{Third}.{Fourth}";
    }
}