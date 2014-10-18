using System;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace SqlSaturdayCode.Models
{
    public class SessionsForSpeaker : AbstractIndexCreationTask<Event, SessionsForSpeaker.Result>
    {
        public class Result
        {
            public string SpeakerId { get; set; }
            public string EventTitle { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTimeOffset? EventDate { get; set; }
        }
        public SessionsForSpeaker()
        {
            Map = events => from evt in events
                from session in evt.Sessions
                select new Result
                {
                    SpeakerId = session.SpeakerId,
                    Title = session.Title,
                    Description = session.Description,
                    EventTitle = evt.Title,
                    EventDate = evt.Date
                };
            StoreAllFields(FieldStorage.Yes);
        }   
    }
}