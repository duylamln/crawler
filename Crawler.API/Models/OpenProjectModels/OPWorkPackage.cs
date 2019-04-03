using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crawler.API.Models.OpenProjectModels
{
    public partial class OPWorkPackage: Element
    {
        [JsonProperty("description")]
        public new OPWorkPackageDescripion Description { get; set; }

        [JsonProperty("spentTime")]
        public string SpentTime { get; set; }        

        [JsonProperty("lockVersion")]
        public long LockVersion { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("startDate")]
        public object StartDate { get; set; }

        [JsonProperty("dueDate")]
        public object DueDate { get; set; }

        [JsonProperty("estimatedTime")]
        public string EstimatedTime { get; set; }

        [JsonProperty("percentageDone")]
        public long PercentageDone { get; set; }       

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("storyPoints")]
        public object StoryPoints { get; set; }

        [JsonProperty("remainingTime")]
        public object RemainingTime { get; set; }

        [JsonProperty("customField1")]
        public string CustomField1 { get; set; }

        [JsonProperty("customField37")]
        public object CustomField37 { get; set; }

        [JsonProperty("_links")]
        public OPWorkPackageLinks Links { get; set; }
    }

    public partial class OPWorkPackageDescripion
    {
        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("raw")]
        public string Raw { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }
    }

    public partial class OPWorkPackageLinks
    {
        [JsonProperty("attachments")]
        public Activities Attachments { get; set; }

        [JsonProperty("addAttachment")]
        public AddAttachment AddAttachment { get; set; }

        [JsonProperty("self")]
        public Assignee Self { get; set; }

        [JsonProperty("update")]
        public AddAttachment Update { get; set; }

        [JsonProperty("schema")]
        public Activities Schema { get; set; }

        [JsonProperty("updateImmediately")]
        public AddAttachment UpdateImmediately { get; set; }

        [JsonProperty("delete")]
        public AddAttachment Delete { get; set; }

        [JsonProperty("logTime")]
        public Atom LogTime { get; set; }

        [JsonProperty("move")]
        public Atom Move { get; set; }

        [JsonProperty("copy")]
        public Atom Copy { get; set; }

        [JsonProperty("pdf")]
        public Atom Pdf { get; set; }

        [JsonProperty("atom")]
        public Atom Atom { get; set; }

        [JsonProperty("availableRelationCandidates")]
        public Assignee AvailableRelationCandidates { get; set; }

        [JsonProperty("customFields")]
        public Atom CustomFields { get; set; }

        [JsonProperty("configureForm")]
        public Atom ConfigureForm { get; set; }

        [JsonProperty("activities")]
        public Activities Activities { get; set; }

        [JsonProperty("availableWatchers")]
        public Activities AvailableWatchers { get; set; }

        [JsonProperty("relations")]
        public Activities Relations { get; set; }

        [JsonProperty("revisions")]
        public Activities Revisions { get; set; }

        [JsonProperty("watchers")]
        public Activities Watchers { get; set; }

        [JsonProperty("addWatcher")]
        public AddWatcher AddWatcher { get; set; }

        [JsonProperty("removeWatcher")]
        public AddWatcher RemoveWatcher { get; set; }

        [JsonProperty("addRelation")]
        public AddChild AddRelation { get; set; }

        [JsonProperty("addChild")]
        public AddChild AddChild { get; set; }

        [JsonProperty("changeParent")]
        public AddChild ChangeParent { get; set; }

        [JsonProperty("addComment")]
        public AddChild AddComment { get; set; }

        [JsonProperty("previewMarkup")]
        public AddAttachment PreviewMarkup { get; set; }

        [JsonProperty("timeEntries")]
        public Atom TimeEntries { get; set; }

        [JsonProperty("category")]
        public Assignee Category { get; set; }

        [JsonProperty("type")]
        public Assignee Type { get; set; }

        [JsonProperty("priority")]
        public Assignee Priority { get; set; }

        [JsonProperty("project")]
        public Assignee Project { get; set; }

        [JsonProperty("status")]
        public Assignee Status { get; set; }

        [JsonProperty("author")]
        public Assignee Author { get; set; }

        [JsonProperty("responsible")]
        public Assignee Responsible { get; set; }

        [JsonProperty("assignee")]
        public Assignee Assignee { get; set; }

        [JsonProperty("version")]
        public Assignee Version { get; set; }

        [JsonProperty("customField3")]
        public Assignee CustomField3 { get; set; }

        [JsonProperty("customField39")]
        public Assignee CustomField39 { get; set; }

        [JsonProperty("customField40")]
        public Assignee CustomField40 { get; set; }

        [JsonProperty("customField41")]
        public Assignee CustomField41 { get; set; }

        [JsonProperty("watch")]
        public AddWatcher Watch { get; set; }

        [JsonProperty("children")]
        public List<Assignee> Children { get; set; }

        [JsonProperty("ancestors")]
        public List<object> Ancestors { get; set; }

        [JsonProperty("parent")]
        public Assignee Parent { get; set; }

        [JsonProperty("customActions")]
        public List<object> CustomActions { get; set; }
    }

    public partial class Activities
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public partial class AddAttachment
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
    }

    public partial class AddChild
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public partial class AddWatcher
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("payload", NullValueHandling = NullValueHandling.Ignore)]
        public Payload Payload { get; set; }

        [JsonProperty("templated", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Templated { get; set; }
    }

    public partial class Payload
    {
        [JsonProperty("user")]
        public Activities User { get; set; }
    }

    public partial class Assignee
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public partial class Atom
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}