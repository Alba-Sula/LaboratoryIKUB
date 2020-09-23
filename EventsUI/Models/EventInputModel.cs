using EventsData;
using EventsUI.CustomValidator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace EventsUI.Models
{
    public class EventInputModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Event Title is required")]
        [StringLength(200, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Title *")]
        public string Title { get; set; }
        [Display(Name = "Date and Time *")]
        [DataType(DataType.DateTime)]
        public DateTime StartDateTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public string AuthorId { get; set; }

        public string Description { get; set; }
        [MaxLength(200)]
        public string Location { get; set; }
        [Display(Name = "Is Public?")]
        public bool IsPublic { get; set; }
        public string Path { get; set; }
        [FileTypeValidator(ErrorMessage = "Your file must be of type .png, .jpeg, .jpg or .gif")]
        [ImgSizeValidator(ErrorMessage = "Your image cannot be larger than 200KB")]
        [HeightWidthImgValidation(ErrorMessage = "Height and width of the image must be between 100 and 400 pixels")]
        public HttpPostedFileBase Img { get; set; }

        public static EventInputModel CreateFromEvent(Event e)
        {
            return new EventInputModel()
            {
                Title = e.Title,
                StartDateTime = e.StartDateTime,
                Duration = e.Duration,
                Location = e.Location,
                Description = e.Description,
                IsPublic = e.IsPublic,
                Path = e.Path,
            };
        }

    }
}