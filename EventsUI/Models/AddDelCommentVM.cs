using EventsData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventsUI.Models
{
    public class AddDelCommentVM
    {
        public AddDelCommentVM()
        {
            this.Date = DateTime.Now;
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "The text is required")]
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string AuthorId { get; set; }
        public int EventId { get; set; }
        public static Comment CreateFromAddDelCommVM(AddDelCommentVM c)
        {
            return new Comment()
            {
                Text = c.Text,
                Date = c.Date,
                AuthorId = c.AuthorId,
                EventId = c.EventId,
            };
        }

        public static AddDelCommentVM CreateFromComment(Comment c)
        {
            return new AddDelCommentVM()
            {
                Text = c.Text,
                Date = c.Date,
                AuthorId = c.AuthorId,
                EventId = c.EventId,
            };
        }
    }
}