using System;
using HtmlAgilityPack;

namespace HTMLScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://www.royalroad.com/fiction/41033/kairos-a-greek-myth-litrpg/chapter/642057/1-the-legend-of-kairos");

            var nodes = doc.DocumentNode.SelectNodes("//a[@class='btn btn-primary col-xs-12']");
            String link = nodes[0].GetAttributeValue("href",null);
        }
    }
}
