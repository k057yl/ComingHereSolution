using System.Text.Json;

namespace ComingHereClient.Services
{
    public class LocalizationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private Dictionary<string, string> _strings = new();

        public LocalizationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public string this[string key] => _strings.TryGetValue(key, out var val) ? val : $"[{key}]";

        public async Task LoadAsync(string culture)
        {
            var client = _httpClientFactory.CreateClient("StaticFilesClient");
            var json = await client.GetStringAsync($"i18n/{culture}.json");
            _strings = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
        }
    }
}