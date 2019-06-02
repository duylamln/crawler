using Crawler.API.Interfaces;
using Crawler.API.Models.OpenProjectModels;
using Crawler.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Crawler.API.Controllers
{
    public class OpenProjectController : ApiController
    {
        private readonly IHttpClientService httpClientService;
        private readonly DateTime _today;
        private readonly DateTime _last2Week;
        private readonly DateTime _next2week;

        private readonly string _logFileFolder = @"log";
        private readonly string _logFilePath = @"log.txt";

        static readonly object _object = new object();

        public OpenProjectController(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
            _today = DateTime.Now;
            _last2Week = _today.AddDays(-14).Date + new TimeSpan(0, 0, 0);
            _next2week = _today.AddDays(28).Date + new TimeSpan(23, 59, 59);
            _logFileFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _logFileFolder);
            if (!Directory.Exists(_logFilePath)) Directory.CreateDirectory(_logFileFolder);
            _logFilePath = Path.Combine(_logFileFolder, _logFilePath);
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

        //https://travel2pay.openproject.com/api/v3/projects/2/work_packages?pageSize=1000&offset=1&filters=[{"status":{"operator":"o","values":[]}},{"version":{"operator":"=","values":["922"]}}]&sortBy=[["parent","asc"]]
        [Route("api/openproject/versions")]
        public async Task<IHttpActionResult> GetVersions()
        {
            CollectionViewModel<VersionViewModel> versions = await GetMyVersions();
            return Ok(versions);
        }

        private async Task<CollectionViewModel<VersionViewModel>> GetMyVersions()
        {
            var sw = Stopwatch.StartNew();

            var openVersions = await GetOpenedVersions();

            WriteText($"Get open versions", sw.ElapsedMilliseconds);

            var versions = new CollectionViewModel<VersionViewModel>();
            var tasks = new List<Task>();
            foreach (var version in openVersions)
            {
                tasks.Add(Task.Run(() => GetWorkpackagesByVersion(versions, version)));
            }

            Task.WaitAll(tasks.ToArray());

            versions.Elements = versions.Elements.OrderBy(x => x.Title).ThenBy(x => x.Id).ToList();
            return versions;
        }

        private async Task GetWorkpackagesByVersion(CollectionViewModel<VersionViewModel> versions, OPVersion version)
        {
            var sw = Stopwatch.StartNew();
            WriteText($"Get workpackages of version [{version.Name}] Start", 0);
            var workPackageByVersion = await GetWorkPackageByVersion(version);

            WriteText($"Get workpackages of version [{version.Name}] End", sw.ElapsedMilliseconds);

            workPackageByVersion = CombineParentPackage(workPackageByVersion);
            versions.Elements.Add(new VersionViewModel
            {
                Id = version.Id,
                Title = version.Name,
                WorkPackages = workPackageByVersion.ToList()
            });
        }

        private void WriteText(string s, long elapsed)
        {
            lock (_object)
            {
                File.AppendAllText(_logFilePath, s.PadRight(100, '.') + ":" + elapsed + "ms" + Environment.NewLine);
            }
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

        [Route("api/openproject/deletetimeentry/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteTimeEntry(long id)
        {
            var url = "https://travel2pay.openproject.com/api/v3/time_entries/" + id;
            var result = await httpClientService.Delete(url);
            return Ok(result);
        }


        /*-------------------------------------private------------------------------------------------*/
        private IEnumerable<WorkPackage> CombineParentPackage(IEnumerable<WorkPackage> workPackageByVersion)
        {
            var workPackageByVersionIds = workPackageByVersion.Select(x => x.Id);
            var parentWorkPackages = workPackageByVersion.Where(x => !x.ParentId.HasValue);
            var parentWorkPakageIds = parentWorkPackages.Select(x => x.Id);

            var childrentWorkPackages = workPackageByVersion.Where(x => x.ParentId.HasValue);
            var result = new List<WorkPackage>();

            result.AddRange(parentWorkPackages);
            result.AddRange(workPackageByVersion
                .Where(x => x.ParentId.HasValue)
                .Where(x => !workPackageByVersionIds.Contains(x.ParentId.Value))
            );

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

        private async Task<List<OPVersion>> GetOpenedVersions()
        {
            var versions = await httpClientService.Get<OPCollection<OPVersion>>("https://travel2pay.openproject.com/api/v3/versions");
            var result = versions.Embedded.Elements
                .Where(x => x.Status == "open" && x.Name.StartsWith("Team Bubble"))
                //.Where(x => x.Status == "open")
                //.Where(x => x.Name.StartsWith("Team Genius"))
                .Where(SameTime)
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .Take(20)
                .ToList();
            var today = DateTime.Now;

            return result;
        }

        private bool SameTime(OPVersion version)
        {
            var name = version.Name;
            var lastSpaceIndex = name.Trim().LastIndexOf(' ');
            if (lastSpaceIndex == -1) return false;
            var versionDateStr = name.Substring(name.Trim().LastIndexOf(' '));
            if (!DateTime.TryParse(versionDateStr, out DateTime versionDate)) return false;

            if ((_last2Week < versionDate) && (versionDate < _next2week)) return true;

            return false;

        }
    }
}
