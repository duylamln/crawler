using Crawler.API.Interfaces;
using Crawler.API.Models.OpenProjectModels;
using Crawler.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Crawler.API.Controllers
{
    public class OpenProjectController : ApiController
    {
        private readonly IHttpClientService httpClientService;
        public OpenProjectController(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        [Route("api/openproject/users")]
        public async Task<IHttpActionResult> GetUsers()
        {
            var users = await httpClientService.Get<OPCollection<User>>("https://travel2pay.openproject.com/api/v3/users");
            return Ok(users);
        }

        [Route("api/openproject/users/{id}")]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            var user = await httpClientService.Get<User>("https://travel2pay.openproject.com/api/v3/users/" + id.ToString());
            return Ok(user);
        }

        [Route("api/openproject/projects")]
        public async Task<IHttpActionResult> GetProjects()
        {
            var projects = await httpClientService.Get<OPCollection<Project>>("https://travel2pay.openproject.com/api/v3/projects");
            return Ok(projects);
        }
        
        [Route("api/openproject/versions/{id}")]
        public async Task<IHttpActionResult> GetVersion(int id)
        {
            var version = await httpClientService.Get<Version>($"https://travel2pay.openproject.com/api/v3/versions/{id}");
            return Ok(version);
        }

        [Route("api/openproject/openedversions")]
        public async Task<IHttpActionResult> GetOpenVersions()
        {
            var versions = await httpClientService.Get<OPCollection<OPVersion>>("https://travel2pay.openproject.com/api/v3/versions");

            var openedVersions = versions.Embedded.Elements.Where(x => x.Status == "open" && x.Name.StartsWith("Team Bubble")).ToList();

            var openedVersionCollection = new OPCollection<OPVersion>
            {
                Count = openedVersions.Count,
                Type = "Collection",
                Embedded = new Embedded<OPVersion>
                {
                    Elements = openedVersions
                }
            };

            return Ok(openedVersionCollection);
        }
        private static CollectionViewModel<VersionViewModel> _versionsCache = null;
        //https://travel2pay.openproject.com/api/v3/projects/2/work_packages?pageSize=1000&offset=1&filters=[{"status":{"operator":"o","values":[]}},{"version":{"operator":"=","values":["922"]}}]&sortBy=[["parent","asc"]]
        [Route("api/openproject/versions")]
        public async Task<IHttpActionResult> GetVersions()
        {
            if (_versionsCache != null) return Ok(_versionsCache);
            CollectionViewModel<VersionViewModel> versions = await GetMyVersions();
            _versionsCache = versions;
            return Ok(versions);
        }

        [Route("api/openproject/reloadversions")]
        [HttpGet]
        public async Task<IHttpActionResult> ReloadVersions()
        {
            _versionsCache = null;
            return await GetVersions();
        }

        private async Task<CollectionViewModel<VersionViewModel>> GetMyVersions()
        {
            var bubbleTeamOpenedVersions = await GetBubbleTeamOpenedVersions();

            var versions = new CollectionViewModel<VersionViewModel>();
            foreach (var version in bubbleTeamOpenedVersions)
            {
                var workPackageByVersion = await GetWorkPackageByVersion(version);
                workPackageByVersion = CombineParentPackage(workPackageByVersion);
                versions.Elements.Add(new VersionViewModel
                {
                    Id = version.Id,
                    Title = version.Name,
                    WorkPackages = workPackageByVersion.ToList()
                });
            }

            return versions;
        }

        [Route("api/openproject/timeentryactivities")]
        public IHttpActionResult GetTimeEntryActivities()
        {
            var activities = new List<TimeEntryActivity>() {
                new TimeEntryActivity {
                    Id = 2, Name = "Specification", Order = 1
                },
                new TimeEntryActivity {
                    Id = 3, Name = "Development", Order = 2
                },
                new TimeEntryActivity {
                        Id = 4, Name = "Testing", Order = 3
                },
                new TimeEntryActivity {
                    Id = 19, Name = "Reproduce Bug", Order = 4
                },
                new TimeEntryActivity {
                    Id = 18, Name = "Fix Bug", Order = 5
                },
                new TimeEntryActivity {
                    Id = 5, Name = "Support", Order = 6
                },
                new TimeEntryActivity {
                    Id = 6, Name = "Other", Order = 7
                }
            };

            return Ok(activities);
        }


        [Route("api/openproject/createtimeentry")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateTimeEntry(TimeEntryViewModel model)
        {
            var url = "https://travel2pay.openproject.com/api/v3/time_entries";
            var createTimeEntryRequest = new OPCreateTimeEntryRequest()
            {
                Comment = model.Comment,
                Hours = $"PT{model.Hours}H",
                Links = new OPCreateTimeEntryModelLink()
                {
                    Activity = new OPCreateTimeEntryModelActivity { Href = $"/api/v3/time_entries/activities/{model.ActivityId}" },
                    Project = new OPCreateTimeEntryModelProject { Href = "/api/v3/projects/2" },
                    WorkPackage = new OPCreateTimeEntryModelWorkPackage { Href = $"/api/v3/work_packages/{model.WorkPackageId}" }
                },
                SpentOn = model.SpentOn.ToString("yyyy-MM-dd")
            };
            var timeEntry = await httpClientService.Post<OPCreateTimeEntryRequest, OpCreateTimeEntryResponse>(url, createTimeEntryRequest);

            return Ok(timeEntry);
        }

        [Route("api/openproject/updatetimeentry")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateTimeEntry(TimeEntryViewModel model)
        {
            var url = "https://travel2pay.openproject.com/api/v3/time_entries/" + model.Id;
            var createTimeEntryRequest = new OPCreateTimeEntryRequest()
            {
                Comment = model.Comment,
                Hours = $"PT{model.Hours}H",
                Links = new OPCreateTimeEntryModelLink()
                {
                    Activity = new OPCreateTimeEntryModelActivity { Href = $"/api/v3/time_entries/activities/{model.ActivityId}" },
                    Project = new OPCreateTimeEntryModelProject { Href = "/api/v3/projects/2" },
                    WorkPackage = new OPCreateTimeEntryModelWorkPackage { Href = $"/api/v3/work_packages/{model.WorkPackageId}" }
                },
                SpentOn = model.SpentOn.ToString("yyyy-MM-dd")
            };
            var timeEntry = await httpClientService.Patch<OPCreateTimeEntryRequest, OpCreateTimeEntryResponse>(url, createTimeEntryRequest);

            return Ok(timeEntry);
        }


        /*-------------------------------------private------------------------------------------------*/
        private IEnumerable<WorkPackage> CombineParentPackage(IEnumerable<WorkPackage> workPackageByVersion)
        {
            var parentWorkPackages = workPackageByVersion.Where(x => !x.ParentId.HasValue);
            var childrentWorkPackages = workPackageByVersion.Where(x => x.ParentId.HasValue);
            var result = new List<WorkPackage>();
            result.AddRange(parentWorkPackages);
            result.ForEach(parent => parent.Children.AddRange(childrentWorkPackages.Where(x => x.ParentId.Value == parent.Id)));
            return result;
        }

        private async Task<IEnumerable<WorkPackage>> GetWorkPackageByVersion(OPVersion version)
        {
            string url = "https://travel2pay.openproject.com/api/v3/projects/2/work_packages?pageSize=1000&offset=1&filters=[{\"status\":{\"operator\":\"o\",\"values\":[]}},{\"version\":{\"operator\":\"=\",\"values\":[\"" + version.Id + "\"]}}]&sortBy=[[\"parent\",\"asc\"]]";
            var wps = await httpClientService.Get<OPCollection<OPWorkPackage>>(url);
            return wps.Embedded.Elements.Select(x => new WorkPackage
            {
                Id = x.Id,
                Subject = $"{x.Id} - {x.Subject} - {x.Links.Type.Title}",
                ProjectId = GetId(x.Links.Project.Href).Value,
                ProjectName = x.Links.Project.Title,
                VersionId = GetId(x.Links.Version.Href).Value,
                VersionTitle = x.Links.Version.Title,
                ParentId = GetId(x.Links.Parent.Href)
            });
        }

        private long? GetId(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            return long.Parse(str.Substring(str.LastIndexOf("/") + 1));
        }

        private async Task<List<OPVersion>> GetBubbleTeamOpenedVersions()
        {
            var versions = await httpClientService.Get<OPCollection<OPVersion>>("https://travel2pay.openproject.com/api/v3/versions");
            return versions.Embedded.Elements
                .Where(x => x.Status == "open" && x.Name.StartsWith("Team Bubble"))
                .OrderByDescending(x => x.Id)
                .Take(5)
                .ToList();
        }
    }
}
