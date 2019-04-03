using Newtonsoft.Json;
using System.Collections.Generic;

namespace Crawler.API.Models.OpenProjectModels
{
    public partial class Query: Element
    {
        [JsonProperty("starred")]
        public bool Starred { get; set; }

        [JsonProperty("filters")]
        public List<Filter> Filters { get; set; }

        [JsonProperty("sums")]
        public bool Sums { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }

        [JsonProperty("timelineVisible")]
        public bool TimelineVisible { get; set; }

        [JsonProperty("showHierarchies")]
        public bool ShowHierarchies { get; set; }

        [JsonProperty("timelineZoomLevel")]
        public string TimelineZoomLevel { get; set; }

        [JsonProperty("timelineLabels")]
        public TimelineLabels TimelineLabels { get; set; }

        [JsonProperty("highlightingMode")]
        public string HighlightingMode { get; set; }

        [JsonProperty("_links")]
        public QueryLinks Links { get; set; }
    }

    public partial class Filter
    {
        [JsonProperty("_type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("_links")]
        public FilterLinks Links { get; set; }
    }

    public partial class FilterLinks
    {
        [JsonProperty("schema")]
        public Results Schema { get; set; }

        [JsonProperty("filter")]
        public GroupBy Filter { get; set; }

        [JsonProperty("operator")]
        public GroupBy Operator { get; set; }

        [JsonProperty("values")]
        public List<object> Values { get; set; }
    }

    public partial class GroupBy
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public partial class Results
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public partial class QueryLinks
    {
        [JsonProperty("self")]
        public GroupBy Self { get; set; }

        [JsonProperty("project")]
        public GroupBy Project { get; set; }

        [JsonProperty("results")]
        public Results Results { get; set; }

        [JsonProperty("schema")]
        public Results Schema { get; set; }

        [JsonProperty("update")]
        public Update Update { get; set; }

        [JsonProperty("user")]
        public GroupBy User { get; set; }

        [JsonProperty("sortBy")]
        public List<object> SortBy { get; set; }

        [JsonProperty("groupBy")]
        public GroupBy GroupBy { get; set; }

        [JsonProperty("columns")]
        public List<GroupBy> Columns { get; set; }

        [JsonProperty("highlightedAttributes")]
        public List<object> HighlightedAttributes { get; set; }
    }

    public partial class Update
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
    }

    public partial class TimelineLabels
    {
    }
}