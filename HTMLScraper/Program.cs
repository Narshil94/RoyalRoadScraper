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
            IndexFetcher ifetcher = new IndexFetcher("https://www.royalroad.com/fiction/41033/kairos-a-greek-myth-litrpg/chapter/642057/1-the-legend-of-kairos");
            ifetcher.getAllChapters();
            var chapters = ifetcher.getChapterList();
            foreach (var chapter in chapters)
            {
                Console.WriteLine(chapter);
                var web = new HtmlWeb();
                var doc = web.Load(chapter);
                var nodes = doc.DocumentNode.SelectNodes($"//div[contains(@class,'chapter-inner chapter-content')]");
                Console.WriteLine(nodes[0].InnerHtml);
            }
        }
    }

    class IndexFetcher
    {
        //Example link
        //https://www.royalroad.com/fiction/41033/kairos-a-greek-myth-litrpg/chapter/642057/1-the-legend-of-kairos
        public IndexFetcher(String chapter1link)
        {
            Chapter1link = chapter1link;
            Chapters = new List<string>();
        }

        public String getNextChapter(String chapterlink)
        {
            web = new HtmlWeb();
            doc = web.Load(chapterlink);
            Chapters.Add(chapterlink);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes($"//a[@class='{NextChapterbtn}']");
            int node_idx = 0;

            //If its the last chapter exit here
            if((Chapters.Count > 1) && (nodes.Count == 1))
            {
                lastChapter = true;
                return "";
            }

            //There are 2 buttons on every chapter except first and last
            if(nodes.Count > 1)
            {
                node_idx = 1;
            }
            String nextChapterLink = nodes[node_idx].GetAttributeValue("href", null);

            return nextChapterLink;
        }

        public int getAllChapters()
        {
            int chaptercount = 0;
            //We dont know how many chapters there will be before we start
            while(true)
            {
                chaptercount++;
                String nextChap = getNextChapter(Chapter1link);
                if(lastChapter)
                {
                    break;
                }
                String link2nextChap = RRLink + nextChap;
                Chapter1link = link2nextChap;
            }
            return chaptercount;
        }

        public bool isLastChapter()
        {
            return lastChapter;
        }

        public List<String> getChapterList()
        {
            return Chapters;
        }

        private HtmlWeb web;
        private HtmlDocument doc;
        private String Chapter1link;
        private String NextChapterbtn = "btn btn-primary col-xs-12";
        private String RRLink = "https://www.royalroad.com";
        private List<String> Chapters;
        private bool lastChapter = false;
    }
}
