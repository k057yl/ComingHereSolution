using ComingHereServer.Services.Interfaces;
using Ganss.Xss;

namespace ComingHereServer.Services
{
    public class HtmlSanitizingService : IHtmlSanitizingService
    {
        private readonly HtmlSanitizer _sanitizer;

        public HtmlSanitizingService()
        {
            _sanitizer = new HtmlSanitizer();

            _sanitizer.AllowedTags.Clear();
            _sanitizer.AllowedTags.UnionWith(new[] { "a", "p", "strong", "b", "i", "code" });

            _sanitizer.AllowedAttributes.Clear();
            _sanitizer.AllowedAttributes.UnionWith(new[] { "href", "title" });

            _sanitizer.KeepChildNodes = true;
        }

        public string Sanitize(string html)
        {
            return _sanitizer.Sanitize(html);
        }
    }
}