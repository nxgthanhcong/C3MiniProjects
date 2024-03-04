using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace LotteryTicketCrawler.Services
{
    public class LotteryTicketService : ILotteryTicketService
    {
        public void Print(List<LotteryResult> lotteryResults)
        {
            foreach (var result in lotteryResults)
            {
                Console.WriteLine($"Province: {result.Province}, Date: {result.Date}, Prize: {result.Prize}, Number: {result.Number}");
            }
        }

        public List<LotteryResult> Crawl()
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load("https://xoso.com.vn/xo-so-mien-nam/xsmn-p1.html");

                List<LotteryResult> lotteryResults = ParseLotteryResults(doc);
                return lotteryResults;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        List<LotteryResult> ParseLotteryResults(HtmlDocument doc)
        {
            List<LotteryResult> lotteryResults = new List<LotteryResult>();

            var table = doc.DocumentNode.QuerySelector(".table-result.table-xsmn");
            var date = table.ParentNode.PreviousSibling.QuerySelector(".site-link > a:last-child").InnerText;
            var trs = table.QuerySelectorAll("thead tr").ToList();

            var provinceTr = trs[0];
            var provinceTds = provinceTr.QuerySelectorAll("th").Skip(1).Select(selector => selector.InnerText).ToList();

            for (int i = 1; i < trs.Count; i++)
            {
                var prizeTr = trs[i];
                var prizeTds = prizeTr.QuerySelectorAll("td").Select(selector => selector.InnerText).ToList();
                for (int j = 0; j < prizeTds.Count; j++)
                {
                    var prizeType = GetPrizeType(i);
                    var prizeNumbers = prizeTds[j].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var number in prizeNumbers)
                    {
                        lotteryResults.Add(new LotteryResult
                        {
                            Province = GetProvince(j, provinceTds),
                            Date = date,
                            Prize = prizeType,
                            Number = number,
                        });
                    }
                }
            }

            return lotteryResults;
        }

        static string GetPrizeType(int index)
        {
            switch (index)
            {
                case 1: return "G8";
                case 2: return "G7";
                case 3: return "G6";
                case 4: return "G5";
                case 5: return "G4";
                case 6: return "G3";
                case 7: return "G2";
                case 8: return "G1";
                case 9: return "GDB";
                default: return "";
            }
        }

        static string GetProvince(int index, List<string> provinceTds)
        {
            if (index >= 0 && index < provinceTds.Count)
            {
                return provinceTds[index];
            }
            else
            {
                return "";
            }
        }

        
    }
}
