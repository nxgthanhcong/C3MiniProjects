using LotteryTicketCrawler;
using LotteryTicketCrawler.Services;
using Quartz;
using System.Text;
using System.Text.Json;

namespace LotteryTicketCronJob
{
    public class LotteryTicketJob : IJob
    {
        ILotteryTicketService lotteryTicketService;
        IConfiguration configuration;
        string apiUrl;
        public LotteryTicketJob(ILotteryTicketService lotteryTicketService, IConfiguration configuration)
        {
            this.lotteryTicketService = lotteryTicketService;
            this.configuration = configuration;
            string botToken = configuration["TelegramNotifyBot:Token"];
            apiUrl = $"https://api.telegram.org/bot{botToken}/sendMessage";
        }
        
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("job run");
            
            List<LotteryResult> lotteryResults = lotteryTicketService.Crawl();
            lotteryTicketService.Print(lotteryResults);

            return Task.FromResult(true);
        }


        private async Task SendTelegramNotiMessageAsync(string message)
        {
            string chatId = configuration["TelegramNotifyBot:ChatId"];

            string longText = message;
            int chunkSize = 4096; // Maximum message size for Telegram

            // Split the long text into smaller chunks
            for (int i = 0; i < longText.Length; i += chunkSize)
            {
                string chunk = longText.Substring(i, Math.Min(chunkSize, longText.Length - i));

                // Send each chunk as a separate message
                await PostAsync(apiUrl, new
                {
                    chat_id = chatId,
                    text = chunk,
                });
            }
        }

        private async Task<HttpResponseMessage> PostAsync(string apiUrl, object contentObj)
        {
            string jsonBody = JsonSerializer.Serialize(contentObj);
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(apiUrl, new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                return response;
            }
        }
    }
}
