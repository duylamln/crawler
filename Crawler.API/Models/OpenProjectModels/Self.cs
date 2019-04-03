using Newtonsoft.Json;

namespace Crawler.API.Models.OpenProjectModels
{
    public partial class Self
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("templated")]
        public bool Templated { get; set; }
    }
}