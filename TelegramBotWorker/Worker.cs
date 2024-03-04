using TelegramBotWorker.Listeners;

namespace TelegramBotWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Push dict running at: {time}", DateTimeOffset.Now);

            //    await Task.Delay(1000, stoppingToken);
            //}
            new DictionaryBotListener().ListenIncommingMessage();
        }
    }
}