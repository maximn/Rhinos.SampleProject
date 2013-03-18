using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Linq;
using Raven.Json.Linq;
using Rhinos.SampleProject.Authentication;
using Rhinos.SampleProject.Filters;
using Rhinos.SampleProject.Filters.RavenDb;
using Rhinos.SampleProject.Models;
using WebMatrix.WebData;
using YoutubeExtractor;

namespace Rhinos.SampleProject.Controllers
{
    [RavenDbSessionFilter]
    public class HomeController : Controller, IRavenDbSession
    {
        public IDocumentSession RavenDbSession { get; set; }

        public HomeController()
        {
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            if (login.UserName == "admin" && login.Password == "admin")
            {
                FormsAuthentication.SetAuthCookie(login.UserName, true);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            IEnumerable<ScheduledWebinar> scheduledWebinars = RavenDbSession.Query<ScheduledWebinar>()
                                                                            .OrderBy(webinar => webinar.ScheduledTimeUtc)
                                                                            .Take(4)
                                                                            .ToArray();

            return View(scheduledWebinars);
        }

        public ActionResult ScheduledWebinar(string id)
        {
            var questions = RavenDbSession.Query<Question>()
                                          .Customize(x => x.Include<Question>(o => o.WebinarId))
                                          .Where(question => question.WebinarId == id);

            var webinar = RavenDbSession.Load<ScheduledWebinar>(id);

            return View(new WebinarQuestions(webinar.Id, webinar.Title, questions));
        }

        private static object votesLock = new object();

        [HttpPost]
        public ActionResult VoteUp(string id)
        {
            //BUG :
            // Currently - Cannot modify a snapshot, this is probably a bug
            // Seems like a knowin issue and fixed in later version - 
            // https://groups.google.com/forum/?fromgroups=#!topic/ravendb/lD3YCCDzqyc
            // Adding the code that should be used after the RaveDb issue fixed-
            /*
            var documentStore = RavenDbSession.Advanced.DocumentStore;
            documentStore.DatabaseCommands.Patch(id, new[]
                                                        {
                                                            new PatchRequest()
                                                                {
                                                                    Type = PatchCommandType.Inc,
                                                                    Name = "Votes",
                                                                    Value = new RavenJValue(1)
                                                                }
                                                        });
             * */


            // For now I'll do the synchronization in the Web Server, note : this approach won't scale out
            lock (votesLock)
            {
                var question = RavenDbSession.Load<Question>(id);

                question.Votes++;

                RavenDbSession.Store(question);

                RavenDbSession.SaveChanges();
            }


            return new EmptyResult();
        }

        [HttpPost]
        [WebinarsAdministratorRequired]
        public ActionResult CreateWebinar(string title, DateTime scheduledDateUtc, DateTime scheduledTimeUtc)
        {
            var webinar = new ScheduledWebinar()
                            {
                                Title = title,
                                // Compose a DATETIME from seperated date/time the client provided
                                ScheduledTimeUtc = new DateTime(scheduledDateUtc.Year,
                                                                scheduledDateUtc.Month,
                                                                scheduledDateUtc.Day,
                                                                scheduledTimeUtc.Hour,
                                                                scheduledTimeUtc.Minute,
                                                                0,
                                                                DateTimeKind.Utc)
                            };

            RavenDbSession.Store(webinar);

            return RedirectToAction("ScheduledWebinar", "Home", new { id = webinar.Id });
        }


        [HttpPost]
        public ActionResult CreateQeustion(Question question)
        {
            question.Comments = Enumerable.Empty<Comment>();
            RavenDbSession.Store(question);

            return RedirectToAction("ScheduledWebinar", "Home", new { id = question.WebinarId });
        }

        public ActionResult AddComment(string webinarId, string questionId, Comment comment)
        {
            var documentStore = RavenDbSession.Advanced.DocumentStore;
            documentStore.DatabaseCommands.Patch(questionId, new[]
                                                        {
                                                            new PatchRequest()
                                                                {
                                                                    Type = PatchCommandType.Add,
                                                                    Name = "Comments",
                                                                    Value = RavenJObject.FromObject(comment)
                                                                }
                                                        });

            return RedirectToAction("ScheduledWebinar", new { id = webinarId });
        }
    }
}
