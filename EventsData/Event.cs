﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EventsData
{
    public class Event
    {
        public Event()
        {
            this.IsPublic = true;
            this.StartDateTime = DateTime.Now;
            this.Comments = new HashSet<Comment>();

        }

        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public string Description { get; set; }
        [MaxLength(200)]
        public string Location { get; set; }
        public bool IsPublic { get; set; }
        public string Path { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }


    }
}
