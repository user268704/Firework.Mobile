namespace Firework.Maui.Extensions;

public static class ContentViewExtension
{
    public static Page GetParentPage(this Element element)
    {
        while (element != null)
        {
            if (element is Page page)
                return page;

            element = element.Parent;
        }

        return null;
    }
}