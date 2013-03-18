using System;
using System.Collections.Generic;

namespace Rhinos.SampleProject.Models
{
    public class ScheduledWebinar
    {
        public string Id { get; set; }
        public string Title { get; set; }
        
        public DateTime ScheduledTimeUtc { get; set; }
    }
}