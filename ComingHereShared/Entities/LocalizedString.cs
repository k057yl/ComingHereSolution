using System.Text.Json.Serialization;

namespace ComingHereShared.Entities
{
    public class LocalizedString
    {
        public Dictionary<string, string> Values { get; set; } = new();

        public string Get(string culture, string fallback = "uk")
        {
            if (Values.TryGetValue(culture, out var value))
                return value;

            return Values.TryGetValue(fallback, out var fallbackValue)
                ? fallbackValue
                : string.Empty;
        }

        public void Set(string culture, string value)
        {
            Values[culture] = value;
        }

        public override string ToString() => Get("uk");
    }
}