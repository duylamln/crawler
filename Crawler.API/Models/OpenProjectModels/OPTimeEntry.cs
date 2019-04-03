using Newtonsoft.Json;
using System;

namespace Crawler.API.Models.OpenProjectModels
{
    public partial class OpTimeEntry: Element
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("spentOn")]
        public DateTimeOffset SpentOn { get; set; }

        [JsonProperty("hours")]
        public string Hours { get; set; }

        [JsonProperty("_links")]
        public OPTimeEntryLinks Links { get; set; }
    }

    public partial class OPTimeEntryLinks
    {
        [JsonProperty("self")]
        public Self Self { get; set; }

        [JsonProperty("project")]
        public OpTimeEntryProject Project { get; set; }

        [JsonProperty("workPackage")]
        public OpTimeEntryWorkPackage WorkPackage { get; set; }

        [JsonProperty("user")]
        public OpTimeEntryUser User { get; set; }

        [JsonProperty("activity")]
        public OpTimeEntryActivity Activity { get; set; }
    }

    public partial class OpTimeEntryProject
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }


    public partial class OpTimeEntryUser
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }


    public partial class OpTimeEntryWorkPackage
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }


    public partial class OpTimeEntryActivity
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }


    public partial class OpTimeEntrySelf
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }
}