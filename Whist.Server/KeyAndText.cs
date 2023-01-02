using System.Text.Json.Serialization;

namespace Whist.Server
{
    public sealed class KeyAndText
    {
        [JsonInclude]
        public string? Key;
        [JsonInclude]
        public string? Text;
    }
}