using Newtonsoft.Json;

namespace Crawler.API.Models.OpenProjectModels
{
    public class OPCollection<T> where T: Element
    {
        [JsonProperty("_type")]
        public string Type { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("_embedded")]
        public Embedded<T> Embedded { get; set; }       
    }
}