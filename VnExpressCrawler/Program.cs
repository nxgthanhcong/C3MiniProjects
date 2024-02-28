
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

Console.OutputEncoding = System.Text.Encoding.UTF8;

HtmlWeb web = new HtmlWeb();
HtmlDocument doc = web.Load("https://vnexpress.net/giai-tri");

var itemNews = doc.DocumentNode.QuerySelectorAll("#automation_5News > article");

foreach (var item in itemNews)
{
    var className = item.GetAttributeValue("class", "");
    if (className.Contains("close_not_qc"))
    {
        continue;
    }

    string title = item.QuerySelector(".title-news > a").InnerText;
    string imgUrl = item.QuerySelector(".thumb-art > a").GetAttributeValue("href", "");
    string description = item.QuerySelector(".description > a").InnerText;

    Console.WriteLine(title + imgUrl + description);
}

Console.ReadKey();
