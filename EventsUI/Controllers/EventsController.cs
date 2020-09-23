using EventsData;
using EventsUI.Extensions;
using EventsUI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventsUI.Controllers
{
    [Authorize]
    public class EventsController : BaseController
    {
        // GET: Events/Create

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventInputModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                string dbPath = null;
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extention = Path.GetExtension(file.FileName);
                    fileName = "event-" + fileName + extention;
                    dbPath = "/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(fileName);

                }

                var e = new Event()
                {
                    AuthorId = this.User.Identity.GetUserId(),
                    Title = model.Title,
                    StartDateTime = model.StartDateTime,
                    Duration = model.Duration,
                    Description = model.Description,
                    Location = model.Location,
                    IsPublic = model.IsPublic,
                    Path = dbPath,
                };
                this.db.Events.Add(e);
                this.db.SaveChanges();
                this.AddNotification("Event Created", NotificationType.INFO);
                return this.RedirectToAction("My");
            }
            return this.View(model);
        }

        public ActionResult My(int PageNoUpcomming = 1, int PageNoPassed = 1)
        {
            string currentUserId = this.User.Identity.GetUserId();
            var events = this.db.Events
                .Where(e => e.AuthorId == currentUserId)
                .OrderBy(e => e.StartDateTime)
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

        public ActionResult Edit(int id)
        {
            var eventToEdit = this.LoadEvent(id);
            if (eventToEdit == null)
            {
                this.AddNotification("Cannot edit the event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }
            var model = EventInputModel.CreateFromEvent(eventToEdit);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EventInputModel model)
        {
            var eventToEdit = this.LoadEvent(id);
            if (eventToEdit == null)
            {
                this.AddNotification("Cannot edit event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            if (model != null && ModelState.IsValid)
            {
                eventToEdit.Title = model.Title;
                eventToEdit.StartDateTime = model.StartDateTime;
                eventToEdit.Description = model.Description;
                eventToEdit.Duration = model.Duration;
                eventToEdit.Location = model.Location;
                eventToEdit.IsPublic = model.IsPublic;

                this.db.SaveChanges();
                this.AddNotification("Event edited", NotificationType.INFO);
                return this.RedirectToAction("My");

            }

            return this.View(model);
        }

        public ActionResult Delete(int id)
        {
            var eventToDelete = this.LoadEvent(id);
            if (eventToDelete == null)
            {
                this.AddNotification("Cannot delete the event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }
            var model = EventInputModel.CreateFromEvent(eventToDelete);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, EventInputModel model)
        {
            var eventToDelete = this.LoadEvent(id);
            if (eventToDelete == null)
            {
                this.AddNotification("Cannot delete the event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }
            this.db.Events.Remove(eventToDelete);
            this.db.SaveChanges();
            this.AddNotification("Event deleted", NotificationType.INFO);
            return this.RedirectToAction("My");
        }

        public ActionResult Image(int id)
        {
            Event e = this.db.Events.Where(s => s.Id == id).FirstOrDefault();
            string dbPath = e.Path;
            var indexOfSlash = dbPath.IndexOf("/", 3);
            string fileName = dbPath.Substring(indexOfSlash + 1);
            string filePath = Path.Combine(Server.MapPath("~/Images/"), fileName);
            byte[] filedData = System.IO.File.ReadAllBytes(filePath);
            string contentType = MimeMapping.GetMimeMapping(filePath);
            return File(filedData, contentType, fileName);
        }

        private Event LoadEvent(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var eventToEdit = this.db.Events
                .Where(e => e.Id == id)
                .FirstOrDefault(e => e.AuthorId == currentUserId || isAdmin);
            return eventToEdit;
        }


    }
}