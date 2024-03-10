using Quartz;
using Telegram.Bot;
using Translation.Business.Interfaces;
using Translation.Models.DataModels;

namespace TelegramBotWorker.QuartzJobs
{
    public class DDictJob : IJob
    {
        private static TelegramBotClient botClient = new TelegramBotClient("1869826512:AAHagbHuIMp6uL0o-Tt1LIoPNwb7fJQNsXs");
        ITranslationBusiness _translationBusiness;

        public DDictJob(ITranslationBusiness translationBusiness)
        {
            _translationBusiness = translationBusiness;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            TranslationData translationData = await _translationBusiness.GetRandomTranslation();
            List<string> customers = new List<string>
            {
                "1605537163",
                //"6892907913",
            };
            foreach ( var customer in customers)
            {
                await botClient.SendTextMessageAsync(chatId: customer, text: $"{translationData.Word.WordText} - {translationData.TranslatedWord.WordText}");
            }
        }
    }
}
