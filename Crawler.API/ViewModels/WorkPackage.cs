using System.Collections.Generic;

namespace Crawler.API.ViewModels
{
    public class WorkPackage
    {
        public WorkPackage()
        {
            Children = new List<WorkPackage>();
        }
        public string Subject { get; set; }
        public long Id { get; set; }
        public long VersionId { get; set; }
        public string VersionTitle { get; set; }
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public long? ParentId { get; set; }
        public List<WorkPackage> Children { get; set; }
    }
}