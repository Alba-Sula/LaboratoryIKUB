using EventsData;
using EventsUI.CustomValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace EventsUI.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDateTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public string Author { get; set; }
        public string Location { get; set; }
        public string Path { get; set; }

        public static Expression<Func<Event, EventViewModel>> ViewModel
        {
            get
            {
                return e => new EventViewModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    StartDateTime = e.StartDateTime,
                    Duration = e.Duration,
                    Author = e.Author.FullName,
                    Location = e.Location,
                    Path = e.Path,
                };
            }
        }
    }
}