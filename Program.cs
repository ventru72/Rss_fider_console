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
    public  class Program
    {
        public class Feed_RSS
        {

            public int Update { get; set; }
            public string Url { get; set; }
            public string Proxy_Ip { get; set; }
            public string Proxy_User { get; set; }
            public string Proxy_Password { get; set; }


            private readonly string Feed_Uri;
            public Feed_RSS( )
            {
                 
            }
            public Feed_RSS(string Proxy_Ip, string Proxy_User, string Proxy_Password) 
            {
                this.Proxy_Ip = Proxy_Ip;
                this.Proxy_User = Proxy_User;
                this.Proxy_Password = Proxy_Password;
            }
            public Feed_RSS(string Feed_Uri)
            {
                this.Feed_Uri = Feed_Uri;
            }
            public void Initializing_Settings()
            {
                Feed_RSS feed_RSS_setting = new Feed_RSS("dfdf", "dfdf", "dfdf");
                string path = "setting.xml";
                XmlSerializer deser = new XmlSerializer(typeof(Feed_RSS), path);
                using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                   deser.Serialize(stream, feed_RSS_setting);
                }
            }
            public Feed_RSS Deser()
            {
                
                string path = "setting.xml";
                XmlSerializer deser = new XmlSerializer(typeof(Feed_RSS), path);
                using(Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    Feed_RSS feed_RSS_setting = deser.Deserialize(stream) as Feed_RSS;
                    return feed_RSS_setting;
                }
            }
            public async Task GetNewsFeed()
            {
                Feed_RSS feed_RSS_setting = Deser();
                if (feed_RSS_setting.Update == 0) feed_RSS_setting.Update = 1;

                WebProxy wp = new WebProxy(feed_RSS_setting.Proxy_Ip, true);
                wp.Credentials = new NetworkCredential(feed_RSS_setting.Proxy_User, feed_RSS_setting.Proxy_Password);
                WebRequest wrq = WebRequest.Create("http://www.example.com");
                wrq.Proxy = wp;
                WebResponse wrs = wrq.GetResponse(); 
                
                List<Instance_Feed> rssNewsItems = new List<Instance_Feed>();

                bool exit = false;
                while (exit != true)
                {
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
                    await Task.Delay(feed_RSS_setting.Update * 3600);
                }

            }
        }
        static void Main(string[] args)
        {
           
            Feed_RSS newsFeedService = new Feed_RSS("https://habr.com/rss/interesting/");

            newsFeedService.GetNewsFeed();

            newsFeedService.Initializing_Settings();

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
