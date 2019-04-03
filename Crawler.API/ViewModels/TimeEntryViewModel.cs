using System;

namespace Crawler.API.ViewModels
{
    public class TimeEntryViewModel
    {
        public long PojectId { get; set; }
        public long ActivityId { get; set; }
        public long WorkPackageId { get; set; }
        public decimal Hours { get; set; }
        public string Comment { get; set; }
        public DateTime SpentOn { get; set; }
    }
}