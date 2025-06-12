using System.Text.Json;

namespace ComingHereClient.Services
{
    public class LocalizationService
    {
        private readonly HttpClient _http;

        private Dictionary<string, string> _strings = new();

        public LocalizationService(HttpClient http)
        {
            _http = http;
        }

        public string this[string key] => _strings.TryGetValue(key, out var val) ? val : $"[{key}]";

        public async Task LoadAsync(string culture)
        {
            var json = await _http.GetStringAsync($"i18n/{culture}.json");
            _strings = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
        }
    }
}
