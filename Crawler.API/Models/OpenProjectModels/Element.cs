using Newtonsoft.Json;
using System;

namespace Crawler.API.Models.OpenProjectModels
{
    public class Element
    {
        [JsonProperty("_type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset? UpdatedAt { get; set; }
    }

    public class Project: Element
    {
    }

    public class User: Element
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("admin")]
        public bool Admin { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public object Email { get; set; }

        [JsonProperty("avatar")]
        public Uri Avatar { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("identityUrl")]
        public object IdentityUrl { get; set; }
    }
}