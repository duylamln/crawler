using System;

namespace Crawler.API.ViewModels
{
    public class TimeEntryViewModel
    {
        public TimeEntryViewModel()
        {
            ProjectId = 2;
            ActivityId = 6;
        }
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public long ActivityId { get; set; }
        public long WorkPackageId { get; set; }
        public decimal Hours { get; set; }
        public string Comment { get; set; }
        public DateTime SpentOn { get; set; }
    }
}