using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace RecognizerTelegramBot
{
    internal class Program
    {
        private static ITelegramBotClient _bot;
        private static YandexSpeechKit _speechKit;
        private static string _token;
        private static WebClient _webClient;

        public static IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddUserSecrets<Program>()
            .Build();
        
        private static void Main(string[] args)
        {
            _token = config.GetSection("TelegramToken").Value;
            _bot = new TelegramBotClient(_token);
            _speechKit = new YandexSpeechKit();
            _webClient = new WebClient();
           
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            
            Console.WriteLine("Запущен бот " + _bot.GetMeAsync(cancellationToken).Result.FirstName);
            
            
            _bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

            while (true)
            {
                
            }
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if(update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                
                if (message.Text?.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(
                        message.Chat,
                        "Привет, я умею распознавать голосовые сообщения длительностью до 30 секунд. Пришли мне их:)");
                    return;
                }

                
                if (message.Voice != null)
                {
                    if (message.Voice.Duration >= 30)
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "Длительность голосового сообщения должна быть меньше 30 секунд.");
                        return;
                    }
                    
                    var voiceFileId = message.Voice.FileId;
                    var fileInfo = await botClient.GetFileAsync(voiceFileId);
                    var filePath = fileInfo.FilePath;
                    
                    var voiceBytes = _webClient.DownloadData($"https://api.telegram.org/file/bot{_token}/{filePath}");
                    
                    var result = await _speechKit.RecognizeAudio(
                        voiceBytes,
                        cancellationToken);
                    
                    Console.WriteLine(result);
                    await botClient.SendTextMessageAsync(message.Chat, result, cancellationToken: cancellationToken);
                }
            }
        }
        
        // ошибки
        private static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await Task.Run(() => Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception)));

        }
    }
}