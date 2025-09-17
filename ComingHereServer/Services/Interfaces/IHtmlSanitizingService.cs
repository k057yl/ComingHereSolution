namespace ComingHereServer.Services.Interfaces
{
    public interface IHtmlSanitizingService
    {
        string Sanitize(string html);
    }
}
