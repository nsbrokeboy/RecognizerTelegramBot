using System.Text.Json.Serialization;

namespace RecognizerTelegramBot
{
    public class Response
    {
        [JsonPropertyName("result")]
        public string Result { get; set; }
    }
}