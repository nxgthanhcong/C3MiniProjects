using Translation.Models.DataModels;

namespace Translation.Business.Interfaces
{
    public interface ITranslationBusiness
    {
        Task<int> InsertWord(WordData word);
        Task<LanguageData> GetLanguageByName(string pName);
        Task<bool> SaveTranslationFromMessage(string pMessage);
        Task<TranslationData> GetRandomTranslation();

    }
}
