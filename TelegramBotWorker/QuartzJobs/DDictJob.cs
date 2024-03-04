using Quartz;
using System;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotWorker.QuartzJobs
{
    public class DDictJob : IJob
    {
        private static TelegramBotClient botClient = new TelegramBotClient("1869826512:AAHagbHuIMp6uL0o-Tt1LIoPNwb7fJQNsXs");

        public async Task Execute(IJobExecutionContext context)
        {
            Dictionary<string, string> ddict = await GetDDictFromFile("Assets/dict.txt");

            int currentIndex = await GetCurrentIndex();

            var kvpAtIndex = ddict.ElementAt(currentIndex);
            var keyAtIndex = kvpAtIndex.Key;
            var valueAtIndex = kvpAtIndex.Value;

            List<long> customers = await GetDictCustomersFromFile();
            foreach( var customer in customers )
            {
                await botClient.SendTextMessageAsync(chatId: customer, text: $"{keyAtIndex} - {valueAtIndex}");
            }

            if (currentIndex < ddict.Count - 1)
            {
                await SetCurrentIndex(currentIndex + 1);
            }
        }

        private async Task<int> GetCurrentIndex()
        {
            string dictText = await System.IO.File.ReadAllTextAsync("Assets/currentIndex.txt");
            int.TryParse(dictText, out int rs);
            return rs;
        }

        private async Task SetCurrentIndex(int index)
        {
            await System.IO.File.WriteAllTextAsync("Assets/currentIndex.txt", index.ToString());
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

        private async Task<List<long>> GetDictCustomersFromFile()
        {
            string dictText = await System.IO.File.ReadAllTextAsync("Assets/dictCustomers.txt");
            return JsonSerializer.Deserialize<List<long>>(dictText);
        }

    }
}
