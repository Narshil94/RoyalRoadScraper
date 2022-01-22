using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace HTMLScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string link2chapter1 = "https://www.royalroad.com/fiction/25082/blue-core/chapter/649554/book-3-chapter-1a-day-204-flight-alpha-tlulipechua";
            IndexFetcher ifetcher = new IndexFetcher(link2chapter1);
            ifetcher.getAllChapters();
            var chapters = ifetcher.getChapterList();
            foreach (string chapter in chapters)
            {
                string chaptername = chapter.Split('/').Last();
                var web = new HtmlWeb();
                var doc = web.Load(chapter);
                var nodes = doc.DocumentNode.SelectNodes($"//div[contains(@class,'chapter-inner chapter-content')]");
                string chapterbody  = nodes[0].OuterHtml;

                var nodes_title = doc.DocumentNode.SelectNodes($"//h1[contains(@style,'margin-top: 10px')]");
                string chaptertitle2  = nodes_title[0].OuterHtml;

                ifetcher.Chapters.Add(new IndexFetcher.Chapter(chaptertitle2, chapterbody));
            }

            foreach (var chapter in ifetcher.Chapters)
            {
                Console.WriteLine(chapter.ChapterTitle);
                Console.WriteLine(chapter.ChapterBody);
                String heading = $"<h1>{chapter.ChapterTitle.Replace("-", " ")} </h1>";

                using StreamWriter file = new("Blue Core 3.html", append: true);
                file.WriteLine(heading);
                file.WriteLine(chapter.ChapterBody);
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
            Chapter_links = new List<string>();
            Chapters = new List<Chapter>();
        }

        public String getNextChapter(String chapterlink)
        {
            web = new HtmlWeb();
            doc = web.Load(chapterlink);
            Chapter_links.Add(chapterlink);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes($"//a[@class='{NextChapterbtn}']");
            int node_idx = 0;

            //If its the last chapter exit here
            if((Chapter_links.Count > 1) && (nodes.Count == 1))
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
            return Chapter_links;
        }


        public List<Chapter> Chapters;

        private HtmlWeb web;
        private HtmlDocument doc;
        private String Chapter1link;
        private String NextChapterbtn = "btn btn-primary col-xs-12";
        private String RRLink = "https://www.royalroad.com";
        private List<String> Chapter_links;
        private bool lastChapter = false;

        public class Chapter
        {
            public Chapter(string title, string body)
            {
                ChapterTitle = title;
                ChapterBody = body;
            }
            public string ChapterTitle;
            public string ChapterBody;
        }
    }
}
