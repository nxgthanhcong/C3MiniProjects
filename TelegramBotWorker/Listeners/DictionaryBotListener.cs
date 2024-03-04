using System.Text;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotWorker.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TelegramBotWorker.Listeners
{
    public class DictionaryBotListener
    {
        private static TelegramBotClient botClient;
        private static CancellationTokenSource cts;

        public async Task ListenIncommingMessage()
        {
            botClient = new TelegramBotClient("1869826512:AAHagbHuIMp6uL0o-Tt1LIoPNwb7fJQNsXs");
            cts = new CancellationTokenSource();
            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: (ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) => {
                    var ErrorMessage = exception switch
                    {
                        ApiRequestException apiRequestException
                            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                        _ => exception.ToString()
                    };

                    Console.WriteLine(ErrorMessage);
                    return Task.CompletedTask;
                },
                receiverOptions: new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>()
                },
                cancellationToken: cts.Token
            );
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                string filePath = "Assets/dict.txt";

                Message message = update.Message;
                long chatId = message.Chat.Id;
                string messageText = message?.Text;
                string username = message.From.FirstName;

                Console.WriteLine($"Received a '{messageText}' message in chat {chatId} of {username}.");
                if(messageText.Contains("cmdreset"))
                {
                    await System.IO.File.WriteAllTextAsync(filePath, string.Empty);
                    return;
                }

                Word w = GetWordFromMessage(messageText);

                Dictionary<string, string> ddict = await GetDDictFromFile(filePath);
                ddict.Add(w.En, w.Vi);

                await WriteDDictToFile(filePath, ddict);

                await botClient.SendTextMessageAsync(
                       chatId: update.Message.Chat.Id,
                       text: "word save succeed",
                       cancellationToken: cancellationToken
                       );
            }
            catch (System.ArgumentException ex)
            {
                await botClient.SendTextMessageAsync(
                       chatId: update.Message.Chat.Id,
                       text: "this word already exist",
                       cancellationToken: cancellationToken
                       );
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(
                       chatId: update.Message.Chat.Id,
                       text: "handler error " + ex,
                       cancellationToken: cancellationToken
                       );
            }
        }

        private Word GetWordFromMessage(string messageText)
        {
            string trimedMessageText = messageText.Trim().ToLower();

            string[] splited = trimedMessageText.Split('-');

            Word w = new Word
            {
                En = splited[0].Trim(),
                Vi = splited[1].Trim(),
            };

            return w;
        }
        
        private async Task<Dictionary<string, string>> GetDDictFromFile(string filePath)
        {
            string dictText = await System.IO.File.ReadAllTextAsync(filePath);
            Dictionary<string, string> ddict = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(dictText))
            {
                ddict = JsonSerializer.Deserialize<Dictionary<string, string>>(dictText);
            }

            return ddict;
        }

        private async Task WriteDDictToFile(string filePath, Dictionary<string, string> ddict)
        {
            await System.IO.File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(ddict, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            }), Encoding.UTF8);
        }
    }
}
