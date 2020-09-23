using EventsData;
using EventsUI.Extensions;
using EventsUI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventsUI.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(int PageNoUpcomming = 1, int PageNoPassed = 1)
        {
            var events = this.db.Events
                .OrderBy(e => e.StartDateTime)
                .Where(e => e.IsPublic)
                .Select(EventViewModel.ViewModel);

            var upcomingE = events.Where(e => e.StartDateTime > DateTime.Now);
            var passedE = events.Where(e => e.StartDateTime <= DateTime.Now);
            //pagination upcomming events
            int NoOfUpcommingEPerPage = 6;
            int NoOfUpcommingPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(upcomingE.Count()) / Convert.ToDouble(NoOfUpcommingEPerPage)));
            int NoOfUpcommingToSkip = (PageNoUpcomming - 1) * NoOfUpcommingEPerPage;
            ViewBag.PageNoUpcomming = PageNoUpcomming;
            ViewBag.NoOfUpcommingPages = NoOfUpcommingPages;
            var upcommingEvents = upcomingE.Skip(NoOfUpcommingToSkip).Take(NoOfUpcommingEPerPage);


            //pagination passed events
            int NoOfPassedEPerPage = 6;
            int NoOfPassedPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(passedE.Count()) / Convert.ToDouble(NoOfPassedEPerPage)));
            int NoOfPassedToSkip = (PageNoPassed - 1) * NoOfPassedEPerPage;
            ViewBag.PageNoPassed = PageNoPassed;
            ViewBag.NoOfPassedPages = NoOfPassedPages;
            var passedEvents = passedE.Skip(NoOfPassedToSkip).Take(NoOfPassedEPerPage);


            return View(new UpcomingPassedEventsViewModel()
            {
                UpcommingEvents = upcommingEvents,
                PassedEvents = passedEvents
            });
        }

        public ActionResult EventDetailsById(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var eventDetails = this.db.Events
                .Where(e => e.Id == id)
                .Where(e => e.IsPublic || isAdmin || (e.AuthorId != null && e.AuthorId == currentUserId))
                .Select(EventDetailsViewModel.ViewModel)
                .FirstOrDefault();

            var isOwner = (eventDetails != null && eventDetails.AuthorId != null && eventDetails.AuthorId == currentUserId);
            this.ViewBag.CanEdit = isOwner || isAdmin;
            this.ViewBag.User = currentUserId;
            this.ViewBag.IsAdmin = isAdmin;
            return this.PartialView("_EventDetails", eventDetails);
        }

        public ActionResult AddComment(int id)
        {
            TempData["EventId"] = id;
            return this.PartialView("_AddComment");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(AddDelCommentVM comment)
        {
            if (comment != null && ModelState.IsValid)
            {

                var currentUserId = this.User.Identity.GetUserId();
                var c = AddDelCommentVM.CreateFromAddDelCommVM(comment);
                if (currentUserId != null)
                {
                    c.AuthorId = currentUserId;
                }
                int eventID = Convert.ToInt32(TempData["EventId"]);
                c.Event = this.db.Events.Where(e => e.Id == eventID).FirstOrDefault();
                this.db.Comments.Add(c);
                this.db.SaveChanges();
                this.AddNotification("Comment Created", NotificationType.INFO);
                return this.RedirectToAction("Index", "Home");
            }
            this.AddNotification("Comment not created", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DeleteComment(int id)
        {
            var comment = this.db.Comments.Where(c => c.Id == id).FirstOrDefault();
            if (comment == null)
            {
                this.AddNotification("Cannot delete comment", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }

            this.db.Comments.Remove(comment);
            this.db.SaveChanges();
            this.AddNotification("Comment Deleted", NotificationType.INFO);
            return RedirectToAction("Index", "Home");
        }
    }
}