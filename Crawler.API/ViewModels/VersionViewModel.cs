using System.Collections.Generic;

namespace Crawler.API.ViewModels
{
    public class VersionViewModel
    {
        public VersionViewModel()
        {
            WorkPackages = new List<WorkPackage>();
        }
        public long Id { get; set; }
        public string Title { get; set; }
        public List<WorkPackage> WorkPackages { get; set; }
    }
}