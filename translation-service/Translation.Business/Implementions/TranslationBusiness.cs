using System.Collections.Generic;
using Translation.Business.Interfaces;
using Translation.Models.DataModels;
using Translation.Repositories.Interfaces;

namespace Translation.Business.Implementions
{
    public class TranslationBusiness : ITranslationBusiness
    {
        ITranslationRepository _translationRepository;

        public TranslationBusiness(ITranslationRepository translationRepository)
        {
            _translationRepository = translationRepository;
        }

        public async Task<int> InsertWord(WordData word)
        {
            return await _translationRepository.InsertWord(word);
        }

        public async Task<LanguageData> GetLanguageByName(string pName)
        {
            return await _translationRepository.GetLanguageByName(pName);
        }

        public async Task<bool> SaveTranslationFromMessage(string pMessageCombine)
        {
            Dictionary<WordData, WordData> dict = await GetWordsDictionaryFromText(pMessageCombine);

            foreach (KeyValuePair<WordData, WordData> entry in dict)
            {
                await _translationRepository.InsertTranslation(entry);
            }

            return true;
        }

        private async Task<Dictionary<WordData, WordData>> GetWordsDictionaryFromText(string pMessageCombine)
        {
            Dictionary < WordData, WordData > dict = new Dictionary<WordData, WordData >();
            string[] arrLine = pMessageCombine.Split('\n');

            foreach (string line in arrLine)
            {
                KeyValuePair<WordData, WordData> keyValuePair = await GetPairWordsFromMessage(line);
                dict.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return dict;
        }

        private async Task<KeyValuePair<WordData, WordData>> GetPairWordsFromMessage(string pMessage)
        {
            string trimedMessageText = pMessage.ToLower();
            string[] splited = trimedMessageText.Split('-');

            int EngLanguageId = (await _translationRepository.GetLanguageByName("English")).LanguageId;
            int VieLanguageId = (await _translationRepository.GetLanguageByName("VietNamese")).LanguageId;

            return new KeyValuePair<WordData, WordData>(
            new WordData
            {
                WordText = splited[0].Trim(),
                LanguageId = EngLanguageId,
            },
            new WordData
            {
                WordText = splited[1].Trim(),
                LanguageId = VieLanguageId,
            });
        }

        public async Task<TranslationData> GetRandomTranslation()
        {
            return await _translationRepository.GetRandomTranslation();
        }
    }
}
