using EventsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace EventsUI.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string AuthorId { get; set; }
        public static Expression<Func<Comment, CommentViewModel>> ViewModel
        {
            get
            {
                return c => new CommentViewModel()
                {
                    Id = c.Id,
                    Text = c.Text,
                    Author = c.Author.FullName,
                    AuthorId = c.AuthorId,
                };
            }
        }

    }
}