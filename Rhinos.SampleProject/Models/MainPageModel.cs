using System.Collections.Generic;

namespace Rhinos.SampleProject.Models
{
    public class MainPageModel
    {
        public IEnumerable<ScheduledWebinar> UpcomingWebinars { get; set; }
        public IEnumerable<Webinar> HisotricalWebinars { get; set; }

        public MainPageModel(IEnumerable<Webinar> hisotricalWebinars, IEnumerable<ScheduledWebinar> upcomingWebinars)
        {
            UpcomingWebinars = upcomingWebinars;
            HisotricalWebinars = hisotricalWebinars;
        }
    }
}