namespace Translation.Models.DataModels
{
    public class WordData
    {
        public int WordId { get; set; }
        public int LanguageId { get; set; }
        public string WordText { get; set; }
        public LanguageData Language { get; set; }
    }

}
