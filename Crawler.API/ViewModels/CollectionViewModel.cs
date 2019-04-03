using System.Collections.Generic;

namespace Crawler.API.ViewModels
{
    public class CollectionViewModel<T>
    {
        public CollectionViewModel()
        {
            Elements = new List<T>();
        }
        public long Total { get; set; }
        public long Count { get { return Elements.Count; } }
        public List<T> Elements { get; set; }
    }
}