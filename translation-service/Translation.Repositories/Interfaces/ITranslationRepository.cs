using Translation.Models.DataModels;

namespace Translation.Repositories.Interfaces
{
    public interface ITranslationRepository
    {
        Task Test();
        Task<int> InsertWord(WordData word);
        Task<LanguageData> GetLanguageByName(string pName);
        Task<int> InsertTranslation(KeyValuePair<WordData, WordData> pKeyValue);
        Task<TranslationData> GetTranslationByPairWordId(int pWordId, int pTranslatedWordId);
        Task<TranslationData> GetRandomTranslation();
    }
}
