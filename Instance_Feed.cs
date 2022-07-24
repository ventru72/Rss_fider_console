using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Rss
{
    internal class Instance_Feed
    {
        public Instance_Feed() { }
        public Instance_Feed(string Title, string Discription_News, DateTimeOffset PublishDate, string Uri)
        {
            this.Title = Title;
            this.Discription_News = Discription_News;
            this.PublishDate = PublishDate;
            this.Uri = Uri;
        }

        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string Discription_News { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public string Uri { get; set; }
    }
}
