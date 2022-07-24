using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;

namespace Test_Rss
{
    internal class Program
    {
        public class Feed_RSS
        {

            public int Update { get; set; }
            private readonly string Feed_Uri;
            public Feed_RSS(string Feed_Uri)
            {
                this.Feed_Uri = Feed_Uri;
            }

            public async Task GetNewsFeed()
            {
                WebProxy wp = new WebProxy("92.168.1.100", true);
                wp.Credentials = new NetworkCredential("user1", "user1Password");
                WebRequest wrq = WebRequest.Create("http://www.example.com");
                wrq.Proxy = wp;
                WebResponse wrs = wrq.GetResponse();
                List<Instance_Feed> rssNewsItems = new List<Instance_Feed>();
                using (XmlReader xmlReader = XmlReader.Create(Feed_Uri, new XmlReaderSettings() { Async = true }))
                {
                    RssFeedReader feedReader = new RssFeedReader(xmlReader);
                    while (await feedReader.Read())
                    {
                        if (feedReader.ElementType ==  SyndicationElementType.Item)
                        {
                            Instance_Feed rssItem = new Instance_Feed();
                            ISyndicationItem item = await feedReader.ReadItem();
                            rssItem.Discription_News= item.Description;
                            rssItem.Title = item.Title;
                            rssItem.Uri = item.Id;
                            rssItem.PublishDate = item.Published;
                            rssNewsItems.Add(rssItem);
                        }
                    }
                }
               // return rssNewsItems.OrderByDescending(p => p.PublishDate).ToArray();
            }
        }
        static void Main(string[] args)
        {
            Feed_RSS newsFeedService = new Feed_RSS("https://habr.com/rss/interesting/");
            newsFeedService.GetNewsFeed();
            

            //  XmlReader reader = XmlReader.Create("https://habr.com/rss/interesting/");
            //SyndicationFeed feed = SyndicationFeed.Load(reader);

            //List <NewsItem> list = new List<NewsItem>();
            //foreach(var i in feed.Items)
            //{
            //    NewsItem instance = new NewsItem();

            //    list.Add(instance);
            //}
            //foreach(NewsItem instance in list)
            //{
            //    Console.WriteLine(instance);
            //}
            //Console.WriteLine(feed.Title.Text);
            Console.ReadKey();
            //StreamedAtom10FeedFormatter formatter = new StreamedAtom10FeedFormatter(counter);

            //SyndicationFeed feed = formatter.ReadFrom(reader);
        }
    }
}
