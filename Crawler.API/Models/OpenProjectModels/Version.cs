using Newtonsoft.Json;
using System;

namespace Crawler.API.Models.OpenProjectModels
{
    public partial class OPVersion: Element
    {
        [JsonProperty("description")]
        public new OPVersionDescripion Description { get; set; }

        [JsonProperty("startDate")]
        public DateTimeOffset? StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTimeOffset? EndDate { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("_links")]
        public VersionLinks Links { get; set; }
    }

    public partial class OPVersionDescripion
    {
        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("raw")]
        public string Raw { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }
    }

    public partial class VersionLinks
    {
        [JsonProperty("self")]
        public DefiningProject Self { get; set; }

        [JsonProperty("definingProject")]
        public DefiningProject DefiningProject { get; set; }

        [JsonProperty("availableInProjects")]
        public AvailableInProjects AvailableInProjects { get; set; }
    }

    public partial class AvailableInProjects
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public partial class DefiningProject
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}