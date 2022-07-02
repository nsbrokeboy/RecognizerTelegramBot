#nullable enable

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RecognizerTelegramBot
{
    public class YandexSpeechKit
    {
        private readonly HttpClient _client = new();
        
        public async Task<string> RecognizeAudio(byte[] oggInBytesArray, CancellationToken cancellationToken)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    Program.config.GetSection("Bearer").Value);
            
            var response = await _client.PostAsync(
                "https://stt.api.cloud.yandex.net/speech/v1/stt:recognize?folderId=b1gennubg43a98hekts0",
                new ByteArrayContent(oggInBytesArray), 
                cancellationToken);

            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            var responseInstance = JsonSerializer.Deserialize<Response>(responseString);

            return !string.IsNullOrEmpty(responseInstance?.Result) ? responseInstance.Result : "---";
        }
    }
}