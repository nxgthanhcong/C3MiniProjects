using TelegramBotWorker.Listeners;
using Translation.Repositories.Interfaces;

namespace TelegramBotWorker
{
    public class Worker : BackgroundService
    {
        private readonly IDictionaryBotListener _dictionaryBotListener;

        public Worker(IDictionaryBotListener dictionaryBotListener)
        {
            _dictionaryBotListener = dictionaryBotListener;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _dictionaryBotListener.ListenIncommingMessage();
        }
    }
}