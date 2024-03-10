namespace Translation.Models.DataModels
{
    public class TranslationData
    {
        public int TranslationId { get; set; }
        public int WordId { get; set; }
        public int TranslatedWordId { get; set; }
        public WordData Word { get; set; }
        public WordData TranslatedWord { get; set; }
    }

}
