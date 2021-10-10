using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace HTMLScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            String RRLink = "https://www.royalroad.com/";
            String Chptr1Link = "https://www.royalroad.com/fiction/41033/kairos-a-greek-myth-litrpg/chapter/642057/1-the-legend-of-kairos";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Chptr1Link);
            var nodes = doc.DocumentNode.SelectNodes("//a[@class='btn btn-primary col-xs-12']");
            String link = nodes[0].GetAttributeValue("href", null);

            IndexFetcher ifetcher = new IndexFetcher();
            while(true)
            {
                String nextChap = ifetcher.getNextChapter(Chptr1Link);
                String link2nextChap = RRLink + nextChap;
                Console.WriteLine(link2nextChap);
                Chptr1Link = link2nextChap;
            }
        }
    }

    class IndexFetcher
    {
        //Example link
        //https://www.royalroad.com/fiction/41033/kairos-a-greek-myth-litrpg/chapter/642057/1-the-legend-of-kairos
        public IndexFetcher()
        {
            Chapters = new List<string>();
            //regex check for valid link
        }

        public String getNextChapter(String chapterlink)
        {
            web = new HtmlWeb();
            doc = web.Load(chapterlink);
            Chapters.Add(chapterlink);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes($"//a[@class='{NextChapterbtn}']");
            String nextChapterLink = nodes[0].GetAttributeValue("href", null);

            return nextChapterLink;
        }

        private HtmlWeb web;
        private HtmlDocument doc;
        private String Chapter1link;
        private String NextChapterbtn = "btn btn-primary col-xs-12";
        private String RRLink = "https://www.royalroad.com/";
        private List<String> Chapters;
    }
}
