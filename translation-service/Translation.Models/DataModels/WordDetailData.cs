namespace Translation.Models.DataModels
{
    public class WordDetailData
    {
        public int DetailId { get; set; }
        public int WordId { get; set; }
        public string PartOfSpeech { get; set; }
        public string Definition { get; set; }
        public string Example { get; set; }
        public WordData Word { get; set; }
    }

}
