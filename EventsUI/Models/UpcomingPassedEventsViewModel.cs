using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsUI.Models
{
    public class UpcomingPassedEventsViewModel
    {
        public IEnumerable<EventViewModel> UpcommingEvents { get; set; }
        public IEnumerable<EventViewModel> PassedEvents { get; set; }
    }
}