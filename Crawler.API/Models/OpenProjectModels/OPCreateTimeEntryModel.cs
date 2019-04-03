using Newtonsoft.Json;
using System;

namespace Crawler.API.Models.OpenProjectModels
{
    public class OPCreateTimeEntryModel
    {
        [JsonProperty("_links")]
        public OPCreateTimeEntryModelLink Links { get; set; }

        [JsonProperty("hours")]
        public string Hours { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("spentOn")]
        public DateTime SpentOn { get; set; }
    }

    public class OPCreateTimeEntryModelLink
    {
        [JsonProperty("project")]
        public OPCreateTimeEntryModelProject Project { get; set; }

        [JsonProperty("activity")]
        public OPCreateTimeEntryModelActivity Activity { get; set; }

        [JsonProperty("workPackage")]
        public OPCreateTimeEntryModelWorkPackage WorkPackage { get; set; }
    }

    public class OPCreateTimeEntryModelWorkPackage
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public class OPCreateTimeEntryModelActivity
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public class OPCreateTimeEntryModelProject
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }
}