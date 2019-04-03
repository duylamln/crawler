using Newtonsoft.Json;
using System.Collections.Generic;

namespace Crawler.API.Models.OpenProjectModels
{
    public partial class Embedded<T> where T: Element
    {
        [JsonProperty("elements")]
        public List<T> Elements { get; set; }
    }
}