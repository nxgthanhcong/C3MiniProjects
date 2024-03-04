namespace LotteryTicketCrawler.Services
{
    public interface ILotteryTicketService
    {
        List<LotteryResult> Crawl();
        void Print(List<LotteryResult> lotteryResults);
    }
}
