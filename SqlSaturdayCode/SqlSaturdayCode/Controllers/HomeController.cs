using System;
using System.Linq;
using System.Web.Mvc;
using Raven.Client;
using Raven.Client.Linq;
using SqlSaturdayCode.Helpers;
using SqlSaturdayCode.Models;

namespace SqlSaturdayCode.Controllers
{
    public class HomeController : RavenBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Speaker(int id)
        {
            var speaker = DocumentSession.Load<Speaker>(id);
            var queryResult = DocumentSession.Query<SessionsPerSpeaker.Result, SessionsPerSpeaker>()
                .FirstOrDefault(r => r.SpeakerId == speaker.Id);
            
            ViewBag.NumberOfSessions = 0;
            if (queryResult != null)
                ViewBag.NumberOfSessions = queryResult.NumberOfSessions;

            var sessionsForThisSpeaker = DocumentSession.Query<SessionsForSpeaker.Result, SessionsForSpeaker>()
                .OrderByDescending(e => e.EventDate)
                .Where(e => e.SpeakerId == speaker.Id)
                .ProjectFromIndexFieldsInto<SessionsForSpeaker.Result>()
                .Take(3)
                .ToList();

            ViewBag.Sessions = sessionsForThisSpeaker;
            return View(speaker);
        }

        public ActionResult ViewEvent(int id)
        {
            var evt = DocumentSession.Load<Event>(id);
            return View(evt);
        }

        public ActionResult ListEvents(string tag)
        {
            var query = DocumentSession.Query<Event>().OrderBy(e => e.Date);
            if (!string.IsNullOrWhiteSpace(tag))
            {
                query = query.Where(e => e.tags.Contains(tag));
            }
            var events = query
                .Take(10)
                .ToList();
            return View(events);
        }

        [HttpGet]
        public ActionResult EditEvent(int id)
        {
            var evt = DocumentSession.Load<Event>(id);

            return View(evt);
        }        
        
        [HttpPost]
        public ActionResult EditEvent(Event evt)
        {
            evt.Id = "events/" + evt.Id;
            DocumentSession.Store(evt);

            return RedirectToAction("ViewEvent", new {id = evt.Id.ToIntId()});
        }

        public ActionResult NewEvent()
        {
            var newEvent = new Event
            {
                Title = "SQL Saturday Oporto 2014",
                Description = "Greatest SQL Conference in the WORLD!",
                Date = new DateTimeOffset(2014, 10, 18, 09, 0, 0, new TimeSpan()),
                State = EventState.Published,
                tags = new[] { "sql", "portugal", "sqlsaturday", "databases", ".net", "conference", "tech" },
                Sessions = new[]
                {
                    new EventSession()
                    {
                        SpeakerId = "speakers/1",
                        Title = "Because the world is not just relational - Intro to RavenDB",
                        Description = "RavenDb is a .NET document database with an HTTP REST API, Lucene based indexes, ACID read/writes and BASE queries that's been picking up steam in the .NET space. In this session we'll see the very basics of using RavenDB from a web application, including reads, writes and simple queries. We'll see how RavenDB uses LINQ and Lucene to create indexes similar to materialized views to provide fast query-time responses, and what are the main differences in modeling data and entities from the perspective of a developer used to relational databases. ",
                        Tags = new []{"nosql", "database", "document-database", "beginner", "ravendb", "whoho"}
                        
                    }, 
                    new EventSession()
                    {
                        SpeakerId = "speakers/3",
                        Title = "ETL Patterns with Clustered Columnstore Indexes",
                        Description = "Join me for an hour of playing with different ETL patterns by using Clustered Columnstore Indexes. Using different Hardware might lead you to different conclusions,and the size of the workload is always the paramount of your performance. Loading data first and then creating a Clustered Columnstore or creating Clustered Columnstore and than loading - join me to find the answers! ",
                        Tags = new []{"sql", "database", "etl", "advanced", "columnwhat?", "join-me!"}
                        
                    }, 
                }
            };
            DocumentSession.Store(newEvent);
            return RedirectToAction("ViewEvent", new { id = newEvent.Id.ToIntId() });
        }
    }
}