using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EventsData
{
    public class Comment
    {
        public Comment()
        {
            this.Date = DateTime.Now;
        }
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public int EventId { get; set; }
        [Required]
        public virtual Event Event { get; set; }
    }
}
