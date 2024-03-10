using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Translation.Business.Interfaces;

namespace TelegramBotWorker.Listeners
{
    public class DictionaryBotListener : IDictionaryBotListener
    {
        ITranslationBusiness _translationBusiness;
        
        private static TelegramBotClient botClient;
        private static CancellationTokenSource cts;

        public DictionaryBotListener(ITranslationBusiness translationBusiness)
        {
            _translationBusiness = translationBusiness;
        }

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
                Message message = update.Message;
                long chatId = message.Chat.Id;
                string messageText = message?.Text;
                string username = message.From.FirstName;

                Console.WriteLine($"Received a '{messageText}' message in chat {chatId} of {username}.");

                await _translationBusiness.SaveTranslationFromMessage(messageText);

                await botClient.SendTextMessageAsync(
                       chatId: update.Message.Chat.Id,
                       text: "word save succeed",
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
    }
}
