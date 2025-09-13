using System.Text.Json.Serialization;

namespace ComingHereShared.Entities
{
    public class LocalizedString
    {
        public Dictionary<string, string> Values { get; set; } = new();

        public LocalizedString() { }

        public LocalizedString(string value, string culture = "uk")
        {
            Values[culture] = value;
        }

        public LocalizedString(LocalizedString other)
        {
            if (other == null) return;
            foreach (var kvp in other.Values)
                Values[kvp.Key] = kvp.Value;
        }

        public string Get(string culture, string fallback = "uk")
        {
            if (Values.TryGetValue(culture, out var value))
                return value;

            return Values.TryGetValue(fallback, out var fallbackValue) ? fallbackValue : string.Empty;
        }

        public void Set(string culture, string value) => Values[culture] = value;

        public override string ToString() => Get("uk");
    }
}